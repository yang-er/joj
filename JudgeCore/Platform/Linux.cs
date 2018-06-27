using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace JudgeCore.Platform
{
    public class Linux : SandboxProcess
    {
        private int pid_t, memp, timep, ec;

        [DllImport("JudgeL64.so", EntryPoint = "watch_sandbox")]
        public static extern void WatchSandbox(uint mem, uint time, int app, ref int max_mem, ref int max_time, ref int exitcode, bool pf);

        [DllImport("JudgeL64.so", EntryPoint = "unset_sandbox")]
        public static extern void UnsetSandbox(int app);
        
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
                StartInfo.Arguments = $"-p{proc_l} -ptrace -chroot /dest/{tmp_fn} {tmp_args}";
                StartInfo.FileName = "/home/xiaoyang/Source/joj/Debug/netcoreapp2.0/JudgeL64.out";
            }
            else
            {
                StartInfo.Arguments = $"-p{proc_l} {tmp_fn} {tmp_args}";
                StartInfo.FileName = "/home/xiaoyang/Source/joj/Debug/netcoreapp2.0/JudgeL64.out";
            }

            Trace.WriteLine(StartInfo.FileName + " " + StartInfo.Arguments);
            inside = Process.Start(StartInfo);
            pid_t = inside.Id;
            StartInfo.FileName = tmp_fn;
            StartInfo.Arguments = tmp_args;

            if (info.RedirectStandardInput)
                stdin = inside.StandardInput;

            if (info.RedirectStandardError)
            {
                if (_err is null)
                {
                    stderr = inside.StandardError;
                }
                else
                {
                    inside.ErrorDataReceived += (s, e) => StreamPipe(_err, e);
                    inside.BeginErrorReadLine();
                }
            }

            if (info.RedirectStandardOutput)
            {
                if (_out is null)
                {
                    stdout = inside.StandardOutput;
                }
                else
                {
                    inside.OutputDataReceived += (s, e) => StreamPipe(_out, e);
                    inside.BeginOutputReadLine();
                }
            }
        }

        public override bool WaitForExit(int len = -1) => inside.WaitForExit(len);
        protected override int ExitCodeCore() => ec;
        protected override ulong MaxMemoryCore() => (ulong)memp;
        public override void Watch() => WatchSandbox((uint)mem_l, (uint)time_l, pid_t, ref memp, ref timep, ref ec, false);

        protected override double TotalTimeCore() => timep;

        protected override double RunningTimeCore()
        {
            throw new NotImplementedException();
        }

        public override void WaitForExit() => inside.WaitForExit();
        
        public Linux()
        {
            if (Environment.OSVersion.Platform != PlatformID.Unix)
                throw new Win32Exception("平台读取错误");
        }
    }
}
