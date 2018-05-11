using System;
using System.Diagnostics;
using System.Text;

namespace JudgeCore
{
    class Helper
    {
        public static Process MakeProcess(string filename, string arguments = "")
        {
            Debug.WriteLine(filename + " " + arguments);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var ret = new Process();
            ret.StartInfo.RedirectStandardOutput = true;
            ret.StartInfo.StandardOutputEncoding = Encoding.GetEncoding(936);
            ret.StartInfo.RedirectStandardError = true;
            ret.StartInfo.StandardErrorEncoding = Encoding.GetEncoding(936);
            ret.StartInfo.CreateNoWindow = true;
            ret.StartInfo.FileName = filename;
            ret.StartInfo.Arguments = arguments;
            return ret;
        }
    }
}
