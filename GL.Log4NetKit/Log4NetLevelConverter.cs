using GL.Kit.Log;
using log4net.Core;
using System.ComponentModel;

namespace GL.Log4NetKit
{
    public class Log4NetLevelConverter : ILogLevelConverter<Level>
    {
        public Level GetLevel(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Debug: return Level.Debug;
                case LogLevel.Info: return Level.Info;
                case LogLevel.Warn: return Level.Warn;
                case LogLevel.Error: return Level.Error;
                case LogLevel.Fatal: return Level.Fatal;
                default: throw new InvalidEnumArgumentException(nameof(level), (int)level, typeof(LogLevel));
            }
        }
    }
}
