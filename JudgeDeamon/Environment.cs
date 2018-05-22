﻿using MySql.Data.MySqlClient;
using System;
using System.Diagnostics;
using System.IO;
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
                Console.Title = "JLU Online Judge Daemon";
                MySqlConnection = new MySqlConnection("server=localhost;User Id=root;password=root;Database=judge;SSLMode=none");
                MySqlConnection.Open();
                TotalQueries++;
                Console.WriteLine("MySQL connected successfully.");
                JobObject = SetupSandbox(128, 1000, 1);
                Console.WriteLine("JobObject created successfully.");
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
            UnsetSandbox(JobObject);
        }
    }
}
