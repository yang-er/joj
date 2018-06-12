using System;
using System.Diagnostics;
using System.Reflection;

namespace JudgeDaemon
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "JLU Online Judge Daemon";
            if (args.Length == 1 && args[0] == "acled")
            {
                MainACLed();
            }
            else
            {
                Console.Write("Do you want to re-ACL? y/n : [ ]\b\b");
                var read = Console.ReadLine();
                if (read == "y" || read == "Y")
                {
                    Main2ACL();
                }
                else
                {
                    MainACLed();
                }
            }
        }

        static void Main2ACL()
        {
            Console.WriteLine("Please turn WerSvc to disabled in services.msc.");
            Console.WriteLine("Press Ctrl-C to exit this daemon.");
            Console.WriteLine();
            var start = new ProcessStartInfo
            {
                Arguments = Assembly.GetExecutingAssembly().Location + " acled",
                FileName = Process.GetCurrentProcess().MainModule.FileName,
                UserName = "Judge",
                Password = new System.Security.SecureString(),
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                RedirectStandardInput = true
            };
            start.Password.AppendChar('1');
            start.Password.AppendChar('2');
            start.Password.AppendChar('3');
            start.Password.AppendChar('4');
            start.Password.AppendChar('5');
            start.Password.AppendChar('6');
            start.RedirectStandardOutput = true;
            
            var proc = Process.Start(start);
            Console.CancelKeyPress += (sender, e) => 
            {
                Console.WriteLine("Ctrl-C detected.");
                proc.Kill();
                e.Cancel = true;
            };
            proc.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
            proc.ErrorDataReceived += (sender, e) => JudgeCore.Helper.WriteDebug(e.Data);
            proc.BeginErrorReadLine();
            proc.BeginOutputReadLine();
            proc.WaitForExit();

            Console.Write("Main thread gone. Press any key to exit...");
            Console.ReadKey();
        }
        
        static void MainACLed()
        {
            try
            {
                Init();
                while (true) JudgeQueue();
            }
            catch (Exception ex)
            {
                JudgeCore.Helper.WriteDebug(ex.ToString());
                Console.WriteLine("Exception occurred, cannot recovery. ");
            }
            finally
            {
                Cleanup();

                if (!Console.IsInputRedirected)
                {
                    Console.WriteLine("Press any key to exit...");
                    Console.Read();
                }
            }
        }
    }
}
