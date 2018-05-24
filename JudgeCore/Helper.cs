using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace JudgeCore
{
    public class Helper
    {
        static Action<string> writeDbg;

        /// <summary>
        /// 输出调试信息的函数
        /// </summary>
        public static Action<string> WriteDebug
        {
            get
            {
                if (writeDbg is null)
                {
                    if (Console.IsErrorRedirected)
                        writeDbg = Console.Error.WriteLine;
                    else
                        writeDbg = (str) => Debug.WriteLine(str);
                }

                return writeDbg;
            }
        }

        /// <summary>
        /// 创建内部进程，不受限制的那种
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="arguments">参数</param>
        /// <returns>等待启动的进程</returns>
        public static Process MakeProcess(string filename, string arguments = "")
        {
            WriteDebug(filename + " " + arguments);
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

        /// <summary>
        /// 创建评价进程，受限制的那种
        /// </summary>
        /// <param name="guid">程序标识符</param>
        /// <returns>等待启动的进程</returns>
        public static ProcessStartInfo MakeJudgeInfo(Guid guid)
        {
            var filename = new FileInfo(guid.ToString("D") + ".exe").FullName;
            if (!File.Exists(filename)) return null;
            var ret = new ProcessStartInfo
            {
                RedirectStandardOutput = true,
                StandardOutputEncoding = Encoding.GetEncoding(936),
                RedirectStandardInput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                FileName = filename,
                // UserName = "Judge",
                // Password = new System.Security.SecureString()
            };

            /* ret.Environment["Path"] = "\\;"; */
            ret.WorkingDirectory = Directory.Exists("F:\\joj") ? "F:\\joj" : Environment.CurrentDirectory;
            return ret;
        }
    }
}
