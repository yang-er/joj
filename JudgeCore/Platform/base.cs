using JudgeCore.Platform;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using static JudgeCore.Helper;

namespace JudgeCore
{
    /// <summary>
    /// 沙盒进程
    /// </summary>
    public class SandboxProcess
    {
        private Process inside;
        private ProcessStartInfo info;
        private long mem_l;
        private int time_l, proc_l;
        private IntPtr job_obj;
        private StreamReader stdout, stderr;
        private StreamWriter stdin;

        /// <summary>
        /// 退出状态码
        /// </summary>
        public int ExitCode => inside.ExitCode;

        /// <summary>
        /// 是否已经退出
        /// </summary>
        public bool HasExited => inside.HasExited;

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
        public long MemoryLimit => mem_l;

        /// <summary>
        /// 沙盒进程时间限制
        /// </summary>
        public long TimeLimit => time_l;

        /// <summary>
        /// 沙盒内进程数量限制
        /// </summary>
        public long ProcessLimit => proc_l;

        /// <summary>
        /// 进程启动信息
        /// </summary>
        public ProcessStartInfo StartInfo => info;

        /// <summary>
        /// 运行总时间
        /// </summary>
        public double TotalTime => inside.UserProcessorTime.TotalMilliseconds;

        /// <summary>
        /// 启动后的时间
        /// </summary>
        public double RunningTime => (DateTime.Now - inside.StartTime).TotalMilliseconds;

        /// <summary>
        /// 已经使用的最大内存
        /// </summary>
        public ulong MaxMemory
        {
            get
            {
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                    return WinNT.PeakProcessMemoryInfo(inside.Handle).ToUInt64();
                else
                    return (ulong)inside.PeakWorkingSet64;
            }
        }

        /// <summary>
        /// 是否超过沙盒的限制
        /// </summary>
        public bool OutOfLimit => TotalTime >= TimeLimit || RunningTime >= TimeLimit * 10 || (long)MaxMemory > MemoryLimit << 20;

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
            if (!File.Exists(file)) return null;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var ret = new SandboxProcess();
            ret.info.RedirectStandardOutput = true;
            ret.info.StandardOutputEncoding = Encoding.GetEncoding(936);
            ret.info.RedirectStandardError = stderr;
            if (stderr) ret.info.StandardErrorEncoding = Encoding.GetEncoding(936);
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
        public void Start(StringBuilder _out = null, StringBuilder _err = null)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                inside = Process.Start(info);
                WinNT.AssignProcessToJobObject(job_obj, inside.Handle);

                if (info.RedirectStandardInput)
                    stdin = inside.StandardInput;

                if (info.RedirectStandardError)
                {
                    if (_err is null)
                    {
                        stderr = inside.StandardError;
                    }
                    else
                    {
                        inside.ErrorDataReceived += (s, e) => StreamPipe(_err, e);
                        inside.BeginErrorReadLine();
                    }
                }

                if (info.RedirectStandardOutput)
                {
                    if (_out is null)
                    {
                        stdout = inside.StandardOutput;
                    }
                    else
                    {
                        inside.OutputDataReceived += (s, e) => StreamPipe(_out, e);
                        inside.BeginOutputReadLine();
                    }
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 引导异步流内容
        /// </summary>
        /// <param name="dest">目标</param>
        /// <param name="e">元数据</param>
        private void StreamPipe(StringBuilder dest, DataReceivedEventArgs e)
        {
            dest.AppendLine(e.Data);
            if (dest.Length > 4096)
            {
                dest.Append(", ......");
                Kill();
            }
        }
        
        /// <summary>
        /// 设置沙盒参数
        /// </summary>
        /// <param name="mem">内存限制</param>
        /// <param name="cpu">CPU时间限制</param>
        /// <param name="pl">进程数量限制</param>
        /// <returns>无意义</returns>
        public bool Setup(long mem, int time, int proc)
        {
            mem_l = mem;
            time_l = time;
            proc_l = proc;
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                job_obj = WinNT.SetupSandbox((uint)mem, (uint)time, (uint)proc);
            return true;
        }

        /// <summary>
        /// 等待退出
        /// </summary>
        /// <param name="len">时间长度</param>
        /// <returns>是否退出</returns>
        public bool WaitForExit(int len)
        {
            return inside.WaitForExit(len);
        }

        /// <summary>
        /// 等待退出
        /// </summary>
        public void WaitForExit()
        {
            inside.WaitForExit();
        }

        /// <summary>
        /// 结束沙盒进程
        /// </summary>
        public void Kill()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                if (job_obj == IntPtr.Zero) return;
                WinNT.UnsetSandbox(job_obj);
                job_obj = IntPtr.Zero;
            }
            else
            {
                inside.Kill();
            }
        }
    }
}
