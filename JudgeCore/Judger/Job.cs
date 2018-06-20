using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        /// 最大运行时间限制
        /// </summary>
        public int TimeLimit { get; set; } = 1000;

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
        
        /// <summary>
        /// 对某一次进行评价
        /// </summary>
        /// <param name="id"></param>
        public JudgeResult Judge(int id)
        {
            var ti = new TestInfo();
            try
            {
                ti.Result = JudgeResult.Pending;

                var proc = Compiler.CreateJudgeProcess(RunID);
                if (proc is null) return ti.Result = JudgeResult.CompileError;
                
                ti.Result = JudgeResult.Running;

                // Judge process
                // Compiler.SetJudgeEnv(proc);
                proc.Setup(MemoryLimit, TimeLimit, 1, true);
                proc.Start();
                
                Task.Run(() => proc.Watch());
                Task.Run(() => Judger[id].Input(proc.StandardInput));
                ti.Result = Judger[id].Judge(proc.StandardOutput);
                proc.WaitForExit();

                if (proc.ExitCode != 0)
                    ti.Result = JudgeResult.UndefinedError;

                // Judge extra info
                ti.Time = proc.TotalTime;
                if (proc.IsTimeLimitExceeded)
                    ti.Result = JudgeResult.TimeLimitExceeded;
                ti.Memory = (long)proc.MaxMemory;
                if (proc.IsMemoryLimitExceeded)
                    ti.Result = JudgeResult.MemoryLimitExceeded;
                ti.ExitCode = proc.ExitCode;
                if (proc.IsRuntimeError)
                    ti.Result = JudgeResult.RuntimeError;
                Trace.WriteLine($"Runtime: {ti.Time}ms, Memory: {ti.Memory / 1024}kb, ExitCode: 0x" + ti.ExitCode.ToString("x"));
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
                ti.Result = JudgeResult.RuntimeError;
            }
            finally
            {
                State.Add(ti);
            }

            return ti.Result;
        }

        /// <summary>
        /// 批量评价
        /// </summary>
        public void Judge(bool show_log = false)
        {
            var proc = Compiler.CreateJudgeProcess(RunID);

            if (proc is null)
            {
                State.Add(new TestInfo { Result = JudgeResult.CompileError });
                if (show_log) Console.WriteLine("0ms\t0mb\tCompileError");
                return;
            }

            /**********************************************************
             *  Old ways: Windows Error Reporting APIs
             *  - Open privilige to JudgeUser for
             *      HKLM\SOFTWARE\Microsoft\Windows\Windows Error Reporting\ExcludedApplications
             *  # WerAddExcludedApplication(pi.FileName, true);
             *  # WerRemoveExcludedApplication(pi.FileName, true);
             * 
             *  New ways:
             *  - Setup the flag JOB_OBJECT_LIMIT_DIE_ON_UNHANDLED_EXCEPTION
             **********************************************************/

            proc.StartInfo.Environment.Clear();
            if (Compiler.GetType().Name == "MinGW")
                proc.StartInfo.Environment["PATH"] = Compiler.ToolchainPath[0] + ";";
            else
                proc.StartInfo.Environment["PATH"] = "";

            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                // Prefetch
                proc.Setup(MemoryLimit, TimeLimit, 1, true);
                proc.Start();
                proc.StandardOutput.Close();
                proc.WaitForExit(1000);
                proc.Kill();
            }

            for (int i = 0; i < Judger.Count; i++)
            {
                Judge(i);
                if (show_log) Console.WriteLine("{0}ms\t{1}mb\t{2}", 
                    (int)Math.Round(State[i].Time), 
                    (int)Math.Round(State[i].Memory / 1048576.0), 
                    State[i].Result.ToString());
            }
        }

        /// <summary>
        /// 初始化任务
        /// </summary>
        /// <param name="cl">编译器</param>
        /// <param name="o">评测器</param>
        public Job(ICompiler cl, List<IJudger> o)
        {
            Trace.WriteLine($"[{DateTime.Now}]");
            Trace.WriteLine("New judge job entered.");
            Compiler = cl;
            RunID = Guid.NewGuid();
            Judger = o;
            State = new List<TestInfo>();
        }
    }
}
