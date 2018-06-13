using MySql.Data.MySqlClient;
using System;
using System.IO;
using static JudgeCore.Helper;

namespace JudgeDaemon
{
    partial class Program
    {
        static MySqlConnection MySqlConnection;
        static int TotalQueries = 0;

        static void Init()
        {
            MySqlConnection = new MySqlConnection("server=localhost;User Id=root;password=root;Database=judge;SSLMode=none;Charset=utf8");
            MySqlConnection.Open();
            TotalQueries++;
            Console.Error.WriteLine("MySQL connected successfully.");
            LoadCompilers();
            Console.Error.WriteLine("Compiler list created successfully.");
            LoadProblems();
            Console.Error.WriteLine("Problem list loaded successfully.");
            Console.Error.WriteLine();
            var judged_wd = Path.Combine(WorkingDirectory, "dest");
            if (Directory.Exists(judged_wd))
                Environment.CurrentDirectory = judged_wd;
        }

        static void Cleanup()
        {
            Console.WriteLine("Total Queries : " + TotalQueries);
            MySqlConnection.Close();
        }
    }
}
