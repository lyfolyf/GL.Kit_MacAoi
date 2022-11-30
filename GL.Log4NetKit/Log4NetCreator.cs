using GL.Kit.Log;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System.Text;

namespace GL.Log4NetKit
{
    public static class Log4NetCreator
    {
        public static IGLogger GetLogger(string name)
        {
            LogKit kit = new LogKit();
            ILog log = kit.GetRollingFileLogger(name);

            return new Log4NetLogger(log);
        }

        /// <summary>
        /// 如果 name 已存在，则后面设置的参数无效
        /// </summary>
        /// <param name="name"></param>
        /// <param name="extension">日志文件的扩展名
        /// <para>默认值：.log</para>
        /// </param>
        /// <param name="maximumFileSize">最大文件大小
        /// <para>默认值：100MB</para>
        /// </param>
        /// <param name="maxSizeRollBackups">最大日志文件数
        /// <para>默认值：10</para>
        /// </param>
        /// <param name="level">日志等级
        /// <para>默认值：Info</para></param>
        /// <param name="datePattern">在日期滚动时用于生成文件名的日期格式
        /// <para>默认值：_yyyy-MM-dd</para></param>
        /// <param name="conversionPattern">日志字符串输出格式
        /// <para>默认值：%date{yyyy-MM-dd HH:mm:ss.fff}  %-5thread%-6level%message%newline</para>
        /// </param>
        public static IGLogger GetLog(string name,
            string extension = LogKit.DefaultExtension,
            string maximumFileSize = LogKit.DefaultMaximumFileSize,
            int maxSizeRollBackups = LogKit.DefaultMaxSizeRollBackups,
            LogLevel level = LogLevel.Info,
            string datePattern = LogKit.DatePattern,
            string conversionPattern = LogKit.DefaultConversionPattern)
        {
            Log4NetLevelConverter levelAdapter = new Log4NetLevelConverter();
            Level log4netLevel = levelAdapter.GetLevel(level);

            LogKit kit = new LogKit(extension, maximumFileSize, maxSizeRollBackups, log4netLevel, datePattern, conversionPattern);
            ILog log = kit.GetRollingFileLogger(name);

            return new Log4NetLogger(log);
        }

        class LogKit
        {
            internal const string DefaultExtension = ".log";
            internal const string DefaultMaximumFileSize = "100MB";
            internal const int DefaultMaxSizeRollBackups = 10;
            readonly static Level DefaultLevel = Level.Info;
            internal const string DefaultConversionPattern = "%date{yyyy-MM-dd HH:mm:ss.fff}  %-5thread%-6level%message%newline";
            internal const string DatePattern = "_yyyy-MM-dd";

            readonly string m_extension;
            readonly string m_maximumFileSize;
            readonly int m_maxSizeRollBackups;
            readonly Level m_level;
            readonly string m_datePattern;
            readonly string m_conversionPattern;

            public LogKit()
                : this(DefaultExtension, DefaultMaximumFileSize, DefaultMaxSizeRollBackups, DefaultLevel, DatePattern, DefaultConversionPattern)
            {

            }

            public LogKit(string extension, string maximumFileSize, int maxSizeRollBackups, Level level, string datePattern, string conversionPattern)
            {
                m_extension = extension;
                m_maximumFileSize = maximumFileSize;
                m_maxSizeRollBackups = maxSizeRollBackups;
                m_level = level;
                m_datePattern = datePattern;
                m_conversionPattern = conversionPattern;
            }

            public ILog GetRollingFileLogger(string name)
            {
                ILog log = LogManager.Exists(name);
                if (log != null) return log;

                PatternLayout layout = CreatePatternLayout(m_conversionPattern);

                RollingFileAppender appender = CreateRollingFileAppender(name, m_maximumFileSize, m_maxSizeRollBackups, m_datePattern, layout);

                Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();

                Logger logger = hierarchy.GetLogger(name, hierarchy.LoggerFactory); //!!! 此处写法是重点，不容更改
                logger.AddAppender(appender);
                logger.Level = m_level;
                logger.Parent = null;   // 如果不加，执行 BasicConfigurator.Configure() 的时候会自动给 Parent 加一个 ConsoleAppender

                BasicConfigurator.Configure();//!!! 此处写法是重点，不容更改

                log = LogManager.GetLogger(name);
                return log;
            }

            private RollingFileAppender CreateRollingFileAppender(string name, string maximumFileSize, int maxSizeRollBackups, string datePattern, ILayout layout)
            {
                RollingFileAppender appender = new RollingFileAppender();
                appender.Name = $"{name}Appender";
                appender.AppendToFile = true;
                appender.DatePattern = datePattern;
                appender.Encoding = Encoding.UTF8;
                appender.File = $"Log\\{name}{m_extension}";
                appender.LockingModel = new FileAppender.MinimalLock();
                appender.MaximumFileSize = maximumFileSize;
                appender.MaxSizeRollBackups = maxSizeRollBackups;
                appender.PreserveLogFileNameExtension = true;
                appender.RollingStyle = RollingFileAppender.RollingMode.Composite;
                appender.StaticLogFileName = false;
                appender.Layout = layout;

                appender.ActivateOptions();

                return appender;
            }

            private PatternLayout CreatePatternLayout(string conversionPattern)
            {
                return new PatternLayout(conversionPattern);
            }
        }
    }
}
