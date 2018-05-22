using JudgeCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading;

namespace JudgeDeamon
{
    partial class Program
    {
        static readonly Queue<int> WaitingQueue = new Queue<int>();
        static long last_all = -1;
        
        static void JudgeQueue()
        {
            var command = MySqlConnection.CreateCommand();
            command.CommandText = "select count(1) from `submission` where `status`=8";
            TotalQueries++;
            long all = (long)command.ExecuteScalar();

            if (last_all != all)
            {
                Console.WriteLine("Current queue length : {0}", all);
                last_all = all;
                if (all == 0)
                    Console.WriteLine("Waiting for the next task...");
            }

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

            command.CommandText = "update `submission` set" +
                " `status` = 9" +
                $" where `runid`={runid}";
            TotalQueries++;
            command.ExecuteNonQuery();

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
                    TimeLimit = prob.ExecuteTimeLimit,
                    ActiveJob = JobObject,
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
                    $"values ({runid},{(int)res.Result},{res.Memory / 1024},{res.Time},{res.ExitCode})";

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
