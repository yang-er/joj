using JudgeCore;
using System;
using System.IO;

namespace JudgeTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            JudgerTest();
            CompilerTest();
            Console.ReadKey();
        }

        static void JudgerTest()
        {
            Console.WriteLine("Judging the results...");
            IJudger judger = new JudgeCore.Judger.CommonJudge("1 + 2 = 3\n");
            int ppp = new DirectoryInfo("..\\..\\..\\out").GetFiles().Length;
            for (int i = 1; i <= ppp; i++)
            {
                FileStream fs = new FileStream($"..\\..\\..\\out\\{i}.txt", FileMode.Open);
                var pp = new StreamReader(fs);
                Console.WriteLine("Result {0} : {1}", i, judger.Judge(pp).ToString());
            }
        }

        static void CompilerTest()
        {
            Console.WriteLine("\n\nTest building with msvc141...");
            ICompiler compiler = new JudgeCore.Compiler.Msvc();
            var ret = compiler.Compile("#include <cstdio>\nint main() { return 0; }", Guid.NewGuid());
            if (ret == false) Console.WriteLine(compiler.StandardOutput);
        }
    }
}
