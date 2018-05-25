using System.Runtime.InteropServices;

namespace JudgeUtility
{
    partial class Program
    {
        [DllImport("msvcrt.dll", EntryPoint = "system")]
        static extern bool CommandSystem(string str);

        static void CleanUp()
        {
            CommandSystem("echo del F:\\joj\\dest\\*.obj");
            CommandSystem("del F:\\joj\\dest\\*.obj");
            CommandSystem("echo del F:\\joj\\dest\\*.o");
            CommandSystem("del F:\\joj\\dest\\*.o");
            CommandSystem("echo del F:\\joj\\dest\\*.exe");
            CommandSystem("del F:\\joj\\dest\\*.exe");
            CommandSystem("echo del F:\\joj\\dest\\*.cpp");
            CommandSystem("del F:\\joj\\dest\\*.cpp");
        }
    }
}
