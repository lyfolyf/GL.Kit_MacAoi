using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GL.Kit.Log
{
    public class GLogAdapter : ILogAdapter, IDisposable
    {
        /// <summary>
        /// 日志名称
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 日志所在的目录
        /// </summary>
        public string Directory { get; }

        /// <summary>
        /// 保存天数
        /// </summary>
        public int SaveDays { get; }

        readonly string extension;
        readonly LogFormat format;

        BlockingCollection<LogInfo> blocking;

        public GLogAdapter(string name, string directory, int saveDays, LogFormat format)
        {
            Name = name;
            Directory = directory;
            SaveDays = saveDays;

            this.format = format;

            if (format == LogFormat.CSV)
                extension = ".csv";
            else
                extension = ".log";
        }

        ~GLogAdapter()
        {
            Dispose(false);
        }

        public void AddLog(LogInfo log)
        {
            blocking.Add(log);
        }

        public void AddLog(LogLevel level, string message, Exception exception = null)
        {
            LogInfo log = new LogInfo(level, message, exception);
            blocking.Add(log);
        }

        public void AddLog(LogLevel level, string format, params object[] args)
        {
            LogInfo log = new LogInfo(level, string.Format(format, args));
            blocking.Add(log);
        }

        public void AddLog(LogLevel level, LogMessage message)
        {
            LogInfo log = new LogInfo(level, message.ToString(format));
            blocking.Add(log);
        }

        Task writeTask;
        Task delTask;
        CancellationTokenSource cts;

        StreamWriter sw;
        DateTime today = DateTime.Today;

        public void Start()
        {
            blocking = new BlockingCollection<LogInfo>();

            System.IO.Directory.CreateDirectory(Directory);

            sw = new StreamWriter(Path.Combine(Directory, $"{Name}_{DateTime.Today: yyyy-MM-dd}{extension}"), true, Encoding.Default);

            writeTask = new Task(WriteLog, TaskCreationOptions.LongRunning);
            writeTask.Start();

            if (SaveDays > 0)
            {
                cts = new CancellationTokenSource();
                delTask = new Task(() => DeleteLog(cts.Token), cts.Token, TaskCreationOptions.LongRunning);
                delTask.Start();
            }
        }

        public void Stop()
        {
            blocking?.CompleteAdding();
            cts?.Cancel();
        }

        void WriteLog()
        {
            while (!blocking.IsCompleted)
            {
                LogInfo logInfo;
                try
                {
                    logInfo = blocking.Take();
                }
                catch (InvalidOperationException)
                {
                    break;
                }

                if (logInfo.CreateTime.Date != today)
                {
                    sw.Flush();
                    sw.Close();

                    today = logInfo.CreateTime.Date;

                    try
                    {
                        sw = new StreamWriter(Path.Combine(Directory, $"{Name}_{today: yyyy-MM-dd}{extension}"), true, Encoding.Default);
                    }
                    catch (Exception)
                    {

                    }
                }

                sw.WriteLine(logInfo.ToString(format));
                sw.Flush();
            }

            sw.Flush();
            sw.Close();
        }

        void DeleteLog(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                DateTime now = DateTime.Today;

                DirectoryInfo d = System.IO.Directory.CreateDirectory(Directory);
                FileInfo[] files = d.GetFiles("*" + extension);

                foreach (FileInfo f in files)
                {
                    if ((now - f.CreationTime).TotalDays > SaveDays)
                    {
                        try
                        {
                            f.Delete();
                        }
                        catch (Exception)
                        {

                        }
                    }
                }

                Thread.Sleep(3600000);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            Stop();
        }
    }
}
