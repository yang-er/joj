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
        /// <summary>
        /// 内存限制
        /// </summary>
        public int MemoryLimit { get; set; }

        /// <summary>
        /// 运行时间限制
        /// </summary>
        public int ExecuteTimeLimit { get; set; }

        /// <summary>
        /// 判断答案对错
        /// </summary>
        public List<IJudger> Judger { get; set; } = new List<IJudger>();

        /// <summary>
        /// 问题名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 问题编号
        /// </summary>
        public int ProblemId { get; set; }

        /// <summary>
        /// 创建问题
        /// </summary>
        public Problem() { }

        /// <summary>
        /// 拓展保留内容
        /// </summary>
        [Obsolete("Extend", true)]
        public virtual void Load() { }

        /// <summary>
        /// 创建问题
        /// </summary>
        /// <param name="doc">所用XML节点</param>
        public Problem(XmlNode doc)
        {
            Title = doc.SelectSingleNode("title").InnerText;
            ProblemId = int.Parse(doc.SelectSingleNode("id").InnerText);
            MemoryLimit = int.Parse(doc.SelectSingleNode("memory_limit").InnerText);
            ExecuteTimeLimit = int.Parse(doc.SelectSingleNode("time_limit").InnerText);

            // 判答案的工具
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
