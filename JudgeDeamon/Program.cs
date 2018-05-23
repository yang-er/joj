using System;
using System.Diagnostics;
using System.Reflection;

namespace JudgeDaemon
{
    partial class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 2 && args[0] == "acled")
            {
                // Process.GetProcessById(int.Parse(args[1])).Exited += (sender, e) => Process.GetCurrentProcess().Kill();
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
            var refer = Assembly.GetExecutingAssembly();
            var start = new ProcessStartInfo
            {
                Arguments = refer.Location + " acled " + Process.GetCurrentProcess().Id,
                FileName = Process.GetCurrentProcess().MainModule.FileName,
                UserName = "Judge",
                Password = new System.Security.SecureString(),
                WorkingDirectory = "F:\\joj\\",
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
            proc.OutputDataReceived += RedirectStdin;
            proc.ErrorDataReceived += RedirectStderr;
            proc.BeginErrorReadLine();
            proc.BeginOutputReadLine();
            proc.WaitForExit();

            Console.Write("Main thread gone. Press any key to exit...");
            Console.ReadKey();
        }

        static void RedirectStdin(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }

        static void RedirectStderr(object sender, DataReceivedEventArgs e)
        {
            Debug.WriteLine(e.Data);
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
