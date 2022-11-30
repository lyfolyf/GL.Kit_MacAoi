using GL.Kit.Log;
using log4net;
using System;

namespace GL.Log4NetKit
{
    /// <summary>
    /// log4net 适配器
    /// </summary>
    public class Log4NetLogger : IGLogger
    {
        readonly ILog log;

        public string Name { get; }

        public LogLevel Level { get; set; }

        public Log4NetLogger(ILog log)
        {
            this.log = log;

            Name = log.Logger.Name;
        }

        public Log4NetLogger(string logname)
        {
            Name = logname;
            log = LogManager.GetLogger(logname);
        }

        #region

        public void Debug(object message) => log.Debug(message);

        public void DebugFormat(string format, params object[] args) => log.DebugFormat(format, args);

        public void Info(object message) => log.Info(message);

        public void InfoFormat(string format, params object[] args) => log.InfoFormat(format, args);

        public void Warn(object message) => log.Warn(message);

        public void Warn(object message, Exception exception) => log.Warn(message, exception);

        public void WarnFormat(string format, params object[] args) => log.WarnFormat(format, args);

        public void Error(object message) => log.Error(message);

        public void Error(object message, Exception exception) => log.Error(message, exception);

        public void ErrorFormat(string format, params object[] args) => log.ErrorFormat(format, args);

        public void Fatal(object message) => log.Fatal(message);

        public void Fatal(object message, Exception exception) => log.Fatal(message, exception);

        public void FatalFormat(string format, params object[] args) => log.FatalFormat(format, args);

        #endregion
    }
}
