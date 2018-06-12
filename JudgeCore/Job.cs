using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using static JudgeCore.Helper;

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
        /// 检查运行时间
        /// </summary>
        /// <param name="pro">运行的进程</param>
        /// <param name="ti">测试结果</param>
        /// <param name="tle">是否超时</param>
        private void CheckRuntime(SandboxProcess pro, ref TestInfo ti, ref bool tle)
        {
            while (!pro.HasExited && !pro.OutOfLimit) ;
            if (!pro.HasExited) pro.Kill();
        }

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

                var proc = SandboxProcess.Create(RunID.ToString("D") + Appedix, stdin: true, cd: true);
                if (proc is null) return ti.Result = JudgeResult.CompileError;
                
                ti.Result = JudgeResult.Running;

                // Judge process
                if (Compiler.GetType().Name == "MinGW")
                    proc.StartInfo.Environment["PATH"] = Compiler.ToolchainPath[0] + ";";

                proc.Setup(MemoryLimit, TimeLimit, 1);
                proc.Start();
                
                bool tle = false;
                Task.Run(() => CheckRuntime(proc, ref ti, ref tle));
                Task.Run(() => Judger[id].Input(proc.StandardInput));
                ti.Result = Judger[id].Judge(proc.StandardOutput);
                proc.WaitForExit();
                proc.Kill();

                if (proc.ExitCode == -1) ti.Result = JudgeResult.UndefinedError;

                // Judge extra info
                ti.Time = proc.TotalTime;
                WriteDebug($"Runtime: {ti.Time}ms");
                if (ti.Time >= TimeLimit)
                    ti.Result = JudgeResult.TimeLimitExceeded;
                ti.Memory = (long)proc.MaxMemory;
                WriteDebug($"Memory: {ti.Memory / 1024}kb");
                if (ti.Memory > MemoryLimit << 20)
                    ti.Result = JudgeResult.MemoryLimitExceeded;
                ti.ExitCode = proc.ExitCode;
                WriteDebug("ExitCode: 0x" + ti.ExitCode.ToString("x"));
                if (ti.ExitCode != 0 && ti.ExitCode != -1 && ti.ExitCode != -2) ti.Result = JudgeResult.RuntimeError;
            }
            catch (Exception ex)
            {
                WriteDebug(ex.ToString());
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
            var proc = SandboxProcess.Create(RunID.ToString("D") + Appedix, cd: true);

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
            
            proc.Setup(MemoryLimit, TimeLimit, 1);
            proc.Start();
            proc.StandardOutput.Close();
            proc.WaitForExit(1000);
            proc.Kill();

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
            WriteDebugTimestamp();
            WriteDebug("New judge job entered.");
            Compiler = cl;
            RunID = Guid.NewGuid();
            Judger = o;
            State = new List<TestInfo>();
        }
    }
}
