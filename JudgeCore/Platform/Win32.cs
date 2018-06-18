using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace JudgeCore.Platform
{
    public class WinNT : SandboxProcess
    {
        protected Process inside;
        private IntPtr job_obj;
        
        protected override int ExitCodeCore() => inside.ExitCode;
        protected override ulong MaxMemoryCore() => PeakProcessMemoryInfo(inside.Handle).ToUInt64();
        
        public override bool OutOfLimit()
        {
            return TotalTime >= TimeLimit || RunningTime >= TimeLimit * 10 || (long)MaxMemory > MemoryLimit << 20;
        }
        
        public override void Start(StringBuilder _out = null, StringBuilder _err = null)
        {
            inside = Process.Start(info);
            AssignProcessToJobObject(job_obj, inside.Handle);

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
        
        public override bool Setup(long mem, int time, int proc, bool trace)
        {
            base.Setup(mem, time, proc, trace);
            job_obj = SetupSandbox((uint)mem, (uint)time, (uint)proc);
            return true;
        }

        public override bool WaitForExit(int len) => inside.WaitForExit(len);
        public override void WaitForExit() => inside.WaitForExit();
        protected override double TotalTimeCore() => inside.UserProcessorTime.TotalMilliseconds;
        protected override double RunningTimeCore() => (DateTime.Now - inside.StartTime).TotalMilliseconds;
        protected override bool HasExitedCore() => inside.HasExited;

        public override void Kill()
        {
            if (job_obj == IntPtr.Zero) return;
            UnsetSandbox(job_obj);
            job_obj = IntPtr.Zero;
        }

        public override void Watch()
        {
            while (!HasExited && !OutOfLimit()) ;
            if (!HasExited) Kill();
        }

        public WinNT()
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
                throw new Win32Exception("平台读取错误");
        }

        #region P/Invoke

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

        #endregion
    }
}
