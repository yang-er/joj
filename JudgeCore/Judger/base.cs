using System.IO;

namespace JudgeCore
{
    /// <summary>
    /// 测评器
    /// </summary>
    public interface IJudger
    {
        /// <summary>
        /// 写入评测内容
        /// </summary>
        /// <param name="stream">输入流</param>
        void Input(StreamWriter stream);

        /// <summary>
        /// 得到测评结果
        /// </summary>
        /// <param name="stream">输出流</param>
        /// <returns>判断结果</returns>
        JudgeResult Judge(StreamReader stream);
    }
}
