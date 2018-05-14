using System;
using System.Collections.Generic;

namespace JudgeCore
{
    public interface ICompiler
    {
        /// <summary>
        /// 引用头文件路径
        /// </summary>
        List<string> IncludePath { get; }

        /// <summary>
        /// 链接文件路径
        /// </summary>
        List<string> LibraryPath { get; }

        /// <summary>
        /// 编译器选项
        /// </summary>
        List<string> Options { get; }

        /// <summary>
        /// 工具链路径
        /// </summary>
        List<string> ToolchainPath { get; }

        /// <summary>
        /// 主路径
        /// </summary>
        string MasterPath { get; }

        /// <summary>
        /// 标准错误输出内容
        /// </summary>
        string StandardError { get; }

        /// <summary>
        /// 标准输出输出内容
        /// </summary>
        string StandardOutput { get; }

        /// <summary>
        /// 进程退出代码
        /// </summary>
        int ExitCode { get; }

        /// <summary>
        /// 编译
        /// </summary>
        /// <param name="ans">要编译的代码</param>
        /// <param name="identify">程序标识符</param>
        /// <returns>是否编译成功</returns>
        bool Compile(string ans, Guid identify);

        /// <summary>
        /// 编译
        /// </summary>
        /// <param name="file">编译的文件名称</param>
        /// <returns>是否编译成功</returns>
        bool Compile(string file);
    }
}
