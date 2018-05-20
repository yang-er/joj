using Microsoft.Win32.SafeHandles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace JudgeCore.Platform
{
    public class Win32
    {
        #region JudgeW32.dll P/Invoke

        [DllImport("JudgeW32.dll", SetLastError = true)]
        public static extern UIntPtr PeakProcessMemoryInfo(IntPtr hProcess);

        [DllImport("JudgeW32.dll", SetLastError = true)]
        public static extern IntPtr SetupSandbox(uint dwMemoryLimit, uint dwCPUTime, uint dwProcessLimit);

        [DllImport("JudgeW32.dll", SetLastError = true)]
        public static extern void UnsetSandbox(IntPtr hJob);

        [DllImport("JudgeW32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int CreateJudgeProcess(ref JudgeInfo pJudgeInfo);

        [StructLayout(LayoutKind.Sequential)]
        public struct JudgeInfo
        {
            public IntPtr hJob;
            public IntPtr hStdIn;
            public IntPtr hStdOut;
            public IntPtr hStdErr;
            public IntPtr pEnv;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pwzExe;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pwzCmd;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pwzDir;
        }

        #endregion

        #region Stream Redirect from dotNetSrc

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CreatePipe(out SafeFileHandle hReadPipe, out SafeFileHandle hWritePipe, ref SecurityAttributes lpPipeAttributes, uint nSize);

        [StructLayout(LayoutKind.Sequential)]
        public struct SecurityAttributes
        {
            public int nLength;
            public IntPtr lpSecurityDescriptor;
            public bool bInheritHandle;

            public SecurityAttributes(bool bI = false)
            {
                nLength = 12;
                lpSecurityDescriptor = IntPtr.Zero;
                bInheritHandle = bI;
            }
        }
        
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DuplicateHandle(HandleRef hSourceProcessHandle, SafeFileHandle hSourceHandle, HandleRef hTargetProcessHandle, out SafeFileHandle lpTargetHandle, uint dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, uint dwOptions);

        public static void CreatePipeWithSecurityAttributes(out SafeFileHandle hReadPipe, out SafeFileHandle hWritePipe, SecurityAttributes lpPipeAttributes, uint nSize)
        {
            bool ret = CreatePipe(out hReadPipe, out hWritePipe, ref lpPipeAttributes, nSize);
            if (!ret || hReadPipe.IsInvalid || hWritePipe.IsInvalid)
            {
                throw new Win32Exception();
            }
        }

        public static void CreatePipe(out SafeFileHandle parentHandle, out SafeFileHandle childHandle, bool parentInputs)
        {
            var securityAttributesParent = new SecurityAttributes(true);
            SafeFileHandle hTmp = null;

            try
            {
                if (parentInputs)
                {
                    CreatePipeWithSecurityAttributes(out childHandle, out hTmp, securityAttributesParent, 128 * 1024);
                }
                else
                {
                    CreatePipeWithSecurityAttributes(out hTmp, out childHandle, securityAttributesParent, 128 * 1024);
                }

                var ptr = Process.GetCurrentProcess().Handle;
                if (!DuplicateHandle(new HandleRef(Process.GetCurrentProcess(), ptr), hTmp, new HandleRef(Process.GetCurrentProcess(), ptr), out parentHandle, 0, false, 2))
                {
                    throw new Win32Exception();
                }
            }
            finally
            {
                if (hTmp != null && !hTmp.IsInvalid)
                {
                    hTmp.Close();
                }
            }
        }

        #endregion

        #region Other WINAPI Series

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AssignProcessToJobObject(IntPtr hJob, IntPtr hProcess);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool IsProcessInJob(IntPtr hProcess, IntPtr hJob, out bool hResult);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);

        [DllImport("wer.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int WerAddExcludedApplication(string pwzExeName, bool bAllUsers);
        
        #endregion
        
        static object cjp_lock = new object();

        public static Process CreateJudgeProcess(IntPtr job, ProcessStartInfo info, out StreamReader stdout, out StreamWriter stdin)
        {
#if !MANAGED_JOB
            var ji = new JudgeInfo
            {
                hJob = job,
                pwzExe = info.FileName,
                pwzCmd = info.Arguments,
                pwzDir = info.WorkingDirectory
            };
            
            int pid;

            lock (cjp_lock)
            {
                byte[] environmentBytes = EnvironmentBlock.ToByteArray(info.EnvironmentVariables, true);
                var environmentHandle = GCHandle.Alloc(environmentBytes, GCHandleType.Pinned);
                ji.pEnv = environmentHandle.AddrOfPinnedObject();

                pid = CreateJudgeProcess(ref ji);

                if (environmentHandle.IsAllocated)
                    environmentHandle.Free();
            }

            stdin = new StreamWriter(new FileStream(new SafeFileHandle(ji.hStdIn, true), FileAccess.Write, 4096, false), Console.InputEncoding, 4096) { AutoFlush = true, NewLine = "\n" };
            stdout = new StreamReader(new FileStream(new SafeFileHandle(ji.hStdOut, true), FileAccess.Read, 4096, false), info.StandardOutputEncoding ?? Console.OutputEncoding, true, 16384);

            var ret = Process.GetProcessById(pid);
            var hdl = ret.Handle;
            return ret;
#else
            WerAddExcludedApplication(info.FileName, false);
            var ret = Process.Start(info);
            if (job != IntPtr.Zero && !AssignProcessToJobObject(job, ret.Handle))
                throw new Win32Exception();
            stdout = ret.StandardOutput;
            stdin = ret.StandardInput;
            return ret;
#endif
        }
        
        internal static class EnvironmentBlock
        {
            public static byte[] ToByteArray(StringDictionary sd, bool unicode)
            {
                string[] keys = new string[sd.Count];
                byte[] envBlock = null;
                sd.Keys.CopyTo(keys, 0);
                
                string[] values = new string[sd.Count];
                sd.Values.CopyTo(values, 0);
                
                Array.Sort(keys, values, OrdinalCaseInsensitiveComparer.Default);
                
                StringBuilder stringBuff = new StringBuilder();
                for (int i = 0; i < sd.Count; ++i)
                {
                    stringBuff.Append(keys[i]);
                    stringBuff.Append('=');
                    stringBuff.Append(values[i]);
                    stringBuff.Append('\0');
                }
                
                stringBuff.Append('\0');
                envBlock = Encoding.Unicode.GetBytes(stringBuff.ToString());
                return envBlock;
            }

            internal class OrdinalCaseInsensitiveComparer : IComparer
            {
                internal static readonly OrdinalCaseInsensitiveComparer Default = new OrdinalCaseInsensitiveComparer();

                public int Compare(Object a, Object b)
                {
                    if (a is String sa && b is string sb)
                    {
                        return String.Compare(sa, sb, StringComparison.OrdinalIgnoreCase);
                    }
                    return Comparer.Default.Compare(a, b);
                }
            }
        }
    }
}
