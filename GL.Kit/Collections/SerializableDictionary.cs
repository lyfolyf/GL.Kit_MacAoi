using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Collections.Generic
{
    /// <summary>
    /// 可序列化的Dictionary
    /// </summary>
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
    {
        Type KeyType = typeof(TKey);
        Type ValueType = typeof(TValue);

        public SerializableDictionary() { }

        public void WriteXml(XmlWriter write)
        {
            XmlSerializer KeySerializer = new XmlSerializer(KeyType);
            XmlSerializer ValueSerializer = new XmlSerializer(ValueType);

            foreach (KeyValuePair<TKey, TValue> kv in this)
            {
                write.WriteStartElement("Item");
                write.WriteStartElement("Key");
                KeySerializer.Serialize(write, kv.Key);
                write.WriteEndElement();
                write.WriteStartElement("Value");
                ValueSerializer.Serialize(write, kv.Value);
                write.WriteEndElement();
                write.WriteEndElement();
            }
        }

        public void ReadXml(XmlReader reader)
        {
            reader.Read();
            XmlSerializer KeySerializer = new XmlSerializer(KeyType);
            XmlSerializer ValueSerializer = new XmlSerializer(ValueType);

            while (reader.NodeType != XmlNodeType.EndElement)
            {
                reader.ReadStartElement("Item");
                reader.ReadStartElement("Key");
                TKey tk = (TKey)KeySerializer.Deserialize(reader);
                reader.ReadEndElement();
                reader.ReadStartElement("Value");
                TValue vl = (TValue)ValueSerializer.Deserialize(reader);
                reader.ReadEndElement();
                reader.ReadEndElement();
                this.Add(tk, vl);
                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        public XmlSchema GetSchema()
        {
            return null;
        }
    }
}
