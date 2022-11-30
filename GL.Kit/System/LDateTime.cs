namespace System
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// 今年元旦
        /// </summary>
        public static DateTime ThisYear
        {
            get => new DateTime(DateTime.Today.Year, 1, 1);
        }

        /// <summary>
        /// 去年元旦
        /// </summary>
        public static DateTime LastYear
        {
            get => new DateTime(DateTime.Today.Year - 1, 1, 1);
        }

        /// <summary>
        /// 明年元旦
        /// </summary>
        public static DateTime NextYear
        {
            get => new DateTime(DateTime.Today.Year + 1, 1, 1);
        }

        /// <summary>
        /// 本月 1 号
        /// </summary>
        public static DateTime ThisMonth
        {
            get => new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        }

        /// <summary>
        /// 上个月 1 号
        /// </summary>
        public static DateTime LastMonth
        {
            get => ThisMonth.AddMonths(-1);
        }

        /// <summary>
        /// 下个月 1 号
        /// </summary>
        public static DateTime NextMonth
        {
            get => ThisMonth.AddMonths(1);
        }

        /// <summary>
        /// 昨天
        /// </summary>
        public static DateTime Yesterday
        {
            get => DateTime.Today.AddDays(-1);
        }

        /// <summary>
        /// 明天
        /// </summary>
        public static DateTime Tomorrow
        {
            get => DateTime.Today.AddDays(1);
        }
    }
}
