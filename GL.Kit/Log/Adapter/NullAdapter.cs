using System;

namespace GL.Kit.Log.Adapter
{
    public class NullAdapter : ILogAdapter
    {
        public void AddLog(LogInfo log)
        {

        }

        public void AddLog(LogLevel level, string message, Exception exception = null)
        {

        }

        public void AddLog(LogLevel level, string format, params object[] args)
        {

        }

        public void AddLog(LogLevel level, LogMessage message)
        {

        }

        public void Start()
        {

        }

        public void Stop()
        {

        }
    }
}
