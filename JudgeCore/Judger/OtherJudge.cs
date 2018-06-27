using System;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace JudgeCore.Judger
{
    /// <summary>
    /// 特殊评判的一个例子
    /// </summary>
    [Obsolete("Only as an example", true)]
    public class OtherJudge : IJudger
    {
        private readonly string input;

        public void Input(StreamWriter stream)
        {
            try
            {
                stream.Write(input);
                stream.Flush();
                stream.Close();
            }
            catch (IOException ex)
            {
                Trace.WriteLine(ex.ToString());
            }
        }

        public JudgeResult Judge(StreamReader stream)
        {
            stream.Read();
            if (stream.EndOfStream) return JudgeResult.Accepted;
            else if (stream.Read() == 1) return JudgeResult.OutputLimitExceeded;
            else if (stream.Read() == 2) return JudgeResult.WrongAnswer;
            else return JudgeResult.PresentationError;
        }

        public OtherJudge(XmlNode node)
        {
            input = node.SelectSingleNode("input").InnerText;
        }
    }
}
