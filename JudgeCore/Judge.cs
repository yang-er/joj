using System.IO;

namespace JudgeCore
{
    /// <summary>
    /// 测评器
    /// </summary>
    public interface IJudger
    {
        /// <summary>
        /// 得到测评结果
        /// </summary>
        /// <param name="stream">读入流</param>
        /// <returns>判断结果</returns>
        JudgeResult Judge(StreamReader stream);
    }
}
