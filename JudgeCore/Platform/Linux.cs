using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace JudgeCore.Platform
{
    public class Linux : SandboxProcess
    {
        private int pid_t, memp, timep, ec;

        [DllImport("JudgeL64.out", EntryPoint = "watch_sandbox")]
        public static extern void WatchSandbox(uint mem, uint time, int app, ref int max_mem, ref int max_time, ref int exitcode, bool pf);

        [DllImport("JudgeL64.out", EntryPoint = "unset_sandbox")]
        public static extern void UnsetSandbox(int app);
        
        public override void Kill()
        {
            UnsetSandbox(pid_t);
        }

        public override bool OutOfLimit()
        {
            throw new PlatformNotSupportedException();
        }
        
        public override void Start(StringBuilder _out = null, StringBuilder _err = null)
        {
            throw new NotImplementedException();
            pid_t = inside.Id;
        }

        public override bool WaitForExit(int len = -1) => inside.WaitForExit(len);
        protected override int ExitCodeCore() => ec;
        protected override ulong MaxMemoryCore() => (ulong)memp;
        public override void Watch() => WatchSandbox((uint)mem_l, (uint)time_l, pid_t, ref memp, ref timep, ref ec, false);

        public Linux()
        {
            if (Environment.OSVersion.Platform != PlatformID.Unix)
                throw new Win32Exception("平台读取错误");
        }
    }
}
