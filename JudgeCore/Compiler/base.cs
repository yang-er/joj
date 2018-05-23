using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using static JudgeCore.Helper;
using static JudgeCore.Platform.Win32;

namespace JudgeCore.Compiler
{
    public abstract class CompilerBase : ICompiler
    {
        public List<string> IncludePath { get; protected set; }
        public List<string> LibraryPath { get; protected set; }
        public List<string> Options { get; protected set; }
        public List<string> ToolchainPath { get; protected set; }
        public string MasterPath { get; protected set; } = "";
        public string StandardError { get; protected set; } = "";
        public string StandardOutput { get; protected set; } = "";
        public int ExitCode { get; protected set; } = 0;

        /// <summary>
        /// 读取编译器进程的输出
        /// </summary>
        /// <param name="proc">进程</param>
        protected void ReadCompileResult(Process proc)
        {
            IntPtr hJob = SetupSandbox(128, 1000, 10);
            proc.Start();
            AssignProcessToJobObject(hJob, proc.Handle);
            proc.WaitForExit(3000);

            if (!proc.HasExited)
            {
                StandardOutput = "Compile Timeout, may have some bad codes.";
            }
            else
            {
                StandardOutput = proc.StandardOutput.ReadToEnd().Trim();
                if (StandardOutput != "") WriteDebug(StandardOutput);
                StandardError = proc.StandardError.ReadToEnd().Trim();
                if (StandardError != "") WriteDebug(StandardError);
            }

            UnsetSandbox(hJob);
            ExitCode = proc.ExitCode;
            WriteDebug($"Compiler exited with status code {ExitCode}. ");
        }

        public bool Compile(string ans, Guid identify)
        {
            var file_name = identify.ToString("D");
            var file = new FileStream(file_name + ".cpp", FileMode.CreateNew);
            byte[] grp = Encoding.ASCII.GetBytes(ans);
            file.Write(grp, 0, grp.Length);
            file.Close();

            var ret = Compile(file_name + ".cpp");
            StandardError = StandardError.Replace(file_name, "main");
            StandardOutput = StandardOutput.Replace(file_name, "main");
            return ret;
        }

        public abstract bool Compile(string file);

        internal CompilerBase() { }

        public override string ToString()
        {
            return "Unknown Compiler";
        }
    }
}
