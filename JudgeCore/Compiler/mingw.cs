using System;
using System.Collections.Generic;

namespace JudgeCore.Compiler
{
    public sealed class MinGW : CompilerBase
    {
        private string ver;

        public override bool Compile(string file)
        {
            var file_name = file.Split('.')[0];

            // Compile
            string cl_args = "";
            Options.ForEach((str) => cl_args += $" {str}");
            cl_args += $" -c {file} -o{file_name}.o";
            var cl = Helper.MakeProcess(ToolchainPath[0] + "\\g++.exe", cl_args);
            cl.StartInfo.Environment["PATH"] += $"{MasterPath}\\bin;";
            ReadCompileResult(cl);
            if (ExitCode != 0) return false;

            // Link
            string link_args = "";
            link_args += $" -o {file_name}.exe {file_name}.o";
            var link = Helper.MakeProcess(ToolchainPath[0] + "\\g++.exe", link_args);
            link.StartInfo.Environment["PATH"] += $"{MasterPath}\\bin;";
            ReadCompileResult(link);
            if (ExitCode != 0) return false;

            return true;
        }

        public MinGW()
        {
            MasterPath = "C:\\MinGW";

            // Compiler Options
            Options = new List<string>
            {
                "-std=c++11",
            };

            // Solve include paths
            IncludePath = new List<string>{};

            // Solve linker paths
            LibraryPath = new List<string>{};

            // Toolchain paths
            ToolchainPath = new List<string>
            {
                $"{MasterPath}\\bin",
            };

            var proc = Helper.MakeProcess(ToolchainPath[0] + "\\gcc.exe", "--version");
            proc.Start();
            var ret = proc.StandardOutput.ReadToEnd().Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            ver = ret[0].Split(") ")[1];

            Console.WriteLine(ToString() + "\tloaded.");
        }

        public override string ToString()
        {
            return $"MinGW Toolchain v{ver}";
        }
    }
}
