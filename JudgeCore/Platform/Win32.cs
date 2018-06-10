using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace JudgeCore.Platform
{
    public class WinNT
    {
        [DllImport("JudgeW32.dll", SetLastError = true)]
        public static extern UIntPtr PeakProcessMemoryInfo(IntPtr hProcess);

        [DllImport("JudgeW32.dll", SetLastError = true)]
        public static extern IntPtr SetupSandbox(uint dwMemoryLimit, uint dwCPUTime, uint dwProcessLimit);

        [DllImport("JudgeW32.dll", SetLastError = true)]
        public static extern void UnsetSandbox(IntPtr hJob);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AssignProcessToJobObject(IntPtr hJob, IntPtr hProcess);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool IsProcessInJob(IntPtr hProcess, IntPtr hJob, out bool hResult);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);

        [DllImport("wer.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int WerAddExcludedApplication(string pwzExeName, bool bAllUsers);

        [DllImport("wer.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int WerRemoveExcludedApplication(string pwzExeName, bool bAllUsers);
        
        public WinNT()
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
                throw new Win32Exception("平台读取错误");
        }
    }
}
