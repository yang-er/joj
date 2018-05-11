using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace JudgeCore.Compiler
{
    public class Msvc : ICompiler
    {
        private string vcv = "";
        private string winv = "";
        private string sku = "";
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
            Options.ForEach((str) => cl_args = cl_args + $" {str}");
            IncludePath.ForEach((str) => cl_args = cl_args + $" /I\"{str}\"");
            cl_args = cl_args + $" /c {file_name}.cpp /Fo{file_name}.obj";
            Debug.WriteLine("cl.exe" + cl_args);
            var cl = new Process();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            cl.StartInfo.RedirectStandardOutput = true;
            cl.StartInfo.StandardOutputEncoding = Encoding.GetEncoding(936);
            cl.StartInfo.RedirectStandardError = true;
            cl.StartInfo.StandardErrorEncoding = Encoding.GetEncoding(936);
            cl.StartInfo.CreateNoWindow = true;
            cl.StartInfo.FileName = ToolchainPath[0] + "\\cl.exe";
            cl.StartInfo.Arguments = cl_args;
            cl.Start();
            cl.WaitForExit();
            var stdout = cl.StandardOutput.ReadToEnd();
            Debug.WriteLine(stdout);
            var stderr = cl.StandardError.ReadToEnd();
            Debug.WriteLine(stderr);
            StandardOutput = stdout.Replace(file_name, "main");
            StandardError = stderr.Replace(file_name, "main");
            ExitCode = cl.ExitCode;
            if (ExitCode != 0) return false;

            // Link

            return true;
        }

        public Msvc()
        {
            var vs = new DirectoryInfo(vsdir);
            sku = vs.GetDirectories()[0].Name;
            var vc = new DirectoryInfo(vsdir + "\\VC\\Tools\\MSVC\\");
            vcv = vc.GetDirectories()[0].Name;
            var sdk = new DirectoryInfo(kit + "\\Source\\");
            winv = sdk.GetDirectories()[0].Name;

            MasterPath = $"{vsdir}\\VC\\Tools\\MSVC\\{vcv}";

            // Compiler Options
            Options = new List<string>
            {
                "/nologo",
                "/EHsc",
                "/DDEBUG",
            };

            // Solve include paths
            IncludePath = new List<string>
            {
                $"{vsdir}\\VC\\Tools\\MSVC\\{vcv}\\include",
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
                $"{MasterPath}\\bin\\Hostx64\\x64",
                $"{vsdir}\\Common7\\IDE",
                $"{kit}\\bin\\{winv}\\x64"
            };
        }

        public override string ToString()
        {
            return $"Microsoft Visual C++ v{vcv} with Windows 10 SDK v{winv}";
        }
    }
}
