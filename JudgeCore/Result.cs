namespace JudgeCore
{
    /// <summary>
    /// 判断结果
    /// </summary>
    public enum JudgeResult
    {
        /// <summary>
        /// 通过测试
        /// </summary>
        Accepted,

        /// <summary>
        /// 答案错误
        /// </summary>
        WrongAnswer,

        /// <summary>
        /// 运行超时
        /// </summary>
        TimeLimitExceeded,

        /// <summary>
        /// 内存超过限制
        /// </summary>
        MemoryLimitExceeded,

        /// <summary>
        /// 输出超过限制
        /// </summary>
        OutputLimitExceeded,

        /// <summary>
        /// 运行时错误
        /// </summary>
        RuntimeError,

        /// <summary>
        /// 编译错误
        /// </summary>
        CompileError,

        /// <summary>
        /// 表达形式错误
        /// </summary>
        PresentationError,

        /// <summary>
        /// 等待评测
        /// </summary>
        Pending,

        /// <summary>
        /// 正在评测
        /// </summary>
        Running
    }
}
