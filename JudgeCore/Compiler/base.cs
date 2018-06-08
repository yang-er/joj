using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using static JudgeCore.Helper;

namespace JudgeCore
{
    public abstract class ICompiler
    {
        /// <summary>
        /// 编译器名称
        /// </summary>
        public string CompilerName { get; protected set; } = "Unknown Compiler";
        
        /// <summary>
        /// 引用头文件路径
        /// </summary>
        public List<string> IncludePath { get; protected set; }

        /// <summary>
        /// 链接文件路径
        /// </summary>
        public List<string> LibraryPath { get; protected set; }

        /// <summary>
        /// 编译器选项
        /// </summary>
        public List<string> Options { get; protected set; }

        /// <summary>
        /// 工具链路径
        /// </summary>
        public List<string> ToolchainPath { get; protected set; }

        /// <summary>
        /// 主路径
        /// </summary>
        public string MasterPath { get; protected set; } = "";

        /// <summary>
        /// 标准错误输出内容
        /// </summary>
        public string StandardError { get; protected set; } = "";

        /// <summary>
        /// 标准输出输出内容
        /// </summary>
        public string StandardOutput { get; protected set; } = "";

        /// <summary>
        /// 进程退出代码
        /// </summary>
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

        /// <summary>
        /// 编译
        /// </summary>
        /// <param name="ans">要编译的代码</param>
        /// <param name="identify">程序标识符</param>
        /// <returns>是否编译成功</returns>
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

        /// <summary>
        /// 编译
        /// </summary>
        /// <param name="file">编译的文件名称</param>
        /// <returns>是否编译成功</returns>
        public abstract bool Compile(string file);
        
        internal ICompiler() { }

        private string Replace(string inn)
        {
            if (Arguments is null) return inn;
            foreach (var a in Arguments.AllKeys)
                inn = inn.Replace($"{{{a}}}", Arguments[a]);
            return inn;
        }

        private NameValueCollection Arguments { get; set; }

        protected void LoadFromXml(XmlNode xml)
        {
            CompilerName = xml.SelectSingleNode("name").InnerText;

            Arguments = new NameValueCollection();
            foreach (XmlNode sub in xml.SelectSingleNode("args").ChildNodes)
                Arguments[sub.Attributes["name"].InnerText] = sub.InnerText;

            MasterPath = Replace(xml.SelectSingleNode("master").InnerText);

            Options = new List<string>();
            foreach (XmlNode sub in xml.SelectSingleNode("options").ChildNodes)
                Options.Add(sub.InnerText);

            ToolchainPath = new List<string>();
            foreach (XmlNode sub in xml.SelectSingleNode("toolchain").ChildNodes)
                ToolchainPath.Add(Replace(sub.InnerText));
            LibraryPath = new List<string>();
            foreach (XmlNode sub in xml.SelectSingleNode("library").ChildNodes)
                LibraryPath.Add(Replace(sub.InnerText));
            IncludePath = new List<string>();
            foreach (XmlNode sub in xml.SelectSingleNode("include").ChildNodes)
                IncludePath.Add(Replace(sub.InnerText));
        }

        public override string ToString()
        {
            return CompilerName;
        }

        protected string Test(string main, string args = "")
        {
            var proc = MakeProcess(ToolchainPath[0] + "\\" + main, args);
            proc.Start();
            Console.WriteLine();
            var val = proc.StandardError.ReadToEnd() + "\n" + proc.StandardOutput.ReadToEnd();
            Console.WriteLine(val.Trim());
            return val;
        }
    }
}
