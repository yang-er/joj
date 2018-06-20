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
        protected Process inside;
        private int pid_t, memp, timep, ec;
        
        public override void Kill()
        {
            // UnsetSandbox(pid_t);
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
                StartInfo.Arguments = $"-t{time_l} -m{mem_l} -p{proc_l} -s/tmp/judge_pipe -ptrace -chroot /dest/{tmp_fn} {tmp_args}";
                StartInfo.FileName = "/home/xiaoyang/Source/joj/Debug/netcoreapp2.0/JudgeL64.out";
            }
            else
            {
                StartInfo.Arguments = $"-t{time_l} -m{mem_l} -p{proc_l} -s/tmp/judge_pipe {tmp_fn} {tmp_args}";
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
        public override void Watch() { }

        protected override double TotalTimeCore()
        {
            throw new NotImplementedException();
        }

        protected override double RunningTimeCore()
        {
            throw new NotImplementedException();
        }

        protected override bool HasExitedCore()
        {
            throw new NotImplementedException();
        }

        public override void WaitForExit()
        {
            throw new NotImplementedException();
        }

        public Linux()
        {
            if (Environment.OSVersion.Platform != PlatformID.Unix)
                throw new Win32Exception("平台读取错误");
        }
    }
}
