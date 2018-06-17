using System;
using System.Diagnostics;

namespace JudgeDaemon
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "JLU Online Judge Daemon";
            CheckUser();
            DaemonMode = args.Length == 1;
            Logger.SetTracer(DaemonMode, Cleanup, ServerID);
            Trace.WriteLine($"[{DateTime.Now}]");
            Trace.WriteLine("Welcome to JLU Online Judge Daemon.\n");
            MainThread();
        }

        static bool DaemonMode = false;
        static bool Running = true;

        static void MainThread()
        {
            try
            {
                Init();
                while (Running) JudgeQueue();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
                Console.WriteLine("Exception occurred, cannot recovery. ");
            }
            finally
            {
                Cleanup();

                if (!Console.IsInputRedirected && !DaemonMode)
                {
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }
    }
}
