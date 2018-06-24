using System.Diagnostics;
using System.IO;
using System.Xml;

namespace JudgeCore.Judger
{
    /// <summary>
    /// 普通测评
    /// </summary>
    public class CommonJudge : IJudger
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
            string r2 = result.Replace("\n", "").Replace("\t", "").Replace(" ", "");

            if (r2 != _realthing)
                return JudgeResult.WrongAnswer;
            else if (result == _correct)
                return JudgeResult.Accepted;
            else if (CheckWithoutLn(result, _correct))
                return JudgeResult.Accepted;
            else if (CheckWithoutLn(_correct, result))
                return JudgeResult.Accepted;
            else
                return JudgeResult.PresentationError;
        }

        private static bool CheckWithoutLn(string a, string b)
        {
            return a.EndsWith('\n')
                    && !b.EndsWith('\n')
                    && a.Substring(0, a.Length - 1) == b;
        }

        public CommonJudge(string inp, string ans)
        {
            _input = inp;
            _correct = ans.Replace("\r", "");
            _realthing = _correct.Replace("\n", "").Replace("\t", "").Replace(" ", "");
        }

        public CommonJudge(XmlNode node) : this(
            node.SelectSingleNode("input").InnerText,
            node.SelectSingleNode("output").InnerText
        ) { }
    }
}
