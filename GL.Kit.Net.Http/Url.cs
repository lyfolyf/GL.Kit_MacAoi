using System.Collections.Generic;
using System.Linq;

namespace System.Net.Http
{
    public class Url
    {
        string host = string.Empty;
        string route;

        /// <summary>
        /// HttpClient 中可以指定 BaseAddress，这样 Host 属性就没用了
        /// </summary>
        public string Host
        {
            get { return host; }
            set { host = value.TrimEnd('/'); }
        }

        public string Route
        {
            get { return route; }
            set
            {
                route = value.StartsWith("/") ? value : "/" + value;
            }
        }

        public QueryParamCollection Queries { get; set; }

        public Url()
        {
            Queries = new QueryParamCollection();
        }

        public Url(string route) : this()
        {
            Route = route;
        }

        public Url(string host, string route) : this()
        {
            Host = host;
            Route = route;
        }

        public void AddQuery(string name, object value)
        {
            Queries.Add(name, value);
        }

        public void AddQueries(IDictionary<string, object> queries)
        {
            Queries.AddRange(queries);
        }

        public string URL
        {
            get
            {
                if (Queries == null || Queries.Count == 0)
                    return $"{Host}{Route}";

                return $"{Host}{Route}?{Queries.ToEncodeString()}";
            }
        }

        public override string ToString()
        {
            return URL;
        }

        public static Url Parse(string str)
        {
            string[] infos = str.Split('?');

            (string host, string route) = GetHostRoute(infos[0]);

            Url url = new Url(host, route);

            if (infos.Length == 2 && infos[1].Length > 0)
            {
                IDictionary<string, object> queries = GetQueries(infos[1]);

                url.AddQueries(queries);
            }

            return url;
        }

        private static (string host, string route) GetHostRoute(string str)
        {
            int index = str.IndexOf("/", 8);
            if (index == -1)
                return (str, "");

            string host = str.Substring(0, index);
            string route = str.Substring(index);

            return (host, route);
        }

        private static IDictionary<string, object> GetQueries(string str)
        {
            return str.Split('&').Select(a => a.Split('=')).ToDictionary(key => key[0], value => (object)value[1]);
        }
    }
}
