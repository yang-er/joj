using System;
using System.IO;
using System.Text;

namespace JudgeUtility
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "JLU Online Judge Utility";
            while (true)
            {
                Console.Clear();
                Console.WriteLine("---- Main menu ----");
                Console.WriteLine(" 0. Exit Utility");
                Console.WriteLine(" 1. Problem Create");
                Console.WriteLine(" 2. Config Cache");
                Console.WriteLine(" 3. Program Clean");
                Console.WriteLine("-------------------");
                Console.Write("Your Choice : [ ]\b\b");
                var op = Console.ReadLine();
                Console.WriteLine();
                switch (op)
                {
                    case "0": return;
                    case "1": ProblemCreator(); break;
                    case "2": ConfigCache(); break;
                    case "3": CleanUp(); break;
                    default: break;
                }
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }
    }
}
