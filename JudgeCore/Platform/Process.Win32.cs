﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace JudgeCore.Platform
{
    public sealed class WinNT : SandboxProcess
    {
        private IntPtr job_obj;
        
        public override void Kill(int exitcode = 0)
        {
            if (job_obj == IntPtr.Zero) return;
            UnsetSandbox(job_obj);
            job_obj = IntPtr.Zero;
        }

        public override bool OutOfLimit()
        {
            return TotalTime >= TimeLimit || RunningTime >= TimeLimit * 10 || MaxMemory > MemoryLimit << 20;
        }
        
        public override void Start(StringBuilder _out = null, StringBuilder _err = null)
        {
            if (_err is null)
            {
                pt_stderr = new StringBuilder();
                _err = pt_stderr;
                info.RedirectStandardError = true;
            }

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
        
        public override bool Setup(ulong mem, int time, int proc, bool trace)
        {
            base.Setup(mem, time, proc, trace);
            job_obj = SetupSandbox((uint)mem, (uint)time, (uint)proc);
            return true;
        }

        protected override double TotalTimeCore() => inside.UserProcessorTime.TotalMilliseconds;
        protected override double RunningTimeCore() => (DateTime.Now - inside.StartTime).TotalMilliseconds;
        protected override ulong MaxMemoryCore() => PeakProcessMemoryInfo(inside.Handle).ToUInt64();

        public override void WaitForExit() => inside.WaitForExit();
        public override bool WaitForExit(int len) => inside.WaitForExit(len);

        public override void RefreshState(ICompiler compiler)
        {
            if (read_result) return;
            ec = inside.ExitCode;
            read_result = true;
            base.RefreshState(compiler);
        }

        public override void Watch()
        {
            while (!HasExited && !OutOfLimit()) ;
            if (!HasExited) Kill();
        }

        public override bool IsRuntimeError => Enum.IsDefined(typeof(ErrorCode), ec);

        public WinNT()
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
                throw new Win32Exception("平台读取错误");
        }

        public enum ErrorCode : int
        {
            IntegerDividedByZero = -1073741676,
            SegmentFault = -1073740940,
            AccessViolation = -1073741819,
            StackOverflow = -1073741571,
            DllNotFound = -1073741515,
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
