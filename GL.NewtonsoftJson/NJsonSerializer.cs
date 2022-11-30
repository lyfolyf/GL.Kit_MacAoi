using GL.Kit.Serialization;
using Newtonsoft.Json;
using System.IO;

namespace GL.NewtonsoftJson
{
    public sealed class NJsonSerializer : ISerializer, ISerializerToFile
    {
        public string Serialize<T>(T source)
        {
            return JsonConvert.SerializeObject(source, Formatting.Indented);
        }

        public T Deserialize<T>(string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }

        public void SerializeToFile<T>(T source, string filename)
        {
            string json = Serialize(source);

            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.Write(json);
                sw.Flush();
            }
        }

        public T DeserializeFromFile<T>(string filename)
        {
            using (StreamReader sr = new StreamReader(filename))
            {
                string json = sr.ReadToEnd();

                return Deserialize<T>(json);
            }
        }

    }
}
