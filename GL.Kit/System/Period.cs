namespace System
{
    /// <summary>
    /// 时间段
    /// </summary>
    public struct PeriodDataTime
    {
        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }
    }

    public struct PeriodTime
    {
        public TimeSpan BeginTime { get; set; }

        public TimeSpan EndTime { get; set; }
    }
}
