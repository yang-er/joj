using JudgeCore;
using JudgeCore.Judger;
using MySql.Data.MySqlClient;
using System;
using System.Diagnostics;
using static JudgeCore.Platform.Win32;

namespace JudgeDeamon
{
    partial class Program
    {
        static IntPtr JobObject;
        static MySqlConnection MySqlConnection;
        static int TotalQueries = 0;

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
    }
}
