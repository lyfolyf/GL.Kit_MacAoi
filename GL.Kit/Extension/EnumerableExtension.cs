using System.Collections.Generic;

namespace System.Linq
{
    public static class EnumerableExtension
    {
        public static bool Remove<T>(this List<T> list, Func<T, bool> predicate)
        {
            int index = list.FindIndex(t => predicate(t));
            if (index != -1)
            {
                list.RemoveAt(index);
                return true;
            }

            return false;
        }

        public static bool Contains<T>(this T[] array, T item)
        {
            if ((Object)item == null)
            {
                for (int i = 0; i < array.Length; i++)
                    if ((Object)array[i] == null)
                        return true;
                return false;
            }
            else
            {
                EqualityComparer<T> c = EqualityComparer<T>.Default;
                for (int i = 0; i < array.Length; i++)
                {
                    if (c.Equals(array[i], item)) return true;
                }
                return false;
            }
        }

        public static TValue GetValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key) where TValue : new()
        {
            if (dict.ContainsKey(key))
                return dict[key];
            else
            {
                TValue value = new TValue();
                dict[key] = value;
                return value;
            }
        }
    }
}
