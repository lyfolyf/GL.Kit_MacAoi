namespace System.Diagnostics
{
    public static class FuncWatch
    {
        public static TimeSpan ElapsedTime(Action action)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            action();

            watch.Stop();

            return watch.Elapsed;
        }

        public static TimeSpan ElapsedTime<T>(Action<T> action, T t1)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            action(t1);

            watch.Stop();

            return watch.Elapsed;
        }

        public static TimeSpan ElapsedTime<T1, T2>(Action<T1, T2> action, T1 t1, T2 t2)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            action(t1, t2);

            watch.Stop();

            return watch.Elapsed;
        }

        public static TimeSpan ElapsedTime<T1, T2, T3>(Action<T1, T2, T3> action, T1 t1, T2 t2, T3 t3)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            action(t1, t2, t3);

            watch.Stop();

            return watch.Elapsed;
        }

        public static TimeSpan ElapsedTime<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            action(t1, t2, t3, t4);

            watch.Stop();

            return watch.Elapsed;
        }

        public static (TimeSpan, TResult) ElapsedTime<TResult>(Func<TResult> func)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            TResult result = func();

            watch.Stop();

            return (watch.Elapsed, result);
        }

        public static (TimeSpan, TResult) ElapsedTime<T, TResult>(Func<T, TResult> func, T t1)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            TResult result = func(t1);

            watch.Stop();

            return (watch.Elapsed, result);
        }

        public static (TimeSpan, TResult) ElapsedTime<T1, T2, TResult>(Func<T1, T2, TResult> func, T1 t1, T2 t2)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            TResult result = func(t1, t2);

            watch.Stop();

            return (watch.Elapsed, result);
        }

        public static (TimeSpan, TResult) ElapsedTime<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func, T1 t1, T2 t2, T3 t3)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            TResult result = func(t1, t2, t3);

            watch.Stop();

            return (watch.Elapsed, result);
        }

        public static (TimeSpan, TResult) ElapsedTime<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> func, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            TResult result = func(t1, t2, t3, t4);

            watch.Stop();

            return (watch.Elapsed, result);
        }
    }
}
