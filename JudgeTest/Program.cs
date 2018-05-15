using JudgeCore;
using System;
using System.Collections.Generic;
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
            JobTest();
            Console.Write("Press any key to continue ... ");
            Console.ReadKey();
        }

        static void JudgerTest()
        {
            Console.WriteLine("Judging the results...");
            IJudger judger = new JudgeCore.Judger.CommonJudge("1 + 2 = 3\n");
            int ppp = new DirectoryInfo("..\\..\\out").GetFiles().Length;
            for (int i = 1; i <= ppp; i++)
            {
                FileStream fs = new FileStream($"..\\..\\out\\{i}.txt", FileMode.Open);
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
            // new JudgeCore.Compiler.ClangC2();
            // new JudgeCore.Compiler.MinGW();
        }

        static void JobTest()
        {
            Console.WriteLine("\n\nTest job...");
            // thanks to fmgu2000
            var pearl_in = new List<string>();
            var pearl_out = new List<IJudger>();
            ICompiler compiler = new JudgeCore.Compiler.Msvc();
            for (int i = 1; i <= 10; i++)
            {
                pearl_in.Add(File.ReadAllText($"..\\pearl{i}.in").Replace("\r", ""));
                pearl_out.Add(new JudgeCore.Judger.CommonJudge(File.ReadAllText($"..\\pearl{i}.ans")));
            }

            foreach (var file in Directory.GetFiles($"..\\source"))
            {
                var job = new Job(compiler, pearl_in, pearl_out);
                job.Build(File.ReadAllText(file));
                Console.WriteLine("Judge Result of " + file);
                job.Judge(true);
                if (job.State[0].Result == JudgeResult.CompileError)
                    Console.WriteLine(job.CompileInfo);
                Console.WriteLine("\n");
            }
        }
    }
}
