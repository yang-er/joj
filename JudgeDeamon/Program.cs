using JudgeCore;
using JudgeCore.Judger;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using static JudgeCore.Platform.Win32;

namespace JudgeDeamon
{
    class Program
    {
        static IntPtr JobObject;
        static MySqlConnection MySqlConnection;
        static readonly Queue<int> WaitingQueue = new Queue<int>();
        static readonly List<ICompiler> CompilerList = new List<ICompiler>();
        static readonly Dictionary<int,Problem> Problems = new Dictionary<int,Problem>();
        static int TotalQueries = 0;

        #region Environment Setup

        static void Init()
        {
            try
            {
                MySqlConnection = new MySqlConnection("server=localhost;User Id=root;password=root;Database=judge;SSLMode=none");
                MySqlConnection.Open();
                TotalQueries++;
                JobObject = SetupSandbox(128, 1000, 2);
                Console.WriteLine("JobObject created successfully.");
                AssignProcessToJobObject(JobObject, Process.GetCurrentProcess().Handle);
                CompilerList.Add(new JudgeCore.Compiler.Msvc());
                Problems.Add(1001, new Problem
                {
                    Title = "A + B Problem",
                    ExecuteTimeLimit = 100,
                    MemoryLimit = 32,
                    ProblemId = 1001,
                    Judger =
                    {
                        new CommonJudge("1 2\n", "3\n"),
                        new CommonJudge("0 0\n", "0\n"),
                        new CommonJudge("1 -1\n", "0\n"),
                        new CommonJudge("145565 2\n", "145567\n"),
                        new CommonJudge("145565 145565\n", "291130\n"),
                        new CommonJudge("122 2\n", "124\n"),
                        new CommonJudge("-1 2\n", "1\n"),
                        new CommonJudge("10 2\n", "12\n"),
                        new CommonJudge("01 2\n", "3\n"),
                        new CommonJudge("1 200\n", "201\n")
                    }
                });
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.ToString());
                throw ex;
            }
        }

        static void Cleanup()
        {
            Console.WriteLine("Total Queries : " + TotalQueries);
            MySqlConnection.Close();
            UnsetSandbox(JobObject);
        }

        static void Main(string[] args)
        {
            try
            {
                Init();
                while (true) JudgeQueue();
            }
            // catch (Exception)
            // {
            //     Console.WriteLine("Exception occurred, cannot recovery. ");
            // }
            finally
            {
                Cleanup();
            }
        }

        #endregion

        static void JudgeQueue()
        {
            var command = MySqlConnection.CreateCommand();
            command.CommandText = "select count(1) from `submission` where `status`=8";
            TotalQueries++;
            long all = (long)command.ExecuteScalar();
            Console.WriteLine("Current queue length : {0}", all);
            if (all > 0)
            {
                command.CommandText = "select `runid` from `submission` where `status`=8";
                TotalQueries++;
                var vlist = command.ExecuteReader();
                while (vlist.Read())
                {
                    WaitingQueue.Enqueue(vlist.GetInt32(0));
                }
                vlist.Close();
                vlist.Dispose();
                command.Dispose();

                while (WaitingQueue.Count > 0)
                    Judge(WaitingQueue.Dequeue());
            }
            else
            {
                command.Dispose();
                Thread.Sleep(5000);
            }
        }

        static void Judge(int runid)
        {
            var command = MySqlConnection.CreateCommand();
            command.CommandText = "select `code`,`proid`,`lang` from `code` where `runid`=" + runid;
            TotalQueries++;
            Problem prob;
            Job job;

            using (var obj = command.ExecuteReader())
            {
                obj.Read();
                prob = Problems[obj.GetInt32(1)];
                job = new Job(CompilerList[obj.GetInt32(2)], prob.Judger)
                {
                    MemoryLimit = prob.MemoryLimit,
                    TimeLimit = prob.ExecuteTimeLimit
                };
                Console.WriteLine();
                Console.WriteLine("Judge RunID #" + runid);
                job.Build(obj.GetString(0));
                obj.Close();
            }
            
            job.Judge(true);
            Console.WriteLine();
            JudgeResult final = JudgeResult.Pending;
            long exemem = 0, exetime = 0, acs = 0;

            // Insert detail info
            foreach (var res in job.State)
            {
                command.CommandText = "insert into `details` " +
                    "(`runid`,`status`,`exemem`,`exetime`,`exitcode`) " +
                    $"values ({runid},{(int)res.Result},{res.Memory/1024},{res.Time},{res.ExitCode})";

                if (final == JudgeResult.Pending)
                {
                    final = res.Result;
                }
                else if (final == JudgeResult.Accepted)
                {
                    final = res.Result;
                }

                if (res.Result == JudgeResult.Accepted)
                    acs++;
                exetime += (long)res.Time;
                exemem += res.Memory / 1024;
                TotalQueries++;
                command.ExecuteNonQuery();
            }

            // Set when compile error
            if (job.State[0].Result == JudgeResult.CompileError)
            {
                command.CommandText = "update `code` set `ce` = \"" + 
                    MySqlHelper.DoubleQuoteString(job.CompileInfo) +
                    "\" where `runid`=" + runid;
                TotalQueries++;
                command.ExecuteNonQuery();
            }

            command.CommandText = "update `submission` set" +
                $" `exetime` = {exetime}," +
                $" `exemem` = {exemem}," +
                $" `grade` = {(int)(100.0 / job.Judger.Count * acs)}," +
                $" `status` = {(int)final}" +
                $" where `runid`={runid}";
            TotalQueries++;
            command.ExecuteNonQuery();
        }
    }
}
