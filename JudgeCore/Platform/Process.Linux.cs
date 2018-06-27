using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace JudgeCore.Platform
{
    public sealed class Linux : SandboxProcess
    {
        private int pid_t;
        private ulong memp, timep;
        const string sandbox_app = "/home/xiaoyang/Source/joj/Debug/netcoreapp2.0/JudgeL64.out";

        public override void Kill(int exitcode = 0)
        {
            if (inside.HasExited) return;

            var proc = Process.Start(new ProcessStartInfo
            {
                FileName = sandbox_app,
                Arguments = $"kill {pid_t} {exitcode} kip",
                RedirectStandardError = true,
                StandardErrorEncoding = Console.Error.Encoding
            });

            var ret = proc.StandardError.ReadToEnd();

            // Prevent zombie sub process
            proc.WaitForExit();
            inside.WaitForExit();
            Trace.WriteLine(ret.Trim());
        }

        public override void Start(StringBuilder _out = null, StringBuilder _err = null)
        {
            var tmp_args = StartInfo.Arguments;
            var tmp_fn = StartInfo.FileName;

            if (File.Exists("/tmp/judge_pipe"))
                File.Delete("/tmp/judge_pipe");

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
            //Trace.WriteLine(StartInfo.FileName + " " + StartInfo.Arguments);
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

        public override void RefreshState(ICompiler compiler)
        {
            if (read_result) return;
            var pipe_filename = "/tmp/judge_pipe."+pid_t;
            if (!File.Exists(pipe_filename))
                throw new NotImplementedException("This is not science.");
            var fp = File.ReadAllText(pipe_filename)
                .Trim()
                .Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            File.Delete(pipe_filename);
            for (int i = 0; i < fp.Length - 1; i++)
                Trace.WriteLine(fp[i]);
            
            if (fp.Length == 0)
            {
                Trace.WriteLine("Nothing traced.");
                timep = 0;
                memp = 0;
                ec = -1;
                read_result = true;
            }
            else
            {
                var list = fp[fp.Length - 1].Split(' ');
                memp = ulong.Parse(list[0]);
                timep = ulong.Parse(list[1]);
                ec = int.Parse(list[2]);
                read_result = true;
            }

            base.RefreshState(compiler);
        }

        public override bool OutOfLimit() => AssertAndReturn(false, "We shouldn't call OutOfLimit() function.", true);
        protected override ulong MaxMemoryCore() => AssertAndReturn(read_result, "State has been checked.", memp);
        protected override double TotalTimeCore() => AssertAndReturn(read_result, "State has been checked.", timep);
        protected override double RunningTimeCore() => (inside.ExitTime - inside.StartTime).TotalMilliseconds;
        
        public override void Watch() { }
        public override void WaitForExit() => inside.WaitForExit();
        public override bool WaitForExit(int len) => inside.WaitForExit(len);

        public override bool IsRuntimeError => Enum.IsDefined(typeof(ExitSignal), ec);
        public override bool IsTimeLimitExceeded => (int)timep > time_l || ec == 14 || ec == 24;

        public Linux()
        {
            if (Environment.OSVersion.Platform != PlatformID.Unix)
                throw new Win32Exception("平台读取错误");
        }

        public enum ExitSignal : int
        {
            SIGSYS = 31,
            SIGBUS = 10,
            SIGABT = 9,
            SIGILL = 4,
            SIGINT = 2,
            SIGFPE = 8,
            SIGSEGV = 11,
            SIGPIPE = 13,
        }
    }
}
