namespace System.Diagnostics
{
    // 实测这个写法是可用的，Stopwatch 貌似并不消耗多少资源，暂时也就没有用它
    internal static class StopwatchCache
    {
        [ThreadStatic]
        private static Stopwatch CachedInstance;

        public static Stopwatch Acquire()
        {
            if (CachedInstance == null)
            {
                CachedInstance = new Stopwatch();
            }
            else
            {
                CachedInstance.Reset();
            }

            return CachedInstance;
        }
    }
}
