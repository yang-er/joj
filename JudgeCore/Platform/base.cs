using System;
using System.Diagnostics;
using System.IO;

namespace JudgeCore
{
    public interface IPlatform
    {
        /// <summary>
        /// 设置沙盒
        /// </summary>
        /// <param name="mem">内存限制</param>
        /// <param name="cpu">CPU时间限制</param>
        /// <param name="pl">进程数量限制</param>
        /// <returns>沙盒的标识符（句柄）</returns>
        IntPtr SetupSandbox(long mem, int cpu, int pl);

        /// <summary>
        /// 启动编译器
        /// </summary>
        /// <param name="proc">未启动的编译器进程</param>
        /// <returns>结束编译器进程的回调</returns>
        Action StartCompilerProcess(Process proc);

        /// <summary>
        /// 创建评测进程
        /// </summary>
        /// <param name="job">沙盒标识符</param>
        /// <param name="info">进程启动信息</param>
        /// <param name="stdout">标准输出流</param>
        /// <param name="stdin">标准输入流</param>
        /// <returns>创建出来的进程</returns>
        Process CreateJudgeProcess(IntPtr job, ProcessStartInfo info, out StreamReader stdout, out StreamWriter stdin);

        /// <summary>
        /// 检测内存使用峰值
        /// </summary>
        /// <param name="proc">寻找的进程</param>
        /// <returns>使用的内存大小 (Bytes)</returns>
        long PeakProcessMemoryInfo(Process proc);

        /// <summary>
        /// 关闭沙盒
        /// </summary>
        /// <param name="hJob">沙盒的唯一标识符</param>
        void UnsetSandbox(ref IntPtr hJob);
    }
}
