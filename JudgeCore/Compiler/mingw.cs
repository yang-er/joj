using System;
using System.Collections.Generic;
using System.Xml;

namespace JudgeCore.Compiler
{
    public sealed class MinGW : ICompiler
    {
        public override bool Compile(string file)
        {
            var file_name = file.Split('.')[0];

            // Compile
            string cl_args = "";
            Options.ForEach((str) => cl_args += $" {str}");
            cl_args += $" -c {file} -o{file_name}.o";
            var cl = MakeProcess(ToolchainPath[0] + "\\g++.exe", cl_args);
            var path_env = $"{MasterPath}\\bin;" + cl.StartInfo.Environment["PATH"];
            cl.StartInfo.Environment["PATH"] = path_env;
            ReadCompileResult(cl);
            if (ExitCode != 0) return false;

            // Link
            string link_args = "";
            link_args += $" -o {file_name}.exe {file_name}.o -lm";
            var link = MakeProcess(ToolchainPath[0] + "\\g++.exe", link_args);
            link.StartInfo.Environment["PATH"] = path_env;
            ReadCompileResult(link);
            if (ExitCode != 0) return false;

            return true;
        }

        public MinGW(XmlNode xml)
        {
            LoadFromXml(xml);
            Test("gcc.exe", "--version");
        }

        public override SandboxProcess CreateJudgeProcess(string filename)
        {
            var ret = base.CreateJudgeProcess(filename);
            if (ret != null)
                ret.StartInfo.Environment["PATH"] = ToolchainPath[0] + ";";
            return ret;
        }

        public override void CheckStandardError(string err, ref int ec)
        {
            if (ec == 4194432) ec = (int)Platform.WinNT.ErrorCode.SegmentFault;
            //throw new NotImplementedException();
        }

        [Obsolete("This kind of access is not flexible", true)]
        public MinGW()
        {
            MasterPath = "C:\\MinGW";
            Appendix = ".exe";

            // Compiler Options
            Options = new List<string>
            {
                "-std=c++11",
                "-fmax-errors=5",
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

            var ret = Test("gcc.exe", "--version");
            var ver = ret.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[0].Split(") ")[1];
            CompilerName = $"MinGW Toolchain v{ver}";
        }
    }
}
