using System.Collections.Concurrent;
using System.Collections.Generic;

namespace System.Linq
{
    public static class GEnumerable
    {
        public static int IndexOf<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            int index = 0;

            foreach (T item in source)
            {
                if (predicate(item))
                {
                    return index;
                }
                index++;
            }
            return -1;
        }

        /// <summary>
        /// 两个元素互换
        /// </summary>
        public static void Exchange<T>(this IList<T> list, int index1, int index2)
        {
            if (list == null) throw new ArgumentNullException();

            if (index1 == index2) return;

            T temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
        }

        public static void Clear<T>(this ConcurrentQueue<T> queue)
        {
            while (!queue.IsEmpty)
            {
                queue.TryDequeue(out _);
            }
        }
    }
}
