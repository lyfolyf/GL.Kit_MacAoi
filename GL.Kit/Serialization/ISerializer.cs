using System;

namespace GL.Kit.Serialization
{
    public interface ISerializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        string Serialize<T>(T source);

        /// <summary>
        /// 序列化
        /// </summary>
        string Serialize(object source);

        /// <summary>
        /// 反序列化
        /// </summary>
        T Deserialize<T>(string str);

        /// <summary>
        /// 反序列化
        /// </summary>
        object Deserialize(string str, Type type);
    }

    public interface ISerializerToFile
    {
        /// <summary>
        /// 序列化到文件
        /// </summary>
        void SerializeToFile<T>(T source, string filename);

        /// <summary>
        /// 从文件反序列化
        /// </summary>
        T DeserializeFromFile<T>(string filename);
    }
}
