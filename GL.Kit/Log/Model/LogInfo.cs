using System;
using System.Threading;

namespace GL.Kit.Log
{
    public struct LogInfo
    {
        public LogInfo(LogLevel level, string message, Exception exception = null)
        {
            CreateTime = DateTime.Now;
            ThreadID = Thread.CurrentThread.ManagedThreadId;

            Level = level;
            Message = message;
            Exception = exception;
        }

        public DateTime CreateTime { get; }

        public int ThreadID { get; }

        public LogLevel Level { get; set; }

        public string Message { get; set; }

        public Exception Exception { get; set; }

        public string ToString(LogFormat format)
        {
            if (format == LogFormat.CSV)
                return CsvString();
            else
                return GeneralString();
        }

        string CsvString()
        {
            return $"\t{CreateTime:yyyy-MM-dd HH:mm:ss.fff},{Level},{ThreadID},{Message},{(Exception != null ? $"\"{Exception}\"" : string.Empty)}";
        }

        string GeneralString()
        {
            return $"{CreateTime:yyyy-MM-dd HH:mm:ss.fff}    {Level,-6}    {ThreadID,-3}    {Message}{(Exception != null ? "\r\n" + Exception.ToString() : string.Empty)}";
        }
    }
}
