using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace JudgeCore.Platform
{
    public class Win32 : IPlatform
    {
        #region Unmanaged code stack

        [DllImport("JudgeW32.dll", SetLastError = true)]
        static extern UIntPtr PeakProcessMemoryInfo(IntPtr hProcess);

        [DllImport("JudgeW32.dll", SetLastError = true)]
        static extern IntPtr SetupSandbox(uint dwMemoryLimit, uint dwCPUTime, uint dwProcessLimit);

        [DllImport("JudgeW32.dll", SetLastError = true)]
        static extern void UnsetSandbox(IntPtr hJob);
        
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AssignProcessToJobObject(IntPtr hJob, IntPtr hProcess);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool IsProcessInJob(IntPtr hProcess, IntPtr hJob, out bool hResult);

        [DllImport("kernel32.dll")]
        static extern bool CloseHandle(IntPtr hObject);

        [DllImport("wer.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern int WerAddExcludedApplication(string pwzExeName, bool bAllUsers);

        [DllImport("wer.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern int WerRemoveExcludedApplication(string pwzExeName, bool bAllUsers);

        #endregion
        
        public IntPtr SetupSandbox(long mem, int cpu, int pl)
        {
            return SetupSandbox((uint)mem, (uint)cpu, (uint)pl);
        }

        public Action StartCompilerProcess(Process proc)
        {
            IntPtr hJob = SetupSandbox(128L, 1000, 10);
            proc.Start();
            AssignProcessToJobObject(hJob, proc.Handle);
            return () => UnsetSandbox(hJob);
        }

        public Process CreateJudgeProcess(IntPtr job, ProcessStartInfo info, out StreamReader stdout, out StreamWriter stdin)
        {
            var ret = Process.Start(info);
            if (job != IntPtr.Zero && !AssignProcessToJobObject(job, ret.Handle))
                throw new Win32Exception();
            stdout = ret.StandardOutput;
            stdin = ret.StandardInput;
            return ret;
        }
        
        public long PeakProcessMemoryInfo(Process proc)
        {
            return (long)PeakProcessMemoryInfo(proc.Handle).ToUInt64();
        }

        public void UnsetSandbox(ref IntPtr hJob)
        {
            if (hJob == IntPtr.Zero) return;
            UnsetSandbox(hJob);
            hJob = IntPtr.Zero;
        }

        public Win32()
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
                throw new Win32Exception("平台读取错误");
        }
    }
}
