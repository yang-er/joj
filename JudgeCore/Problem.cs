using System.Collections.Generic;

namespace JudgeCore
{
    public class Problem
    {
        public int MemoryLimit { get; set; }
        public int ExecuteTimeLimit { get; set; }
        public List<IJudger> Judger { get; set; } = new List<IJudger>();
        public string Title { get; set; }
        public int ProblemId { get; set; }

        public virtual void Load() { }
    }
}
