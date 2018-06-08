﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using static JudgeCore.Helper;

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
            var stdout = new StringBuilder();
            var stderr = new StringBuilder();
            var callback = OS.StartCompilerProcess(proc);
            proc.OutputDataReceived += (sender, e) => stdout.AppendLine(e.Data);
            proc.ErrorDataReceived += (sender, e) => stderr.AppendLine(e.Data);
            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();
            proc.WaitForExit(3000);

            if (!proc.HasExited)
            {
                StandardOutput = "Compile Timeout, may have some bad codes.";
            }
            else
            {
                StandardOutput = stdout.ToString().Trim();
                if (StandardOutput != "") WriteDebug(StandardOutput);
                StandardError = stderr.ToString().Trim();
                if (StandardError != "") WriteDebug(StandardError);
            }

            callback();
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
            StandardError = StandardError
                .Replace(file_name, "main")
                .Replace(MasterPath, GetType().Name);
            StandardOutput = StandardOutput
                .Replace(file_name, "main")
                .Replace(MasterPath, GetType().Name);
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
