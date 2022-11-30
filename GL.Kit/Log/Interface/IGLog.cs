using System;

namespace GL.Kit.Log
{
    public interface IGLog
    {
        void Debug(object message);

        void DebugFormat(string format, params object[] args);


        void Info(object message);

        void InfoFormat(string format, params object[] args);


        void Warn(object message);

        void Warn(object message, Exception exception);

        void WarnFormat(string format, params object[] args);


        void Error(object message);

        void Error(object message, Exception exception);

        void ErrorFormat(string format, params object[] args);


        void Fatal(object message);

        void Fatal(object message, Exception exception);

        void FatalFormat(string format, params object[] args);
    }

    public interface IGLogger : IGLog
    {
        string Name { get; }

        LogLevel Level { get; set; }
    }
}
