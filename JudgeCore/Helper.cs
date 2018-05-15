using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace JudgeCore
{
    class Helper
    {
        /// <summary>
        /// 创建内部进程，不受限制的那种
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="arguments">参数</param>
        /// <returns>等待启动的进程</returns>
        public static Process MakeProcess(string filename, string arguments = "")
        {
            Debug.WriteLine(filename + " " + arguments);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var ret = new Process();
            ret.StartInfo.RedirectStandardOutput = true;
            ret.StartInfo.StandardOutputEncoding = Encoding.GetEncoding(936);
            ret.StartInfo.RedirectStandardError = true;
            ret.StartInfo.StandardErrorEncoding = Encoding.GetEncoding(936);
            ret.StartInfo.CreateNoWindow = true;
            ret.StartInfo.FileName = filename;
            ret.StartInfo.Arguments = arguments;
            return ret;
        }

        /// <summary>
        /// 创建评价进程，受限制的那种
        /// </summary>
        /// <param name="guid">程序标识符</param>
        /// <returns>等待启动的进程</returns>
        public static Process MakeJudgeProcess(Guid guid)
        {
            var filename = guid.ToString("D") + ".exe";
            if (!File.Exists(filename)) return null;
            var ret = new Process();
            ret.StartInfo.RedirectStandardOutput = true;
            ret.StartInfo.StandardOutputEncoding = Encoding.GetEncoding(936);
            ret.StartInfo.RedirectStandardInput = true;
            ret.StartInfo.UseShellExecute = false;
            ret.StartInfo.CreateNoWindow = true;
            ret.StartInfo.FileName = filename;
            ret.StartInfo.Environment["Path"] = "\\;";
            // ret.StartInfo.WorkingDirectory = "F:\\joj";
            return ret;
        }
        
        [StructLayout(LayoutKind.Sequential, Size = 72)]
        private struct PROCESS_MEMORY_COUNTERS
        {
            public uint cb;
            public uint PageFaultCount;
            public ulong PeakWorkingSetSize;
            public ulong WorkingSetSize;
            public ulong QuotaPeakPagedPoolUsage;
            public ulong QuotaPagedPoolUsage;
            public ulong QuotaPeakNonPagedPoolUsage;
            public ulong QuotaNonPagedPoolUsage;
            public ulong PagefileUsage;
            public ulong PeakPagefileUsage;
        }

        [DllImport("psapi.dll", SetLastError = true)]
        static extern bool GetProcessMemoryInfo(IntPtr hProcess, out PROCESS_MEMORY_COUNTERS counters, uint size);

        /// <summary>
        /// 获取进程内存占用情况
        /// </summary>
        /// <param name="proc">进程</param>
        /// <returns>最大占用内存</returns>
        public static long GetProcessMemoryInfo(Process proc)
        {
            PROCESS_MEMORY_COUNTERS memoryCounters;
            memoryCounters.cb = (uint)Marshal.SizeOf(typeof(PROCESS_MEMORY_COUNTERS));

            if (GetProcessMemoryInfo(proc.Handle, out memoryCounters, memoryCounters.cb))
            {
                return (long)memoryCounters.PeakWorkingSetSize;
            }
            else
            {
                return 0;
            }
        }

        [DllImport("wer.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int WerAddExcludedApplication(string pwzExeName, bool bAllUsers);

        [DllImport("wer.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int WerRemoveExcludedApplication(string pwzExeName, bool bAllUsers);
    }
}
