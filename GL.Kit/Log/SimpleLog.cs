using System;
using System.IO;

namespace GL.Kit.Log
{
    /// <summary>
    /// 简易日志书写类
    /// </summary>
    public class SimpleLog
    {
        public static string LogPath = Path.Combine(PathUtils.CurrentDirectory, "SimpleLog.log");

        public static void Debug(string path, string usermsg)
        {
            Write(path, LogLevel.Debug, usermsg, null);
        }

        public static void Info(string path, string usermsg)
        {
            Write(path, LogLevel.Info, usermsg, null);
        }

        public static void Warn(string path, string usermsg, Exception e = null)
        {
            Write(path, LogLevel.Warn, usermsg, e);
        }

        public static void Error(string path, string usermsg, Exception e = null)
        {
            Write(path, LogLevel.Error, usermsg, e);
        }

        public static void Debug(string usermsg)
        {
            Write(LogPath, LogLevel.Debug, usermsg, null);
        }

        public static void Info(string usermsg)
        {
            Write(LogPath, LogLevel.Info, usermsg, null);
        }

        public static void Warn(string usermsg, Exception e = null)
        {
            Write(LogPath, LogLevel.Warn, usermsg, e);
        }

        public static void Error(string usermsg, Exception e = null)
        {
            Write(LogPath, LogLevel.Error, usermsg, e);
        }

        public static void Write(string path, LogLevel level, string usermsg, Exception e)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(path, true))
                {
                    sw.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}\t{level,-5} - {usermsg}");
                    if (e != null)
                    {
                        sw.WriteLine(e.ToString());
                        sw.WriteLine();
                    }
                    sw.Flush();
                }
            }
            catch
            {
            }
        }
    }
}
