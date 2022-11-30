using System;
using System.Collections.Generic;
using System.Linq;

namespace GL.Kit.Log
{
    public class LogPublisher : ILogAppender, IGLog
    {
        readonly List<IGLogger> logs = new List<IGLogger>();

        //public static readonly LogPublisher Log = new LogPublisher();

        //private LogPublisher() { }

        public void AddLogger(IGLogger logger)
        {
            logs.Add(logger);
        }

        public IGLogger GetLogger(string name)
        {
            return logs.FirstOrDefault(a => a.Name == name);
        }

        public void RemoveLogger(IGLogger logger)
        {
            logs.Remove(logger);
        }

        public void Debug(object message)
        {
            foreach (var log in logs)
                log.Debug(message);
        }

        public void DebugFormat(string format, params object[] args)
        {
            foreach (var log in logs)
                log.DebugFormat(format, args);
        }

        public void Info(object message)
        {
            foreach (var log in logs)
                log.Info(message);
        }

        public void InfoFormat(string format, params object[] args)
        {
            foreach (var log in logs)
                log.InfoFormat(format, args);
        }

        public void Warn(object message)
        {
            foreach (var log in logs)
                log.Warn(message);
        }

        public void Warn(object message, Exception exception)
        {
            foreach (var log in logs)
                log.Warn(message, exception);
        }

        public void WarnFormat(string format, params object[] args)
        {
            foreach (var log in logs)
                log.WarnFormat(format, args);
        }

        public void Error(object message)
        {
            foreach (var log in logs)
                log.Error(message);
        }

        public void Error(object message, Exception exception)
        {
            foreach (var log in logs)
                log.Error(message, exception);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            foreach (var log in logs)
                log.ErrorFormat(format, args);
        }

        public void Fatal(object message)
        {
            foreach (var log in logs)
                log.Fatal(message);
        }

        public void Fatal(object message, Exception exception)
        {
            foreach (var log in logs)
                log.Fatal(message, exception);
        }

        public void FatalFormat(string format, params object[] args)
        {
            foreach (var log in logs)
                log.FatalFormat(format, args);
        }
    }
}
