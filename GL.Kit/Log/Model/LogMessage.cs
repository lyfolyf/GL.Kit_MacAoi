namespace GL.Kit.Log
{
    public abstract class LogMessage
    {
        /// <summary>
        /// 模块
        /// </summary>
        public virtual string Module { get; }

        /// <summary>
        /// 操作
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// 成功/失败
        /// </summary>
        public string ActionResult { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        public override string ToString()
        {
            return ToString(LogFormat.CSV);
        }

        public abstract string ToString(LogFormat format);
    }

    public static class ActionResult
    {
        public const string R_Success = "成功";

        public const string R_Fail = "失败";

        public const string R_Clear = "清空";

        public const string R_Start = "开始";

        public const string R_Stop = "停止";

        public const string R_End = "结束";

        public const string R_Close = "关闭";

        public const string R_Disconnect = "断开";

        public const string R_Tip = "提示";
    }
}
