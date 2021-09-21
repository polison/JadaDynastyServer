using System;

namespace JadeDynastyServer
{
    /// <summary>
    /// 日志
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// 打印消息
        /// </summary>
        void Message(string format, params object[] args);

        /// <summary>
        /// 打印错误
        /// </summary>
        void Error(string format, params object[] args);

        /// <summary>
        /// 打印警告
        /// </summary>
        void Warning(string format, params object[] args);
    }
}