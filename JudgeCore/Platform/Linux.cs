using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace JudgeCore.Platform
{
    public class Linux : SandboxProcess
    {
        protected Process inside;
        private int pid_t, ec;
        private ulong memp, timep;
        bool read_result;
        
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
                StartInfo.Arguments = $"-t{time_l} -p{proc_l} -s/tmp/judge_pipe -ptrace -chroot /dest/{tmp_fn} {tmp_args}";
                StartInfo.FileName = "/home/xiaoyang/Source/joj/Debug/netcoreapp2.0/JudgeL64.out";
            }
            else
            {
                StartInfo.Arguments = $"-t{time_l} -p{proc_l} -s/tmp/judge_pipe {tmp_fn} {tmp_args}";
                StartInfo.FileName = "/home/xiaoyang/Source/joj/Debug/netcoreapp2.0/JudgeL64.out";
            }

            read_result = false;
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
        protected override int ExitCodeCore()
        {
            ReadResult();
            return ec;
        }

        protected override ulong MaxMemoryCore()
        {
            ReadResult();
            return memp;
        }
        public override void Watch() { }

        private void ReadResult()
        {
            if (read_result) return;
            if (!File.Exists("/tmp/judge_pipe"))
                throw new NotImplementedException("This is not science.");
            var fp = File.ReadAllText("/tmp/judge_pipe").Trim();
            File.Delete("/tmp/judge_pipe");
            var list = fp.Split(' ');
            memp = ulong.Parse(list[0]);
            timep = ulong.Parse(list[1]);
            ec = int.Parse(list[2]);
            read_result = true;
        }

        protected override double TotalTimeCore()
        {
            ReadResult();
            return timep;
        }

        protected override double RunningTimeCore() => (inside.ExitTime - inside.StartTime).TotalMilliseconds;

        protected override bool HasExitedCore() => inside.HasExited;

        public override void WaitForExit() => inside.WaitForExit();

        public Linux()
        {
            if (Environment.OSVersion.Platform != PlatformID.Unix)
                throw new Win32Exception("平台读取错误");
        }
    }
}
