using System;
using System.Diagnostics;

namespace JudgeDaemon
{
    partial class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Init();
                while (true) JudgeQueue();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                Console.WriteLine("Exception occurred, cannot recovery. ");
            }
            finally
            {
                Cleanup();
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}
