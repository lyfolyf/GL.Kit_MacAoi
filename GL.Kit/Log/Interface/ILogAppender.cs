namespace GL.Kit.Log
{
    public interface ILogAppender
    {
        void AddLogger(IGLogger log);

        void RemoveLogger(IGLogger log);
    }
}
