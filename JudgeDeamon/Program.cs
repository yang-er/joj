using System;
using System.Diagnostics;

namespace JudgeDeamon
{
    class Program
    {
        static IntPtr JobObject;

        static void Main(string[] args)
        {
            try
            {
                JobObject = JudgeCore.Platform.Win32.SetupSandbox(128, 1000, 2);
                Console.WriteLine("JobObject created successfully.");
                JudgeCore.Platform.Win32.AssignProcessToJobObject(JobObject, Process.GetCurrentProcess().Handle);
                JudgeCore.Platform.Win32.IsProcessInJob(Process.GetCurrentProcess().Handle, JobObject, out bool Is);
                Console.WriteLine("Hello World! This app job is : " + Is);
                Console.ReadKey();
            }
            finally
            {
                JudgeCore.Platform.Win32.UnsetSandbox(JobObject);
                JudgeCore.Platform.Win32.CloseHandle(JobObject);
            }
        }
    }
}
