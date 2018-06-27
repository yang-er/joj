using System;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace JudgeCore.Judger
{
    /// <summary>
    /// 特殊判断（类HUSTOJ）
    /// </summary>
    public class SpecialJudge : IJudger
    {
        private readonly string _correct;
        private readonly string _realthing;
        private readonly string _input;
        public bool Special => false;

        public void Input(StreamWriter stream)
        {
            try
            {
                stream.Write(_input);
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
            char[] tojudge = new char[256 * 1024];
            var read_len = 0;
            while (256 * 1024 > read_len && !stream.EndOfStream)
            {
                read_len += stream.Read(tojudge, read_len, 256 * 1024 - read_len);
            }

            if (!stream.EndOfStream)
            {
                stream.Close();
                return JudgeResult.OutputLimitExceeded;
            }

            string result = new string(tojudge).TrimEnd('\0').Replace("\r", "");
            
            // Compile an assembly and run
            // But not decided.
            throw new NotImplementedException();
        }

        public SpecialJudge(XmlNode node)
        {
            _input = node.SelectSingleNode("input").InnerText;
            _correct = node.SelectSingleNode("output").InnerText.Replace("\r", "");
            _realthing = _correct.Replace("\n", "").Replace("\t", "").Replace(" ", "");
        }
    }
}
