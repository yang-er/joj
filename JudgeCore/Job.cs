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
        /// 标准输入内容
        /// </summary>
        public List<string> Input { get; }

        /// <summary>
        /// 标准输出内容
        /// </summary>
        public List<IJudger> Output { get; }

        /// <summary>
        /// 目前状态
        /// </summary>
        public List<TestInfo> State { get; }

        /// <summary>
        /// 编译
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool Build(string code)
        {
            return Compiler.Compile(code, RunID);
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
                pro.Start();
                bool tle = false;
                Task.Run(async () => { await Task.Delay(1000); if (!pro.HasExited) { if (ti.Result == JudgeResult.Running) tle = true; pro.Kill(); } });
                pro.StandardInput.Write(Input[id]);
                pro.StandardInput.Close();
                ti.Result = Output[id].Judge(pro.StandardOutput);
                pro.WaitForExit();
                if (tle)
                {
                    ti.Result = JudgeResult.TimeLimitExceeded;
                    ti.Time = -1;
                    return;
                }

                // Judge time span
                var len = DateTime.Now - pro.StartTime;
                Debug.WriteLine("Runtime: {0}ms", len.TotalMilliseconds);
                ti.Memory = Helper.GetProcessMemoryInfo(pro);
                Debug.WriteLine("Memory: {0}kb", ti.Memory / 1024);
                if (ti.Memory / 1048576 > 128) ti.Result = JudgeResult.MemoryLimitExceeded;
                ti.Time = len.TotalMilliseconds;
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
            if (pro != null)
            {
                pro.Start();
                pro.Kill();
            }

            for (int i = 0; i < Output.Count; i++)
            {
                Judge(i);
                if (show_log) Console.WriteLine("{0}ms\t{1}", (int)Math.Round(State[i].Time), State[i].Result.ToString());
            }
        }

        /// <summary>
        /// 初始化任务
        /// </summary>
        /// <param name="cl">编译器</param>
        /// <param name="i">标准输入</param>
        /// <param name="o">评价标准</param>
        public Job(ICompiler cl, List<string> i, List<IJudger> o)
        {
            Compiler = cl;
            RunID = Guid.NewGuid();
            Input = i;
            Output = o;
            State = new List<TestInfo>();
        }
    }
}
