// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// Modified from dotnet/corefx/System.Diagnostics.Process.Unix

using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace JudgeCore.Platform
{
    public sealed partial class Linux
    {
        internal AsyncStreamReader _output;
        internal AsyncStreamReader _error;

        private bool Start(ProcessStartInfo si)
        {
            int stdinFd, stdoutFd, stderrFd;
            string cwd = !string.IsNullOrWhiteSpace(si.WorkingDirectory) ? si.WorkingDirectory : null;

            s_processStartLock.EnterReadLock();
            try
            {
                pid_t = SetupSandbox((uint)mem_l, (uint)time_l, (uint)proc_l, PTrace,
                    !string.IsNullOrWhiteSpace(si.WorkingDirectory) ? si.WorkingDirectory : Environment.CurrentDirectory,
                    PTrace, si.FileName, ParseArgv(si), CreateEnvp(si),
                    si.RedirectStandardInput, si.RedirectStandardOutput, si.RedirectStandardError,
                    out stdinFd, out stdoutFd, out stderrFd);

                if (pid_t <= 0) return false;
            }
            finally
            {
                s_processStartLock.ExitReadLock();
            }

            if (si.RedirectStandardInput)
            {
                Debug.Assert(stdinFd >= 0);
                stdin = new StreamWriter(OpenStream(stdinFd, FileAccess.Write),
                   Console.InputEncoding, StreamBufferSize)
                { AutoFlush = true };
            }

            if (si.RedirectStandardOutput)
            {
                Debug.Assert(stdoutFd >= 0);
                stdout = new StreamReader(OpenStream(stdoutFd, FileAccess.Read),
                    si.StandardOutputEncoding, true, StreamBufferSize);
            }

            if (si.RedirectStandardError)
            {
                Debug.Assert(stderrFd >= 0);
                stderr = new StreamReader(OpenStream(stderrFd, FileAccess.Read),
                    si.StandardErrorEncoding, true, StreamBufferSize);
            }

            return true;
        }

        private static unsafe void AllocNullTerminatedArray(string[] arr, ref byte** arrPtr)
        {
            int arrLength = arr.Length + 1; // +1 is for null termination

            // Allocate the unmanaged array to hold each string pointer.
            // It needs to have an extra element to null terminate the array.
            arrPtr = (byte**)Marshal.AllocHGlobal(sizeof(IntPtr) * arrLength);
            Debug.Assert(arrPtr != null);

            // Zero the memory so that if any of the individual string allocations fails,
            // we can loop through the array to free any that succeeded.
            // The last element will remain null.
            for (int i = 0; i < arrLength; i++)
            {
                arrPtr[i] = null;
            }

            // Now copy each string to unmanaged memory referenced from the array.
            // We need the data to be an unmanaged, null-terminated array of UTF8-encoded bytes.
            for (int i = 0; i < arr.Length; i++)
            {
                byte[] byteArr = Encoding.UTF8.GetBytes(arr[i]);

                arrPtr[i] = (byte*)Marshal.AllocHGlobal(byteArr.Length + 1); //+1 for null termination
                Debug.Assert(arrPtr[i] != null);

                Marshal.Copy(byteArr, 0, (IntPtr)arrPtr[i], byteArr.Length); // copy over the data from the managed byte array
                arrPtr[i][byteArr.Length] = (byte)'\0'; // null terminate
            }
        }

        private static unsafe void FreeArray(byte** arr, int length)
        {
            if (arr != null)
            {
                // Free each element of the array
                for (int i = 0; i < length; i++)
                {
                    if (arr[i] != null)
                    {
                        Marshal.FreeHGlobal((IntPtr)arr[i]);
                        arr[i] = null;
                    }
                }

                // And then the array itself
                Marshal.FreeHGlobal((IntPtr)arr);
            }
        }

        private static string[] ParseArgv(ProcessStartInfo psi, string alternativePath = null)
        {
            string argv0 = psi.FileName;
            if (string.IsNullOrEmpty(psi.Arguments) && string.IsNullOrEmpty(alternativePath))
            {
                return new string[] { argv0 };
            }

            var argvList = new List<string>();
            if (!string.IsNullOrEmpty(alternativePath))
            {
                argvList.Add(alternativePath);
            }

            argvList.Add(argv0);
            ParseArgumentsIntoList(psi.Arguments, argvList);
            return argvList.ToArray();
        }

        private static FileStream OpenStream(int fd, FileAccess access)
        {
            Debug.Assert(fd >= 0);
            return new FileStream(
                new SafeFileHandle((IntPtr)fd, ownsHandle: true),
                access, StreamBufferSize, isAsync: false);
        }

        private static string[] CreateEnvp(ProcessStartInfo psi)
        {
            var envp = new string[psi.Environment.Count];
            int index = 0;
            foreach (var pair in psi.Environment)
            {
                envp[index++] = pair.Key + "=" + pair.Value;
            }
            return envp;
        }

        private static void ParseArgumentsIntoList(string arguments, List<string> results)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < arguments.Length; i++)
            {
                while (i < arguments.Length && (arguments[i] == ' ' || arguments[i] == '\t'))
                    i++;

                if (i == arguments.Length)
                    break;

                results.Add(GetNextArgument(sb, arguments, ref i));
            }
        }

        private static string GetNextArgument(StringBuilder currentArgument, string arguments, ref int i)
        {
            bool inQuotes = false;

            while (i < arguments.Length)
            {
                int backslashCount = 0;
                while (i < arguments.Length && arguments[i] == '\\')
                {
                    i++;
                    backslashCount++;
                }

                if (backslashCount > 0)
                {
                    if (i >= arguments.Length || arguments[i] != '"')
                    {
                        currentArgument.Append('\\', backslashCount);
                    }
                    else
                    {
                        currentArgument.Append('\\', backslashCount / 2);
                        if (backslashCount % 2 != 0)
                        {
                            currentArgument.Append('"');
                            i++;
                        }
                    }

                    continue;
                }

                char c = arguments[i];

                if (c == '"')
                {
                    if (inQuotes && i < arguments.Length - 1 && arguments[i + 1] == '"')
                    {
                        currentArgument.Append('"');
                        i++;
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }

                    i++;
                    continue;
                }

                if ((c == ' ' || c == '\t') && !inQuotes)
                {
                    break;
                }

                currentArgument.Append(c);
                i++;
            }

            var ret = currentArgument.ToString();
            currentArgument.Clear();
            return ret;
        }
    }
}
