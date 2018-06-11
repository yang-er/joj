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
            try
            {
                MySqlConnection = new MySqlConnection("server=localhost;User Id=root;password=root;Database=judge;SSLMode=none;Charset=utf8");
                MySqlConnection.Open();
                TotalQueries++;
                Console.WriteLine("MySQL connected successfully.");
                LoadCompilers();
                Console.WriteLine("Compiler list created successfully.");
                LoadProblems();
                Console.WriteLine("Problem list loaded successfully.");
                Console.WriteLine();
                Console.WriteLine();
                var judged_wd = Path.Combine(WorkingDirectory, "dest");
                if (Directory.Exists(judged_wd))
                    Environment.CurrentDirectory = judged_wd;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        static void Cleanup()
        {
            Console.WriteLine("Total Queries : " + TotalQueries);
            MySqlConnection.Close();
        }
    }
}
