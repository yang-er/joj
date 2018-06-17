using JudgeCore;
using MySql.Data.MySqlClient;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace JudgeDaemon
{
    partial class Program
    {
        static MySqlConnection MySqlConnection;
        static int TotalQueries = 0;

        [DllImport("libc.so.6")]
        private static extern int getuid();

        static void CheckUser()
        {
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                if (getuid() != 0)
                {
                    Console.WriteLine("FAIL: Now the app is not running as");
                    Console.WriteLine("      root user, fail to continue.");
                    Console.WriteLine();
                    Console.WriteLine("TODO: sudo dotnet JudgeDaemon.dll");
                    Console.WriteLine();
                    Process.GetCurrentProcess().Kill();
                }
            }
            else
            {
                if (Environment.UserName != "Judge")
                {
                    Console.WriteLine("NOTICE: Now the app is not running as special");
                    Console.WriteLine("        user, may take risk to continue.");
                    Console.WriteLine("        It is recommended to switch user.");
                    Console.WriteLine();
                    Console.WriteLine("TODO: runas /user:Judge dotnet JudgeDaemon.dll");
                    Console.WriteLine();
                    Thread.Sleep(7500);
                }
            }
        }

        static void Init()
        {
            SandboxProcess.WorkingDirectory = WorkDir;
            MySqlConnection = new MySqlConnection(SqlConnect);
            MySqlConnection.Open();
            TotalQueries++;
            Console.Error.WriteLine("MySQL connected successfully.");
            LoadCompilers();
            Console.Error.WriteLine("Compiler list created successfully.");
            LoadProblems();
            Console.Error.WriteLine("Problem list loaded successfully.");
            Console.Error.WriteLine();
            var judged_wd = Path.Combine(SandboxProcess.WorkingDirectory, "dest");
            if (Directory.Exists(judged_wd))
                Environment.CurrentDirectory = judged_wd;
        }

        static void Cleanup()
        {
            if (!Running) return;
            Console.WriteLine("Total Queries : " + TotalQueries);
            MySqlConnection.Close();
            Running = false;
        }
    }
}
