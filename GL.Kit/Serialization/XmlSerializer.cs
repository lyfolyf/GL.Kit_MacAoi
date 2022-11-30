using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace GL.Kit.Serialization
{
    /// <summary>
    /// XML 序列化
    /// </summary>
    public class XmlSerializer : ISerializer, ISerializerToFile
    {
        /// <summary>
        /// 是否缩进
        /// </summary>
        public bool Indent { get; }

        readonly XmlWriterSettings settings;
        readonly XmlSerializerNamespaces ns;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="indent">是否缩进</param>
        public XmlSerializer(bool indent = true)
        {
            Indent = indent;

            settings = new XmlWriterSettings
            {
                Indent = indent,

                //去掉XML声明 <?xml version="1.0" encoding="utf-16"?> 
                OmitXmlDeclaration = true
            };

            ns = new XmlSerializerNamespaces();
            ns.Add("", "");
        }

        public string Serialize<T>(T source)
        {
            StringBuilder sb = new StringBuilder();

            using (XmlWriter xw = XmlWriter.Create(sb, settings))
            {
                System.Xml.Serialization.XmlSerializer xmls = new System.Xml.Serialization.XmlSerializer(typeof(T));
                xmls.Serialize(xw, source, ns);
            }

            return sb.ToString();
        }

        public string Serialize(object source)
        {
            StringBuilder sb = new StringBuilder();

            using (XmlWriter xw = XmlWriter.Create(sb, settings))
            {
                System.Xml.Serialization.XmlSerializer xmls = new System.Xml.Serialization.XmlSerializer(source.GetType());
                xmls.Serialize(xw, source, ns);
            }

            return sb.ToString();
        }

        public T Deserialize<T>(string str)
        {
            using (XmlReader xr = XmlReader.Create(new StringReader(str)))
            {
                System.Xml.Serialization.XmlSerializer xmls = new System.Xml.Serialization.XmlSerializer(typeof(T));
                T obj = (T)xmls.Deserialize(xr);
                return obj;
            }
        }

        public object Deserialize(string str, Type type)
        {
            using (XmlReader xr = XmlReader.Create(new StringReader(str)))
            {
                System.Xml.Serialization.XmlSerializer xmls = new System.Xml.Serialization.XmlSerializer(type);
                return xmls.Deserialize(xr);
            }
        }


        public void SerializeToFile<T>(T source, string filename)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filename));

            using (XmlWriter xw = XmlWriter.Create(filename, settings))
            {
                System.Xml.Serialization.XmlSerializer xmls = new System.Xml.Serialization.XmlSerializer(typeof(T));
                xmls.Serialize(xw, source, ns);

                xw.Flush();
            }
        }

        public T DeserializeFromFile<T>(string filename)
        {
            if (!File.Exists(filename))
                return default;

            using (XmlReader xr = XmlReader.Create(filename))
            {
                System.Xml.Serialization.XmlSerializer xmls = new System.Xml.Serialization.XmlSerializer(typeof(T));
                T obj = (T)xmls.Deserialize(xr);
                return obj;
            }
        }
    }
}
