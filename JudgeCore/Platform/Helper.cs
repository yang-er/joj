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

        public Helper()
        {
            if (Console.IsErrorRedirected)
                writeDbg = Console.Error.WriteLine;
            else
                writeDbg = (str) => Debug.WriteLine(str);

            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                WorkingDirectory = "/usr/judge/";
                Appedix = ".out";
            }
            else
            {
                WorkingDirectory = "F:\\joj";
                Appedix = ".exe";
            }
        }

        #endregion
        
        /// <summary>
        /// 文件名后缀
        /// </summary>
        public static string Appedix { get; private set; }

        /// <summary>
        /// 工作目录
        /// </summary>
        public static string WorkingDirectory { get; private set; }

        /// <summary>
        /// 输出调试信息的函数
        /// </summary>
        public static void WriteDebug(string str) => writeDbg(str);

        /// <summary>
        /// 写入调试时间戳
        /// </summary>
        public static void WriteDebugTimestamp() => writeDbg("[" + DateTime.Now.ToLongTimeString() + "]");
    }
}
