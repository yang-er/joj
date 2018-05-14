using System;
using System.Collections.Generic;
using System.IO;

namespace JudgeCore.Compiler
{
    public sealed class Msvc : CompilerBase
    {
        private string vcv = "";
        private string winv = "";
        private string sku = "";
        private string kit = "C:\\Program Files (x86)\\Windows Kits\\10";
        private string vsdir => $"C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\{sku}";
        
        public override bool Compile(string file)
        {
            var file_name = file.Split('.')[0];

            // Compile
            string cl_args = "";
            Options.ForEach((str) => cl_args += $" {str}");
            IncludePath.ForEach((str) => cl_args += $" /I\"{str}\"");
            cl_args += $" /c {file} /Fo{file_name}.obj";
            var cl = Helper.MakeProcess(ToolchainPath[0] + "\\cl.exe", cl_args);
            ReadCompileResult(cl);
            if (ExitCode != 0) return false;

            // Link
            string link_args = " " + Options[0];
            LibraryPath.ForEach((str) => link_args += $" /LIBPATH:\"{str}\"");
            link_args += $" /out:{file_name}.exe {file_name}.obj";
            var link = Helper.MakeProcess(ToolchainPath[0] + "\\link.exe", link_args);
            ReadCompileResult(link);
            if (ExitCode != 0) return false;

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

            Console.WriteLine(ToString() + "\tloaded.");
        }

        public override string ToString()
        {
            return $"Microsoft Visual C++ v{vcv} with Windows 10 SDK v{winv}";
        }
    }
}
