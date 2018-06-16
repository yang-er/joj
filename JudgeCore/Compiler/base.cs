using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Xml;
using static JudgeCore.Helper;

namespace JudgeCore
{
    /// <summary>
    /// 提供编译器对应方法
    /// </summary>
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
        /// 文件后缀名
        /// </summary>
        public string Appendix { get; protected set; } = ".exe";

        /// <summary>
        /// XML 中的编译器参数
        /// </summary>
        public NameValueCollection Arguments { get; set; }

        /// <summary>
        /// 读取编译器进程的输出
        /// </summary>
        /// <param name="proc">进程</param>
        protected void ReadCompileResult(SandboxProcess proc)
        {
            var stdout = new StringBuilder();
            var stderr = new StringBuilder();
            proc.Setup(128, 1000, 10);
            proc.Start(stdout, stderr);
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

            proc.Kill();
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
        
        /// <summary>
        /// XML 参数转化为实际值
        /// </summary>
        /// <param name="inn">输入的字符串</param>
        /// <returns>输出</returns>
        private string Replace(string inn)
        {
            if (Arguments is null) return inn;
            foreach (var a in Arguments.AllKeys)
                inn = inn.Replace($"{{{a}}}", Arguments[a]);
            return inn;
        }

        /// <summary>
        /// 从 XML 中加载编译器信息
        /// </summary>
        /// <param name="xml">XML节点</param>
        protected void LoadFromXml(XmlNode xml)
        {
            CompilerName = xml.SelectSingleNode("name").InnerText;

            Arguments = new NameValueCollection();
            foreach (XmlNode sub in xml.SelectSingleNode("args").ChildNodes)
                Arguments[sub.Attributes["name"].InnerText] = sub.InnerText;

            MasterPath = Replace(xml.SelectSingleNode("master").InnerText);
            Arguments["Master"] = MasterPath;

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

        /// <summary>
        /// 从对应 XML 节点中选择编译器。
        /// </summary>
        /// <param name="node">XML 节点</param>
        /// <returns>编译器实例</returns>
        /// <exception cref="NotImplementedException" />
        /// <exception cref="FileNotFoundException" />
        public static ICompiler GetFromXml(XmlNode node)
        {
            if (node.Name == "Msvc")
                return new Compiler.Msvc(node);
            else if (node.Name == "MinGW")
                return new Compiler.MinGW(node);
            else if (node.Name == "GNU")
                return new Compiler.GNU(node);
            else
                throw new NotImplementedException("This kind Compiler not supported.");
        }

        /// <summary>
        /// 返回编译器的表示串
        /// </summary>
        /// <returns>编译器名称</returns>
        public override string ToString()
        {
            return CompilerName;
        }

        /// <summary>
        /// 测试编译器是否可用
        /// </summary>
        /// <param name="main">主程序名称</param>
        /// <param name="args">传入参数</param>
        /// <returns>调用结果</returns>
        protected string Test(string main, string args = "")
        {
            WriteDebug("");
            var proc = MakeProcess(ToolchainPath[0] + "\\" + main, args);
            proc.Start();
            var val = proc.StandardError.ReadToEnd() + proc.StandardOutput.ReadToEnd();
            WriteDebug(val.Trim());
            return val;
        }
        
        /// <summary>
        /// 创建编译器进程实例
        /// </summary>
        /// <param name="filename">编译器文件</param>
        /// <param name="arguments">编译器参数</param>
        /// <returns>进程实例</returns>
        protected SandboxProcess MakeProcess(string filename, string arguments = "")
        {
            WriteDebug(filename + " " + arguments);
            return SandboxProcess.Create(filename, arguments, true, true);
        }

        /// <summary>
        /// 设置测评进程的参数
        /// </summary>
        /// <param name="proc">测评进程</param>
        public virtual void SetJudgeEnv(SandboxProcess proc) { }

        /// <summary>
        /// 创建评测进程
        /// </summary>
        /// <param name="guid">唯一标识符</param>
        /// <returns>评测进程</returns>
        public SandboxProcess CreateJudgeProcess(Guid guid)
        {
            return CreateJudgeProcess(guid.ToString("D"));
        }

        /// <summary>
        /// 创建评测进程
        /// </summary>
        /// <param name="filename">文件名称</param>
        /// <returns>评测进程</returns>
        public virtual SandboxProcess CreateJudgeProcess(string filename)
        {
            return SandboxProcess.Create(filename + Appendix, stdin: true, cd: true);
        }
    }
}
