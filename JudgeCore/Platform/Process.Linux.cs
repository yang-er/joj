using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace JudgeCore.Platform
{
    public sealed class Linux : SandboxProcess
    {
        private int pid_t, ec;
        private ulong memp, timep;
        bool read_result;
        const string sandbox_app = "/home/xiaoyang/Source/joj/Debug/netcoreapp2.0/JudgeL64.out";

        public override void Kill(int exitcode = 0)
        {
            if (inside.HasExited) return;
            var si = new ProcessStartInfo();
            si.FileName = sandbox_app;
            si.Arguments = $"kill {pid_t} {exitcode} kip";
            si.RedirectStandardError = true;
            si.StandardErrorEncoding = Console.Error.Encoding;
            var proc = Process.Start(si);
            var ret = proc.StandardError.ReadToEnd();
            proc.WaitForExit();
            Trace.WriteLine(ret);
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
                StartInfo.Arguments = $"std -t{time_l} -m{mem_l} -p{proc_l} -l0 -s/tmp/judge_pipe -pt -ch /dest/{tmp_fn} {tmp_args}";
            }
            else
            {
                StartInfo.Arguments = $"std -t{time_l} -m{mem_l} -p{proc_l} -s/tmp/judge_pipe {tmp_fn} {tmp_args}";
            }

            read_result = false;
            StartInfo.FileName = sandbox_app;
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
            var fp = File.ReadAllText("/tmp/judge_pipe").Trim().Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            File.Delete("/tmp/judge_pipe");
            for (int i = 0; i < fp.Length - 1; i++)
                Trace.WriteLine(fp[i]);
            if (fp.Length == 0)
                throw new NotImplementedException("Nothing traced.");
            var list = fp[fp.Length - 1].Split(' ');
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
        
        public override void WaitForExit() => inside.WaitForExit();

        public override bool IsRuntimeError
        {
            get
            {
                switch (ExitCodeCore())
                {
                    case 31:   // SIGSYS
                    case 4:    // SIGILL
                    case 2:    // SIGINT
                    case 8:    // SIGFPE
                    case 11:   // SIGSEGV
                        return true;
                    default:
                        return false;
                }
            }
        }

        public override bool IsTimeLimitExceeded => (int)timep > time_l || ec == 14 || ec == 24;

        public Linux()
        {
            if (Environment.OSVersion.Platform != PlatformID.Unix)
                throw new Win32Exception("平台读取错误");
        }
    }
}
