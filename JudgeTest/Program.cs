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
            Console.ReadKey();
        }

        static void JudgerTest()
        {
            IJudger judger = new JudgeCore.Judger.CommonJudge("1 + 2 = 3\n");
            int ppp = new DirectoryInfo("..\\..\\..\\out").GetFiles().Length;
            for (int i = 1; i <= ppp; i++)
            {
                FileStream fs = new FileStream($"..\\..\\..\\out\\{i}.txt", FileMode.Open);
                var pp = new StreamReader(fs);
                Console.WriteLine("Result {0} : {1}", i, judger.Judge(pp).ToString());
            }
        }
    }
}
