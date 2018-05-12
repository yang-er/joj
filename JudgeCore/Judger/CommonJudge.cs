using System.IO;

namespace JudgeCore.Judger
{
    /// <summary>
    /// 普通测评
    /// </summary>
    public class CommonJudge : IJudger
    {
        private readonly string _correct;
        private readonly string _realthing;
        public bool Special => false;

        public JudgeResult Judge(StreamReader stream)
        {
            char[] tojudge = new char[128 * 1024];
            stream.Read(tojudge, 0, 128 * 1024);

            if (!stream.EndOfStream)
            {
                stream.Close();
                return JudgeResult.OutputLimitExceeded;
            }
            
            string result = new string(tojudge).TrimEnd('\0').Replace("\r", "");
            string r2 = result.Replace("\n", "").Replace("\t", "").Replace(" ", "");

            if (r2 != _realthing)
            {
                return JudgeResult.WrongAnswer;
            }
            
            if (result == _correct)
            {
                return JudgeResult.Accepted;
            }

            if (result.EndsWith('\n') && !_correct.EndsWith('\n') && result.Substring(0, result.Length - 1) == _correct)
            {
                return JudgeResult.Accepted;
            }

            if (_correct.EndsWith('\n') && !result.EndsWith('\n') && _correct.Substring(0, _correct.Length - 1) == result)
            {
                return JudgeResult.Accepted;
            }

            return JudgeResult.PresentationError;
        }

        public CommonJudge(string ans)
        {
            _correct = ans.Replace("\r", "");
            _realthing = _correct.Replace("\n", "").Replace("\t", "").Replace(" ", "");
        }
    }
}
