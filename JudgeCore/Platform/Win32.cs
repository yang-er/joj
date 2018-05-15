using Microsoft.Win32.SafeHandles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace JudgeCore.Platform
{
    public class Win32
    {
        [DllImport("JudgeW32.dll", SetLastError = true)]
        public static extern UIntPtr PeakProcessMemoryInfo(IntPtr hProcess);

        [DllImport("JudgeW32.dll", SetLastError = true)]
        public static extern IntPtr SetupSandbox(uint dwMemoryLimit, uint dwCPUTime);

        [DllImport("JudgeW32.dll", SetLastError = true)]
        public static extern void UnsetSandbox(IntPtr hJob);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AssignProcessToJobObject(IntPtr hJob, IntPtr hProcess);

        [DllImport("JudgeW32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern int CreateJudgeProcess(JudgeInfo pJudgeInfo);

        [DllImport("kernel32.dll")]
        static extern bool CreatePipe(out SafeFileHandle hReadPipe, out SafeFileHandle hWritePipe, ref SecurityAttributes lpPipeAttributes, uint nSize);

        [StructLayout(LayoutKind.Sequential)]
        struct SecurityAttributes
        {
            public int nLength;
            public IntPtr lpSecurityDescriptor;
            [MarshalAs(UnmanagedType.Bool)]
            public bool bInheritHandle;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct JudgeInfo
        {
            public IntPtr hJob;
            public SafeFileHandle hStdIn;
            public SafeFileHandle hStdOut;
            public SafeFileHandle hStdErr;
            public IntPtr pEnv;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pwzExe;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pwzCmd;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pwzDir;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool DuplicateHandle(IntPtr hSourceProcessHandle, SafeFileHandle hSourceHandle, IntPtr hTargetProcessHandle, out SafeFileHandle lpTargetHandle, uint dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, uint dwOptions);
        
        [DllImport("wer.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern int WerAddExcludedApplication(string pwzExeName, bool bAllUsers);
        
        static void CreatePipeWithSecurityAttributes(out SafeFileHandle hReadPipe, out SafeFileHandle hWritePipe, SecurityAttributes lpPipeAttributes, uint nSize)
        {
            bool ret = CreatePipe(out hReadPipe, out hWritePipe, ref lpPipeAttributes, nSize);
            if (!ret || hReadPipe.IsInvalid || hWritePipe.IsInvalid)
            {
                throw new Win32Exception();
            }
        }

        static void CreatePipe(out SafeFileHandle parentHandle, out SafeFileHandle childHandle, bool parentInputs)
        {
            var securityAttributesParent = new SecurityAttributes
            {
                bInheritHandle = true
            };

            SafeFileHandle hTmp = null;
            try
            {
                if (parentInputs)
                {
                    CreatePipeWithSecurityAttributes(out childHandle, out hTmp, securityAttributesParent, 0);
                }
                else
                {
                    CreatePipeWithSecurityAttributes(out hTmp, out childHandle, securityAttributesParent, 0);
                }

                var ptr = Process.GetCurrentProcess().Handle;
                if (!DuplicateHandle(ptr, hTmp, ptr, out parentHandle, 0, false, 2))
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

            SafeFileHandle standardInputWritePipeHandle, standardOutputReadPipeHandle;
            int pid;

            lock (cjp_lock)
            {
                byte[] environmentBytes = EnvironmentBlock.ToByteArray(info.EnvironmentVariables, true);
                var environmentHandle = GCHandle.Alloc(environmentBytes, GCHandleType.Pinned);
                ji.pEnv = environmentHandle.AddrOfPinnedObject();
                ji.hStdErr = new SafeFileHandle(GetStdHandle(-12), false);
                CreatePipe(out standardInputWritePipeHandle, out ji.hStdIn, true);
                CreatePipe(out standardOutputReadPipeHandle, out ji.hStdOut, false);

                pid = CreateJudgeProcess(ji);

                if (environmentHandle.IsAllocated)
                    environmentHandle.Free();
            }

            stdin = new StreamWriter(new FileStream(standardInputWritePipeHandle, FileAccess.Write, 4096, false), Console.InputEncoding, 4096) { AutoFlush = true, NewLine = "\n" };
            stdout = new StreamReader(new FileStream(standardOutputReadPipeHandle, FileAccess.Read, 4096, false), info.StandardOutputEncoding ?? Console.OutputEncoding, true, 4096);

            var ret = Process.GetProcessById(pid);
            var hdl = ret.Handle;
            return ret;
#else
            WerAddExcludedApplication(info.FileName, false);
            var ret = Process.Start(info);
            if (!AssignProcessToJobObject(job, ret.Handle))
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

                if (unicode)
                {
                    envBlock = Encoding.Unicode.GetBytes(stringBuff.ToString());
                }
                else
                {
                    envBlock = Encoding.Default.GetBytes(stringBuff.ToString());

                    if (envBlock.Length > UInt16.MaxValue)
                        throw new InvalidOperationException("Environment Block too long");
                }

                return envBlock;
            }
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
