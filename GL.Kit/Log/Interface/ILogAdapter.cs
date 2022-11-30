using System;

namespace GL.Kit.Log
{
    public interface ILogAdapter
    {
        void AddLog(LogInfo log);

        void AddLog(LogLevel level, string message, Exception exception = null);

        void AddLog(LogLevel level, string format, params object[] args);

        void AddLog(LogLevel level, LogMessage message);

        void Start();

        void Stop();
    }
}
