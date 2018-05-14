﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace JudgeCore.Compiler
{
    public class ClangC2 : ICompiler
    {
        private string vcv = "";
        private string msvcv = "";
        private string winv = "";
        private string sku = "";
        private string clangv = "";
        private string c2v = "";
        private string kit = "C:\\Program Files (x86)\\Windows Kits\\10";
        private string vsdir => $"C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\{sku}";

        public List<string> IncludePath { get; }
        public List<string> LibraryPath { get; }
        public List<string> Options { get; }
        public List<string> ToolchainPath { get; }
        public string MasterPath { get; }
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
            IncludePath.ForEach((str) => cl_args += $" -internal-isystem \"{str.Replace("\\", "\\\\")}\"");
            cl_args += $" -c {file_name}.cpp -o {file_name}.obj";
            var cl = Helper.MakeProcess(ToolchainPath[0] + "\\clang.exe", cl_args);
            cl.StartInfo.Environment["PATH"] += $"{vsdir}\\VC\\Tools\\MSVC\\{msvcv}\\bin\\HostX64\\x64;";
            cl.Start();
            cl.WaitForExit();
            var stdout = cl.StandardOutput.ReadToEnd();
            Debug.WriteLine(stdout);
            var stderr = cl.StandardError.ReadToEnd();
            Debug.WriteLine(stderr);
            StandardOutput = stdout.Replace(file_name, "main").Trim();
            StandardError = stderr.Replace(file_name, "main").Trim();
            ExitCode = cl.ExitCode;
            Debug.WriteLine("clang.exe exited with status code {0}. ", ExitCode);
            if (ExitCode != 0) return false;

            // Link
            string link_args = "";
            // LibraryPath.ForEach((str) => link_args += $" /LIBPATH:\"{str}\"");
            link_args += $" -o {file_name}.exe {file_name}.obj";
            var link = Helper.MakeProcess(ToolchainPath[0] + "\\clang.exe", link_args);
            link.StartInfo.Environment["PATH"] += $"{vsdir}\\VC\\Tools\\MSVC\\{msvcv}\\bin\\HostX64\\x64;";
            link.Start();
            link.WaitForExit();
            stdout = link.StandardOutput.ReadToEnd();
            Debug.WriteLine(stdout);
            stderr = link.StandardError.ReadToEnd();
            Debug.WriteLine(stderr);
            StandardOutput += stdout.Replace(file_name, "main");
            StandardError += stderr.Replace(file_name, "main");
            ExitCode = link.ExitCode;
            Debug.WriteLine("clang.exe exited with status code {0}. ", ExitCode);
            if (ExitCode != 0) return false;

            return true;
        }

        public ClangC2()
        {
            var vs = new DirectoryInfo(vsdir);
            sku = vs.GetDirectories()[0].Name;
            var vc = new DirectoryInfo(vsdir + "\\VC\\Tools\\ClangC2\\");
            vcv = vc.GetDirectories()[0].Name;
            var sdk = new DirectoryInfo(kit + "\\Source\\");
            winv = sdk.GetDirectories()[0].Name;
            msvcv = new DirectoryInfo(vsdir + "\\VC\\Tools\\MSVC\\").GetDirectories()[0].Name;

            MasterPath = $"{vsdir}\\VC\\Tools\\ClangC2\\{vcv}";

            // Compiler Options
            Options = new List<string>
            {
            };

            // Solve include paths
            IncludePath = new List<string>
            {
                $"{vsdir}\\VC\\Tools\\ClangC2\\{vcv}\\include",
                $"{vsdir}\\VC\\Tools\\MSVC\\{msvcv}\\include",
                $"{kit}\\Include\\{winv}\\ucrt",
            };

            // Solve linker paths
            LibraryPath = new List<string>
            {
                $"{MasterPath}\\lib\\x64",
                $"{kit}\\Lib\\{winv}\\ucrt\\x64",
                $"{kit}\\Lib\\{winv}\\um\\x64",
            };

            // Toolchain paths
            ToolchainPath = new List<string>
            {
                $"{MasterPath}\\bin\\HostX64",
                // $"{vsdir}\\VC\\Tools\\MSVC\\{msvcv}\\bin\\HostX64\\x64",
                $"{vsdir}\\Common7\\IDE",
                $"{kit}\\bin\\{winv}\\x64"
            };

            // Get environment
            var clang = Helper.MakeProcess(ToolchainPath[0] + "\\clang.exe", "--version");
            clang.StartInfo.Environment["PATH"] += $"{vsdir}\\VC\\Tools\\MSVC\\{msvcv}\\bin\\HostX64\\x64;";
            clang.Start();
            var ppp = clang.StandardOutput.ReadToEnd().Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            clangv = ppp[0].Split(" version ")[1];
            if (!ppp[2].StartsWith(MasterPath))
            {
                Console.WriteLine("Clang/C2 has some wrong things, please check...");
            }
            else
            {
                c2v = ppp[2].Split(" version ")[1];
            }

            Console.WriteLine(ToString() + "\tloaded.");
        }

        public override string ToString()
        {
            return $"Clang with Microsoft CodeGen v{clangv}, c2 v{c2v}";
        }
    }
}
