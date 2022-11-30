using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GL.Kit.Log
{
    public class DisplayAdapter : ILogAdapter
    {
        /// <summary>
        /// 清空当前显示日志，日志缓存满时发生
        /// </summary>
        public event Action ClearCurrent;

        /// <summary>
        /// 有新日志时发生
        /// </summary>
        public event Action<LogInfo> NewLog;

        // 当前显示的日志等级
        LogLevel m_showLevel = LogLevel.All;
        // 日志最多显示条数
        readonly int m_maxCount;

        readonly LogFormat m_format;

        // ConCurrentQueue 不提供清空方法
        // 所以清空的时候必须要加锁，那么添加也要加锁
        // 既然必须要加锁，那就用 List 了
        readonly Dictionary<LogLevel, List<LogInfo>> logCache;
        readonly object async = new object();

        Task logTask;

        BlockingCollection<LogInfo> blocking;

        public DisplayAdapter(int maxCount, LogFormat format)
        {
            m_maxCount = maxCount;
            m_format = format;

            logCache = new Dictionary<LogLevel, List<LogInfo>>()
            {
                { LogLevel.All,   new List<LogInfo>(m_maxCount) },
                { LogLevel.Debug, new List<LogInfo>(m_maxCount) },
                { LogLevel.Info,  new List<LogInfo>(m_maxCount) },
                { LogLevel.Warn,  new List<LogInfo>(m_maxCount) },
                { LogLevel.Error, new List<LogInfo>(m_maxCount) },
                { LogLevel.Fatal, new List<LogInfo>(m_maxCount) },
            };
        }

        public void Start()
        {
            blocking = new BlockingCollection<LogInfo>();

            logTask = new Task(NewLogMethod, TaskCreationOptions.LongRunning);
            logTask.Start();
        }

        public void Stop()
        {
            blocking?.CompleteAdding();
        }

        public void Clear(LogLevel level)
        {
            lock (async)
            {
                if (level == LogLevel.All)
                {
                    foreach (List<LogInfo> logs in logCache.Values)
                        logs.Clear();
                }
                else
                {
                    logCache[level].Clear();
                    logCache[LogLevel.All].RemoveAll(a => a.Level == level);
                }
            }

            ClearCurrent?.Invoke();
        }

        public void LevelChanged(LogLevel level)
        {
            ClearCurrent?.Invoke();

            m_showLevel = level;

            lock (async)
            {
                ClearBlocking();

                foreach (LogInfo log in logCache[level])
                {
                    blocking.Add(log);
                }
            }
        }

        void ClearBlocking()
        {
            while (blocking.TryTake(out _))
            {

            }
        }

        void NewLogMethod()
        {
            while (!blocking.IsCompleted)
            {
                LogInfo logInfo;
                try
                {
                    logInfo = blocking.Take();
                    NewLog?.Invoke(logInfo);
                }
                catch (InvalidOperationException)
                {
                    break;
                }
            }
        }

        public void AddLog(LogInfo log)
        {
            AddToCache(log);
        }

        public void AddLog(LogLevel level, string message, Exception exception = null)
        {
            LogInfo log = new LogInfo(level, message, exception);
            AddToCache(log);
        }

        public void AddLog(LogLevel level, string format, params object[] args)
        {
            LogInfo log = new LogInfo(level, string.Format(format, args));
            AddToCache(log);
        }

        public void AddLog(LogLevel level, LogMessage message)
        {
            LogInfo log = new LogInfo(level, message.ToString(m_format));
            AddToCache(log);
        }

        void AddToCache(LogInfo log)
        {
            lock (async)
            {
                AddToCache(log, log.Level);
                AddToCache(log, LogLevel.All);

                AddToBlocking(log);
            }
        }

        void AddToCache(LogInfo log, LogLevel level)
        {
            // 这里是有锁的
            List<LogInfo> logs = logCache[level];

            if (logs.Count == m_maxCount)
            {
                logs.Clear();

                if (level == m_showLevel)
                {
                    ClearCurrent?.Invoke();
                }
            }

            logs.Add(log);
        }

        void AddToBlocking(LogInfo log)
        {
            if (m_showLevel == LogLevel.All || m_showLevel == log.Level)
                blocking.Add(log);
        }
    }
}
