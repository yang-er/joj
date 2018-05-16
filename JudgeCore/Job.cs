using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace JudgeCore
{
    /// <summary>
    /// 每一个提交对应着一个任务
    /// </summary>
    public class Job
    {
        /// <summary>
        /// 编译器
        /// </summary>
        public ICompiler Compiler { get; }

        /// <summary>
        /// 提交的唯一标识符
        /// </summary>
        public Guid RunID { get; }
        
        /// <summary>
        /// 子评测内容控制
        /// </summary>
        public List<IJudger> Judger { get; }

        /// <summary>
        /// 目前状态
        /// </summary>
        public List<TestInfo> State { get; }

        /// <summary>
        /// 最大内存限制
        /// </summary>
        public long MemoryLimit { get; set; } = 128;

        /// <summary>
        /// 编译
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool Build(string code)
        {
            bool ret = Compiler.Compile(code, RunID);
            CompileInfo = Compiler.StandardOutput.Trim();
            return ret;
        }

        /// <summary>
        /// 编译信息
        /// </summary>
        public string CompileInfo;

        public IntPtr ActiveJob;

        private void CheckRuntime(Process pro, ref TestInfo ti, ref bool tle)
        {
            while (!pro.HasExited
                && pro.TotalProcessorTime.TotalMilliseconds < 1000
                && (DateTime.Now - pro.StartTime).TotalMilliseconds < 2000
                && (long)Platform.Win32.PeakProcessMemoryInfo(pro.Handle).ToUInt64() < MemoryLimit * 1048576L) ;
            if (!pro.HasExited) pro.Kill();
        }

        /// <summary>
        /// 对某一次进行评价
        /// </summary>
        /// <param name="id"></param>
        public void Judge(int id)
        {
            var ti = new TestInfo();
            try
            {
                ti.Result = JudgeResult.Pending;

                var pi = Helper.MakeJudgeInfo(RunID);
                if (pi is null)
                {
                    ti.Result = JudgeResult.CompileError;
                    return;
                }

                ti.Result = JudgeResult.Running;

                // Judge process
                if (Compiler.GetType().Name == "MinGW")
                    pi.Environment["PATH"] += $"C:\\MinGW\\bin;";
                var pro = Platform.Win32.CreateJudgeProcess(ActiveJob, pi,
                    out var stdout, out var stdin);
                bool tle = false;
                Task.Run(() => CheckRuntime(pro, ref ti, ref tle));
                Task.Run(() => Judger[id].Input(stdin));
                ti.Result = Judger[id].Judge(stdout);
                pro.WaitForExit();

                if (pro.ExitCode == -1) ti.Result = JudgeResult.Pending;

                // Judge extra info
                ti.Time = pro.TotalProcessorTime.TotalMilliseconds;
                Debug.WriteLine("Runtime: {0}ms", ti.Time);
                if (ti.Time >= 1000)
                    ti.Result = JudgeResult.TimeLimitExceeded;
                ti.Memory = Platform.Win32.PeakProcessMemoryInfo(pro.Handle).ToUInt32();
                Debug.WriteLine("Memory: {0}kb", ti.Memory / 1024);
                if (ti.Memory > MemoryLimit << 20)
                    ti.Result = JudgeResult.MemoryLimitExceeded;
                ti.ExitCode = pro.ExitCode;
                Debug.WriteLine("ExitCode: 0x" + ti.ExitCode.ToString("x"));
                if (ti.ExitCode != 0 && ti.ExitCode != -1) ti.Result = JudgeResult.RuntimeError;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                ti.Result = JudgeResult.RuntimeError;
            }
            finally
            {
                State.Add(ti);
            }
        }

        /// <summary>
        /// 批量评价
        /// </summary>
        public void Judge(bool show_log = false)
        {
            ActiveJob = Platform.Win32.SetupSandbox(128, 1000);
            var pi = Helper.MakeJudgeInfo(RunID);
            if (pi != null)
            {
                if (Compiler.GetType().Name == "MinGW")
                    pi.Environment["PATH"] += $"C:\\MinGW\\bin;";
                var pro = Platform.Win32.CreateJudgeProcess(ActiveJob, pi, 
                    out var stdout, out var stdin);
                stdin.Close();
                stdout.Close();
                Task.Run(async () => { await Task.Delay(1000); if (!pro.HasExited) pro.Kill(); });
                pro.WaitForExit();
            }

            for (int i = 0; i < Judger.Count; i++)
            {
                Judge(i);
                if (show_log) Console.WriteLine("{0}ms\t{1}mb\t{2}", 
                    (int)Math.Round(State[i].Time), 
                    (int)Math.Round(State[i].Memory / 1048576.0), 
                    State[i].Result.ToString());
            }
            
            Platform.Win32.UnsetSandbox(ActiveJob);
        }

        /// <summary>
        /// 初始化任务
        /// </summary>
        /// <param name="cl">编译器</param>
        /// <param name="o">评测器</param>
        public Job(ICompiler cl, List<IJudger> o)
        {
            Compiler = cl;
            RunID = Guid.NewGuid();
            Judger = o;
            State = new List<TestInfo>();
        }
    }
}
