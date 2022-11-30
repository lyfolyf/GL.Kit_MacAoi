namespace GL.Kit.Log
{
    /// <summary>
    /// 日志等级转换器
    /// </summary>
    /// <typeparam name="TLevelEnum"></typeparam>
    public interface ILogLevelConverter<TLevelEnum>
    {
        TLevelEnum GetLevel(LogLevel level);
    }
}
