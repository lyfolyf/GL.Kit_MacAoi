namespace System.Collections.Generic
{
    /// <summary>
    /// 
    /// </summary>
    public class LinkedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IEnumerable<KeyValuePair<TKey, TValue>>
    {
        readonly Dictionary<TKey, TValue> m_dict;

        class Node
        {
            public TKey Key;

            public TValue Value;

            public Node Next;
        }

        Node head;
        Node last;

        public LinkedDictionary()
        {
            m_dict = new Dictionary<TKey, TValue>();
            head = null;
            last = null;
        }

        public LinkedDictionary(int capacity)
        {
            m_dict = new Dictionary<TKey, TValue>(capacity);
            head = null;
            last = null;
        }

        public TValue this[TKey key]
        {
            get { return m_dict[key]; }
            set { m_dict[key] = value; }
        }

        public bool TryGetValue(TKey key, out TValue value) => m_dict.TryGetValue(key, out value);

        public void Add(TKey key, TValue value)
        {
            if (m_dict.ContainsKey(key))
                throw new ArgumentException($"{nameof(LinkedDictionary<TKey, TValue>)} 中已存在具有相同键的元素");

            m_dict.Add(key, value);
            AddLinded(key, value);
        }

        void AddLinded(TKey key, TValue value)
        {
            Node newNode = new Node
            {
                Key = key,
                Value = value
            };

            if (head == null)
            {
                head = newNode;
                last = newNode;
            }
            else
            {
                last.Next = newNode;
                last = newNode;
            }
        }

        public bool Remove(TKey key)
        {
            if (m_dict.ContainsKey(key))
            {
                m_dict.Remove(key);
                RemoveLinded(key);

                return true;
            }

            return false;
        }

        void RemoveLinded(TKey key)
        {
            if (head == null) return;

            Node node = head;
            Node prev = null;
            EqualityComparer<TKey> c = EqualityComparer<TKey>.Default;
            do
            {
                if (c.Equals(node.Key, key))
                {
                    if (prev == null)
                    {
                        // 表示删除的是 head
                        head = head.Next;
                    }
                    else
                    {
                        prev.Next = node.Next;
                        if (prev.Next == null)
                            last = prev;
                    }
                    return;
                }

                prev = node;
                node = node.Next;

            } while (node != null);
        }

        public bool ContainsKey(TKey key) => m_dict.ContainsKey(key);

        public bool ContainsValue(TValue value) => m_dict.ContainsValue(value);

        public int Count
        {
            get { return m_dict.Count; }
        }

        public IEnumerable<TKey> Keys
        {
            get
            {
                Node node = head;
                while (node != null)
                {
                    yield return node.Key;

                    node = node.Next;
                }
            }
        }

        public IEnumerable<TValue> Values
        {
            get
            {
                Node node = head;
                while (node != null)
                {
                    yield return node.Value;

                    node = node.Next;
                }
            }
        }

        ICollection<TKey> IDictionary<TKey, TValue>.Keys => m_dict.Keys;

        ICollection<TValue> IDictionary<TKey, TValue>.Values => m_dict.Values;

        public bool IsReadOnly => false;

        public void Clear()
        {
            m_dict.Clear();
            head = null;
            last = null;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            Node node = head;
            while (node != null)
            {
                yield return new KeyValuePair<TKey, TValue>(node.Key, node.Value);

                node = node.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> keyValuePair)
        {
            Add(keyValuePair.Key, keyValuePair.Value);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> keyValuePair)
        {
            if (m_dict.ContainsKey(keyValuePair.Key))
            {
                if (EqualityComparer<TValue>.Default.Equals(keyValuePair.Value, m_dict[keyValuePair.Key]))
                {
                    return true;
                }
            }

            return false;
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            if (m_dict.ContainsKey(item.Key))
            {
                if (EqualityComparer<TValue>.Default.Equals(item.Value, m_dict[item.Key]))
                {
                    m_dict.Remove(item.Key);
                    RemoveLinded(item.Key);
                    return true;
                }
            }

            return false;
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (index < 0 || index > array.Length)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (array.Length - index < Count)
                throw new ArgumentException("目标数组的长度不够。请检查 destIndex 和长度以及数组的下限。");

            Node node = head;
            while (node != null)
            {
                array[index++] = new KeyValuePair<TKey, TValue>(node.Key, node.Value);

                node = node.Next;
            }
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return m_dict.GetEnumerator();
        }
    }
}

