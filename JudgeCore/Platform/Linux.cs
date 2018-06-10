using System;
using System.Diagnostics;
using System.IO;

namespace JudgeCore.Platform
{
    public class Linux
    {
        public Process CreateJudgeProcess(IntPtr job, ProcessStartInfo info, out StreamReader stdout, out StreamWriter stdin)
        {
            throw new NotImplementedException();
        }

        public long PeakProcessMemoryInfo(Process proc)
        {
            throw new NotImplementedException();
        }

        public IntPtr SetupSandbox(long mem, int cpu, int pl)
        {
            throw new NotImplementedException();
        }

        public Action StartCompilerProcess(Process proc)
        {
            throw new NotImplementedException();
        }

        public void UnsetSandbox(ref IntPtr hJob)
        {
            throw new NotImplementedException();
        }
    }
}
