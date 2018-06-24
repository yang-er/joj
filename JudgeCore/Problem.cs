using JudgeCore.Judger;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

namespace JudgeCore
{
    /// <summary>
    /// 对应的问题
    /// </summary>
    public class Problem
    {
        public int MemoryLimit { get; set; }
        public int ExecuteTimeLimit { get; set; }
        public List<IJudger> Judger { get; set; } = new List<IJudger>();
        public string Title { get; set; }
        public int ProblemId { get; set; }

        public Problem() { }
        public virtual void Load() { }

        public Problem(XmlNode doc)
        {
            Title = doc.SelectSingleNode("title").InnerText;
            ProblemId = int.Parse(doc.SelectSingleNode("id").InnerText);
            MemoryLimit = int.Parse(doc.SelectSingleNode("memory_limit").InnerText);
            ExecuteTimeLimit = int.Parse(doc.SelectSingleNode("time_limit").InnerText);
            var type = doc.SelectSingleNode("judge_type");
            string ops = type is null ? "JudgeCore.Judger.CommonJudge" : type.InnerText;
            var reflect_type = Assembly.GetExecutingAssembly().GetType(ops);

            var tc = doc.SelectSingleNode("test_cases");
            foreach (XmlNode group in tc.ChildNodes)
            {
                Judger.Add(Activator.CreateInstance(reflect_type, group) as IJudger);
            }
        }
    }
}
