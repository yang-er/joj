using System.Xml;

namespace JudgeCore.Compiler
{
    public sealed class GNU : ICompiler
    {
        public override bool Compile(string file)
        {
            var file_name = file.Split('.')[0];

            // Compile
            string cl_args = "";
            Options.ForEach((str) => cl_args += $" {str}");
            cl_args += $" -c {file} -o{file_name}.o";
            var cl = MakeProcess(ToolchainPath[0] + "/g++", cl_args);
            var path_env = $"{MasterPath}/bin;" + cl.StartInfo.Environment["PATH"];
            cl.StartInfo.Environment["PATH"] = path_env;
            ReadCompileResult(cl);
            if (ExitCode != 0) return false;

            // Link
            string link_args = "";
            link_args += $" -o {file_name}.out {file_name}.o -lm";
            var link = MakeProcess(ToolchainPath[0] + "/g++", link_args);
            link.StartInfo.Environment["PATH"] = path_env;
            ReadCompileResult(link);
            if (ExitCode != 0) return false;

            return true;
        }

        public GNU(XmlNode xml)
        {
            LoadFromXml(xml);
            Test("gcc", "--version");
        }

        public override SandboxProcess CreateJudgeProcess(string filename)
        {
            var ret = base.CreateJudgeProcess(filename);
            ret.StartInfo.Environment["PATH"] = ToolchainPath[0] + ";";
            return ret;
        }
    }
}
