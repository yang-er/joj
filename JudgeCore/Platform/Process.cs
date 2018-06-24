using JudgeCore.Platform;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace JudgeCore
{
    /// <summary>
    /// 沙盒进程
    /// </summary>
    public abstract class SandboxProcess : Component
    {
        protected ProcessStartInfo info;
        protected ulong mem_l;
        protected int time_l, proc_l;
        protected StreamReader stdout, stderr;
        protected StreamWriter stdin;
        protected Process inside;
        protected abstract ulong MaxMemoryCore();
        protected abstract int ExitCodeCore();
        protected abstract double TotalTimeCore();
        protected abstract double RunningTimeCore();
        protected virtual bool HasExitedCore() => inside.HasExited;
        private bool keep_reading = true;

        /// <summary>
        /// 跟踪进程执行状态
        /// </summary>
        public bool PTrace { get; set; } = false;

        /// <summary>
        /// 沙盒进程工作目录
        /// </summary>
        public static string WorkingDirectory { get; set; } = Environment.CurrentDirectory;

        /// <summary>
        /// 退出状态码
        /// </summary>
        public int ExitCode => ExitCodeCore();

        /// <summary>
        /// 是否已经退出
        /// </summary>
        public bool HasExited => HasExitedCore();

        /// <summary>
        /// 标准输出流
        /// </summary>
        public StreamReader StandardOutput => stdout;

        /// <summary>
        /// 标准错误流
        /// </summary>
        public StreamReader StandardError => stderr;

        /// <summary>
        /// 标准输入流
        /// </summary>
        public StreamWriter StandardInput => stdin;
        
        /// <summary>
        /// 沙盒进程内存限制
        /// </summary>
        public ulong MemoryLimit => mem_l;

        /// <summary>
        /// 沙盒进程时间限制
        /// </summary>
        public int TimeLimit => time_l;

        /// <summary>
        /// 沙盒内进程数量限制
        /// </summary>
        public int ProcessLimit => proc_l;

        /// <summary>
        /// 进程启动信息
        /// </summary>
        public ProcessStartInfo StartInfo => info;

        /// <summary>
        /// 运行总时间
        /// </summary>
        public double TotalTime => TotalTimeCore();

        /// <summary>
        /// 启动后的时间
        /// </summary>
        public double RunningTime => RunningTimeCore();

        /// <summary>
        /// 已经使用的最大内存
        /// </summary>
        public ulong MaxMemory => MaxMemoryCore();

        /// <summary>
        /// 监控进程状态
        /// </summary>
        public abstract void Watch();

        /// <summary>
        /// 是否超过沙盒的限制
        /// </summary>
        public abstract bool OutOfLimit();

        internal SandboxProcess()
        {
            info = new ProcessStartInfo();
        }

        /// <summary>
        /// 创建沙盒
        /// </summary>
        /// <param name="file">文件名称</param>
        /// <param name="args">启动参数</param>
        /// <param name="stderr">是否重定向stderr</param>
        /// <param name="stdin">是否重定向stdin</param>
        /// <returns>新的沙盒进程实例</returns>
        public static SandboxProcess Create(string file, string args = "", bool stderr = false, bool stdin = false, bool cd = false)
        {
            if (!File.Exists(file)) 
            {
                Trace.WriteLine($"File doesn't exist: {file}");
                return null;
            }

            SandboxProcess ret;
            if (Environment.OSVersion.Platform == PlatformID.Unix)
                ret = new Linux();
            else
                ret = new WinNT();
            ret.info.RedirectStandardOutput = true;
            ret.info.StandardOutputEncoding = Console.Out.Encoding;
            ret.info.RedirectStandardError = stderr;
            if (stderr) ret.info.StandardErrorEncoding = Console.Error.Encoding;
            ret.info.RedirectStandardInput = stdin;
            ret.info.CreateNoWindow = true;
            ret.info.UseShellExecute = false;
            ret.info.FileName = file;
            ret.info.Arguments = args;
            ret.info.WorkingDirectory = 
                cd && Directory.Exists(WorkingDirectory) ? 
                    WorkingDirectory : Environment.CurrentDirectory;
            
            return ret;
        }

        /// <summary>
        /// 启动沙盒进程
        /// </summary>
        /// <param name="_err">如果异步流，则将stderr写入此</param>
        /// <param name="_out">如果异步流，则将stdout写入此</param>
        public abstract void Start(StringBuilder _out = null, StringBuilder _err = null);

        /// <summary>
        /// 引导异步流内容
        /// </summary>
        /// <param name="dest">目标</param>
        /// <param name="e">元数据</param>
        protected void StreamPipe(StringBuilder dest, DataReceivedEventArgs e)
        {
            StreamPipe(dest, e.Data);
        }

        /// <summary>
        /// 引导异步流内容
        /// </summary>
        /// <param name="dest">目标</param>
        /// <param name="val">元数据</param>
        protected void StreamPipe(StringBuilder dest, string val)
        {
            if (!keep_reading) return;
            dest.AppendLine(val);
            if (dest.Length > 4096)
            {
                dest.Append(", ......");
                keep_reading = false;
            }
        }

        /// <summary>
        /// 设置沙盒参数
        /// </summary>
        /// <param name="mem">内存限制</param>
        /// <param name="cpu">CPU时间限制</param>
        /// <param name="pl">进程数量限制</param>
        /// <returns>无意义</returns>
        public virtual bool Setup(ulong mem, int time, int proc, bool trace = false)
        {
            mem_l = mem;
            time_l = time;
            proc_l = proc;
            PTrace = trace;
            return true;
        }

        /// <summary>
        /// 等待退出
        /// </summary>
        /// <param name="len">时间长度</param>
        /// <returns>是否退出</returns>
        public abstract bool WaitForExit(int len);
        
        /// <summary>
        /// 等待退出
        /// </summary>
        public abstract void WaitForExit();
        
        /// <summary>
        /// 结束沙盒进程
        /// </summary>
        public abstract void Kill(int exitcode = 0);

        /// <summary>
        /// 是否为运行时错误
        /// </summary>
        public abstract bool IsRuntimeError { get; }

        /// <summary>
        /// 是否为内存超限
        /// </summary>
        public virtual bool IsMemoryLimitExceeded => MaxMemory > mem_l << 20;

        /// <summary>
        /// 是否为内存超限
        /// </summary>
        public virtual bool IsTimeLimitExceeded => TotalTime > time_l;
    }
}
