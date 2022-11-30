using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace GL.Kit.Serialization
{
    public class BinarySerializer : ISerializerToFile
    {
        static Type FormatterType = typeof(BinaryFormatter);

        public void SerializeToFile<T>(T source, string filename)
        {
            if (source == null) throw new ArgumentNullException();

            FileStream fileStream = File.Create(filename);
            try
            {
                IFormatter formatter = (IFormatter)Activator.CreateInstance(FormatterType);
                formatter.Serialize(fileStream, source);
            }
            finally
            {
                if (fileStream != null)
                {
                    ((IDisposable)fileStream).Dispose();
                }
            }
        }

        public T DeserializeFromFile<T>(string filename)
        {
            FileStream fileStream = File.OpenRead(filename);
            try
            {
                IFormatter formatter = (IFormatter)Activator.CreateInstance(FormatterType);

                return (T)formatter.Deserialize(fileStream);
            }
            finally
            {
                if (fileStream != null)
                {
                    ((IDisposable)fileStream).Dispose();
                }
            }
        }
    }
}
