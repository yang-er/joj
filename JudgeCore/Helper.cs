﻿using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace JudgeCore
{
    public class Helper
    {
        #region Startup Functions

        static Action<string> writeDbg;
        static readonly Helper intel = new Helper();
        static string startDir = "";
        static string appedix = "";

        public Helper()
        {
            if (Console.IsErrorRedirected)
                writeDbg = Console.Error.WriteLine;
            else
                writeDbg = (str) => Debug.WriteLine(str);

            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                OS = new Platform.Linux();
                startDir = "/usr/judge/";
            }
            else
            {
                OS = new Platform.Win32();
                startDir = "F:\\joj";
                appedix = ".exe";
            }
        }

        #endregion

        /// <summary>
        /// 平台相关函数
        /// </summary>
        public static IPlatform OS { get; private set; }

        /// <summary>
        /// 输出调试信息的函数
        /// </summary>
        public static void WriteDebug(string str) => writeDbg(str);

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
            var filename = new FileInfo(guid.ToString("D") + appedix).FullName;
            if (!File.Exists(filename)) return null;
            var ret = new ProcessStartInfo
            {
                RedirectStandardOutput = true,
                StandardOutputEncoding = Encoding.GetEncoding(936),
                RedirectStandardInput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                FileName = filename,
            };
            
            ret.WorkingDirectory = Directory.Exists(startDir) ? startDir : Environment.CurrentDirectory;
            return ret;
        }
    }
}
