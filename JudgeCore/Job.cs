﻿using System;
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

        private void CheckRuntime(Process pro, ref TestInfo ti, ref bool tle)
        {
            while (!pro.HasExited
                && pro.TotalProcessorTime.TotalMilliseconds < 1001
                && (DateTime.Now - pro.StartTime).TotalMilliseconds < 2000
                && pro.PeakWorkingSet64 < MemoryLimit * 1048576L) ;
            if (!pro.HasExited)
            {
                if (ti.Result == JudgeResult.Running)
                {
                    tle = true;
                }

                pro.Kill();
            }
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

                var pro = Helper.MakeJudgeProcess(RunID);
                if (pro is null)
                {
                    ti.Result = JudgeResult.CompileError;
                    return;
                }

                ti.Result = JudgeResult.Running;

                // Judge process
                if (Compiler.GetType().Name == "MinGW")
                    pro.StartInfo.Environment["PATH"] += $"C:\\MinGW\\bin;";
                pro.Start();
                bool tle = false;
                Task.Run(() => CheckRuntime(pro, ref ti, ref tle));
                Judger[id].Input(pro.StandardInput);
                pro.StandardInput.Close();
                ti.Result = Judger[id].Judge(pro.StandardOutput);
                pro.WaitForExit();
                if (tle)
                {
                    if (pro.TotalProcessorTime.TotalMilliseconds >= 1001)
                        ti.Result = JudgeResult.TimeLimitExceeded;
                    else
                        ti.Result = JudgeResult.Pending;
                }

                // Judge extra info
                ti.Time = pro.TotalProcessorTime.TotalMilliseconds;
                Debug.WriteLine("Runtime: {0}ms", ti.Time);
                if (ti.Time >= 1001)
                    ti.Result = JudgeResult.TimeLimitExceeded;
                ti.Memory = Helper.GetProcessMemoryInfo(pro);
                Debug.WriteLine("Memory: {0}kb", ti.Memory / 1024);
                if (ti.Memory / 1048576 > MemoryLimit)
                    ti.Result = JudgeResult.MemoryLimitExceeded;
                ti.ExitCode = pro.ExitCode;
                Debug.WriteLine("ExitCode: 0x" + ti.ExitCode.ToString("x"));
                if (!tle && ti.ExitCode != 0) ti.Result = JudgeResult.RuntimeError;
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
            var pro = Helper.MakeJudgeProcess(RunID);
            string full_path = "";
            if (pro != null)
            {
                if (Compiler.GetType().Name == "MinGW")
                    pro.StartInfo.Environment["PATH"] += $"C:\\MinGW\\bin;";
                full_path = new FileInfo(pro.StartInfo.FileName).FullName;
                Helper.WerAddExcludedApplication(full_path, false);
                pro.Start();
                pro.StandardInput.Close();
                pro.StandardOutput.Close();
                Task.Run(async () => { await Task.Delay(5000); if (!pro.HasExited) pro.Kill(); });
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

            if (full_path != "")
                Helper.WerRemoveExcludedApplication(full_path, false);
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
