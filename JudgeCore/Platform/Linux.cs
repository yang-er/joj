using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JudgeCore.Platform
{
    public sealed partial class Linux : SandboxProcess
    {
        private int pid_t, memp, timep, ec;
        private DateTime startTime, endTime;
        private bool _exited = false;
        private Task wait_for_exit_task = new Task(() => { });

        public override void Kill()
        {
            UnsetSandbox(pid_t);
        }

        public override bool OutOfLimit()
        {
            throw new PlatformNotSupportedException();
        }
        
        public override void Start(StringBuilder _out = null, StringBuilder _err = null)
        {
            var tmp_args = StartInfo.Arguments;
            var tmp_fn = StartInfo.FileName;

            if (PTrace)
            {
                StartInfo.WorkingDirectory = "/";
                // StartInfo.Arguments = $"-p{proc_l} -ptrace -chroot /dest/{tmp_fn} {tmp_args}";
                // StartInfo.FileName = "/home/xiaoyang/Source/joj/Debug/netcoreapp2.0/JudgeL64.out";
            }
            else
            {
                // StartInfo.Arguments = $"-p{proc_l} {tmp_fn} {tmp_args}";
                // StartInfo.FileName = "/home/xiaoyang/Source/joj/Debug/netcoreapp2.0/JudgeL64.out";
            }

            Trace.WriteLine(StartInfo.FileName + " " + StartInfo.Arguments);
            if (!Start(StartInfo)) throw new Win32Exception();
            StartInfo.FileName = tmp_fn;
            StartInfo.Arguments = tmp_args;
            
            if (info.RedirectStandardError)
            {
                if (_err != null)
                {
                    _error = new AsyncStreamReader(
                        stderr.BaseStream,
                        (s) => StreamPipe(_err, s),
                        StartInfo.StandardErrorEncoding);
                    _error.BeginReadLine();
                }
            }

            if (info.RedirectStandardOutput)
            {
                if (_out != null)
                {
                    _output = new AsyncStreamReader(
                        stderr.BaseStream,
                        (s) => StreamPipe(_out, s),
                        StartInfo.StandardOutputEncoding);
                    _output.BeginReadLine();
                }
            }
        }
        
        protected override int ExitCodeCore() => ec;
        protected override ulong MaxMemoryCore() => (ulong)memp;
        protected override double TotalTimeCore() => timep;
        public override void WaitForExit() => Task.WaitAll(wait_for_exit_task);
        public override bool WaitForExit(int len)=> Task.WaitAll(new Task[] { wait_for_exit_task }, len);
        
        public override void Watch()
        {
            startTime = DateTime.Now;
            _exited = false;
            WatchSandbox((uint)mem_l, (uint)time_l, pid_t, ref memp, ref timep, ref ec, false);
            endTime = DateTime.Now;
            _exited = true;
            wait_for_exit_task.Start();
        }

        protected override double RunningTimeCore()
        {
            return ((_exited ? endTime : DateTime.Now) - startTime).TotalMilliseconds;
        }

        protected override bool HasExitedCore() => _exited;

        public Linux()
        {
            if (Environment.OSVersion.Platform != PlatformID.Unix)
                throw new Win32Exception("平台读取错误");
        }

        #region P/Invoke

        const int StreamBufferSize = 4096;

        private static readonly ReaderWriterLockSlim s_processStartLock = new ReaderWriterLockSlim();

        [DllImport("JudgeL64.so", EntryPoint = "setup_sandbox")]
        private static extern unsafe int SetupSandbox(
            uint mem, uint time, uint proc,
            bool to_chroot, string to_chdir, bool to_ptrace,
            string fn, byte** argv, byte** envp,
            bool redIn, bool redOut, bool redErr,
            out int std_in, out int std_out, out int std_err);

        [DllImport("JudgeL64.so", EntryPoint = "watch_sandbox")]
        public static extern void WatchSandbox(
            uint mem, uint time, int app,
            ref int max_mem, ref int max_time, ref int exitcode,
            bool pf);

        [DllImport("JudgeL64.so", EntryPoint = "unset_sandbox")]
        public static extern void UnsetSandbox(int app);

        public static unsafe int SetupSandbox(
            uint mem, uint time, uint proc,
            bool to_chroot, string to_chdir, bool to_ptrace,
            string fn, string[] argv, string[] envp,
            bool redIn, bool redOut, bool redErr,
            out int std_in, out int std_out, out int std_err)
        {
            byte** argvPtr = null, envpPtr = null;
            try
            {
                AllocNullTerminatedArray(argv, ref argvPtr);
                AllocNullTerminatedArray(envp, ref envpPtr);
                return SetupSandbox(
                    mem, time, proc, to_chroot, to_chdir, to_ptrace,
                    fn, argvPtr, envpPtr, redIn, redOut, redErr,
                    out std_in, out std_out, out std_err);
            }
            finally
            {
                FreeArray(envpPtr, envp.Length);
                FreeArray(argvPtr, argv.Length);
            }
        }

        #endregion
    }
}
