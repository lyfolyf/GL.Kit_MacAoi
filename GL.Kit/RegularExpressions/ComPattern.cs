namespace System.Text.RegularExpressions
{
    /// <summary>
    /// 通用正则表达式
    /// </summary>
    public static class ComPattern
    {
        /// <summary>
        /// 匹配 IP4
        /// </summary>
        public static readonly string IpPattern = @"^((25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)\.){3}(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)$";

        /// <summary>
        /// 匹配名称（只能包含字母、数字、下划线，且不能以数字开头）
        /// </summary>
        public static readonly string NamePattern = @"^[a-zA-Z_][a-zA-Z0-9_]*$";

        /// <summary>
        /// 匹配非法路径字符
        /// </summary>
        public static readonly string IllegalPathChar = @"[/\\:*?""<>|]+";

        public static readonly string LegalPathChar = @"[^/\\:*?""<>|]+";
    }
}
