using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace System.Net.Http
{
    public class QueryParameter
    {
        private object _value;
        private string _encodeValue;

        public QueryParameter(string name, object value)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException();

            Name = name;
            Value = value;
        }

        public string Name { get; set; }

        public object Value
        {
            get => _value;
            set
            {
                _value = value;

                if (!(value is string) && value is IEnumerable en)
                {
                    _encodeValue = Uri.EscapeDataString(string.Join(",", en.Cast<object>()));
                }
                else if (value is null)
                {
                    _encodeValue = string.Empty;
                }
                else
                {
                    _encodeValue = Uri.EscapeDataString(value.ToString());
                }
            }
        }

        public override string ToString()
        {
            return $"{Name}={Value}";
        }

        public string ToEncodeString()
        {
            return $"{Name}={_encodeValue}";
        }

    }

    public class QueryParamCollection : IEnumerable<QueryParameter>
    {
        List<QueryParameter> _queries = new List<QueryParameter>();

        // 未对同名参数做任何的处理，默认参数是不重名的

        internal QueryParamCollection() { }

        internal QueryParamCollection(IDictionary<string, object> queries)
        {
            _addRange(queries);
        }

        public void Add(string name, object value)
        {
            if (Contains(name))
                throw new Exception($"查询参数 {{{name}}} 已存在。");

            _queries.Add(new QueryParameter(name, value));
        }

        public void AddRange(IDictionary<string, object> queries)
        {
            _queries.ForEach((q) =>
            {
                if (queries.ContainsKey(q.Name))
                {
                    throw new Exception($"查询参数 {{{q.Name}}} 已存在。");
                }
            });

            _addRange(queries);
        }

        private void _addRange(IDictionary<string, object> queries)
        {
            foreach (var query in queries)
            {
                _queries.Add(new QueryParameter(query.Key, query.Value));
            }
        }

        public bool Contains(string name)
        {
            return _queries.Any(p => p.Name == name);
        }

        public int Remove(string name)
        {
            return _queries.RemoveAll(p => p.Name == name);
        }

        public object this[string name]
        {
            get
            {
                int index = _queries.FindIndex(m => m.Name == name);
                if (index == -1)
                    return null;
                else
                    return _queries[index].Value;
            }
            set
            {
                int index = _queries.FindIndex(m => m.Name == name);
                if (index == -1)
                    _queries.Add(new QueryParameter(name, value));
                else
                    _queries[index].Value = value;
            }
        }

        public int Count
        {
            get { return _queries.Count; }
        }

        public override string ToString()
        {
            return string.Join("&", _queries.Select(p => p.ToString()));
        }

        public string ToEncodeString()
        {
            return string.Join("&", _queries.Select(p => p.ToEncodeString()));
        }

        public IEnumerator<QueryParameter> GetEnumerator()
        {
            return ((IEnumerable<QueryParameter>)_queries).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_queries).GetEnumerator();
        }
    }

}
