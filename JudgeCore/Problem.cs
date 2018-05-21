using JudgeCore.Judger;
using System.Collections.Generic;
using System.Xml;

namespace JudgeCore
{
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

            var tc = doc.SelectSingleNode("test_cases");
            foreach (XmlNode group in tc.ChildNodes)
            {
                Judger.Add(
                    new CommonJudge(
                        group.SelectSingleNode("input").InnerText,
                        group.SelectSingleNode("output").InnerText)
                    );
            }
        }
    }
}
