using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace JudgeCore.Compiler
{
    public class MinGW : ICompiler
    {
        private string Version;
        public List<string> IncludePath { get; }
        public List<string> LibraryPath { get; }
        public List<string> Options { get; }
        public List<string> ToolchainPath { get; }
        public string MasterPath => "C:\\MinGW";
        public string StandardError { get; set; }
        public string StandardOutput { get; set; }
        public int ExitCode { get; set; }

        public bool Compile(string ans, Guid identify)
        {
            var file_name = identify.ToString("D");
            var file = new FileStream(file_name + ".cpp", FileMode.CreateNew);
            byte[] grp = Encoding.ASCII.GetBytes(ans);
            file.Write(grp, 0, grp.Length);
            file.Close();

            // Compile
            string cl_args = "";
            Options.ForEach((str) => cl_args += $" {str}");
            cl_args += $" -c {file_name}.cpp -o{file_name}.o";
            var cl = Helper.MakeProcess(ToolchainPath[0] + "\\g++.exe", cl_args);
            cl.StartInfo.Environment["PATH"] += $"{MasterPath}\\bin;";
            cl.Start();
            cl.WaitForExit();
            var stdout = cl.StandardOutput.ReadToEnd();
            Debug.WriteLine(stdout);
            var stderr = cl.StandardError.ReadToEnd();
            Debug.WriteLine(stderr);
            StandardOutput = stdout.Replace(file_name, "main").Trim();
            StandardError = stderr.Replace(file_name, "main").Trim();
            ExitCode = cl.ExitCode;
            Debug.WriteLine("g++.exe exited with status code {0}. ", ExitCode);
            if (ExitCode != 0) return false;

            // Link
            string link_args = "";
            link_args += $" -o {file_name}.exe {file_name}.o";
            var link = Helper.MakeProcess(ToolchainPath[0] + "\\g++.exe", link_args);
            link.StartInfo.Environment["PATH"] += $"{MasterPath}\\bin;";
            link.Start();
            link.WaitForExit();
            stdout = link.StandardOutput.ReadToEnd();
            Debug.WriteLine(stdout);
            stderr = link.StandardError.ReadToEnd();
            Debug.WriteLine(stderr);
            StandardOutput += stdout.Replace(file_name, "main");
            StandardError += stderr.Replace(file_name, "main");
            ExitCode = link.ExitCode;
            Debug.WriteLine("g++.exe exited with status code {0}. ", ExitCode);
            if (ExitCode != 0) return false;

            return true;
        }

        public MinGW()
        {
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
            Version = ret[0].Split(") ")[1];

            Console.WriteLine(ToString() + "\tloaded.");
        }

        public override string ToString()
        {
            return $"MinGW Toolchain v{Version}";
        }
    }
}
