using System;

namespace GL.Kit.Log
{
    // 在 log4net 中，添加一个 LogAdapter，就是添加一种日志输出
    // 但是这里，我以 GLog 为单位，一个 GLog 就是一个日志输出
    // LogAdapter 我仅仅是作为一种写日志的方式，它必须依附于 GLog 而存在
    // 这样做是因为，如果要将 log4net 添加进来，继承 LogAdapter 是无法实现的，只能继承 GLog

    public class GLogger : IGLogger
    {
        readonly ILogAdapter log;

        private GLogger(ILogAdapter log)
        {
            this.log = log;
        }

        public static IGLogger CreateLog(string name, LogLevel level, ILogAdapter logAdapter)
        {
            GLogger gLog = new GLogger(logAdapter);
            gLog.Name = name;
            gLog.Level = level;
            logAdapter.Start();

            return gLog;
        }

        public string Name { get; set; }

        public LogLevel Level { get; set; }

        public void Debug(object message)
        {
            if (Level <= LogLevel.Debug)
            {
                if (message is LogMessage msg)
                    log.AddLog(LogLevel.Debug, msg);
                else
                    log.AddLog(LogLevel.Debug, message.ToString());
            }
        }

        public void DebugFormat(string format, params object[] args)
        {
            if (Level <= LogLevel.Debug)
            {
                log.AddLog(LogLevel.Debug, format, args);
            }
        }

        public void Info(object message)
        {
            if (Level <= LogLevel.Info)
            {
                if (message is LogMessage msg)
                    log.AddLog(LogLevel.Info, msg);
                else
                    log.AddLog(LogLevel.Info, message.ToString());
            }
        }

        public void InfoFormat(string format, params object[] args)
        {
            if (Level <= LogLevel.Info)
            {
                log.AddLog(LogLevel.Info, format, args);
            }
        }

        public void Warn(object message)
        {
            if (Level <= LogLevel.Warn)
            {
                if (message is LogMessage msg)
                    log.AddLog(LogLevel.Warn, msg);
                else
                    log.AddLog(LogLevel.Warn, message.ToString());
            }
        }

        public void Warn(object message, Exception exception)
        {
            if (Level <= LogLevel.Warn)
            {
                log.AddLog(LogLevel.Warn, message.ToString(), exception);
            }
        }

        public void WarnFormat(string format, params object[] args)
        {
            if (Level <= LogLevel.Warn)
            {
                log.AddLog(LogLevel.Warn, format, args);
            }
        }

        public void Error(object message)
        {
            if (Level <= LogLevel.Error)
            {
                if (message is LogMessage msg)
                    log.AddLog(LogLevel.Error, msg);
                else
                    log.AddLog(LogLevel.Error, message.ToString());
            }
        }

        public void Error(object message, Exception exception)
        {
            if (Level <= LogLevel.Error)
            {
                log.AddLog(LogLevel.Error, message.ToString(), exception);
            }
        }

        public void ErrorFormat(string format, params object[] args)
        {
            if (Level <= LogLevel.Error)
            {
                log.AddLog(LogLevel.Error, format, args);
            }
        }

        public void Fatal(object message)
        {
            if (Level <= LogLevel.Fatal)
            {
                if (message is LogMessage msg)
                    log.AddLog(LogLevel.Fatal, msg);
                else
                    log.AddLog(LogLevel.Fatal, message.ToString());
            }
        }

        public void Fatal(object message, Exception exception)
        {
            if (Level <= LogLevel.Fatal)
            {
                log.AddLog(LogLevel.Fatal, message.ToString(), exception);
            }
        }

        public void FatalFormat(string format, params object[] args)
        {
            if (Level <= LogLevel.Fatal)
            {
                log.AddLog(LogLevel.Fatal, format, args);
            }
        }

    }
}
