using MySql.Data.MySqlClient;
using System;
using System.IO;

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
                if (Directory.Exists("F:\\joj\\dest"))
                    Environment.CurrentDirectory = "F:\\joj\\dest";
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
