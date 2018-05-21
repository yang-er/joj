using JudgeCore;
using System.Collections.Generic;

namespace JudgeDeamon
{
    partial class Program
    {
        static readonly List<ICompiler> CompilerList = new List<ICompiler>();
        static readonly Dictionary<int,Problem> Problems = new Dictionary<int,Problem>();
        
        static void Main(string[] args)
        {
            try
            {
                Init();
                while (true) JudgeQueue();
            }
            // catch (Exception)
            // {
            //     Console.WriteLine("Exception occurred, cannot recovery. ");
            // }
            finally
            {
                Cleanup();
            }
        }
    }
}
