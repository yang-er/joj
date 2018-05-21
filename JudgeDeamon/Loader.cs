using JudgeCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace JudgeDeamon
{
    partial class Program
    {
        static readonly List<ICompiler> CompilerList = new List<ICompiler>();
        static readonly Dictionary<int, Problem> Problems = new Dictionary<int, Problem>();

        static void LoadProblems()
        {
            foreach (var file in Directory.EnumerateFiles(".\\prob\\", "*.xml"))
            {
                var doc = new XmlDocument();
                doc.Load(file);
                var prob = new Problem(doc.SelectSingleNode("problem"));
                Problems.Add(prob.ProblemId, prob);
            }
        }

        static void LoadCompilers()
        {
            LoadCompiler<JudgeCore.Compiler.Msvc>();
            LoadCompiler<JudgeCore.Compiler.MinGW>();
            // LoadCompiler<JudgeCore.Compiler.ClangC2>();
            if (CompilerList.Count == 0)
            {
                Console.WriteLine("No compiler found, please check.");
                throw new Exception("No compiler found, please check.");
            }
            Console.WriteLine();
        }

        static void LoadCompiler<T>() where T : ICompiler, new()
        {
            try
            {
                CompilerList.Add(new T());
            }
            catch { }
        }
    }
}
