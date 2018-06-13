using JudgeCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace JudgeDaemon
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
            var compiler_xml = new XmlDocument();
            compiler_xml.Load("compilers.xml");
            var xml_root = compiler_xml.SelectSingleNode("compilers");
            foreach (XmlNode sub_node in xml_root.ChildNodes)
                LoadCompiler(sub_node);
            Helper.WriteDebug("");

            if (CompilerList.Count == 0)
            {
                throw new NotImplementedException("No compiler found, please check.");
            }
        }

        static void LoadCompiler<T>() where T : ICompiler, new()
        {
            try
            {
                CompilerList.Add(new T());
            }
            catch { }
        }

        static void LoadCompiler(XmlNode node)
        {
            try
            {
                CompilerList.Add(ICompiler.GetFromXml(node));
            }
            catch { }
        }
    }
}
