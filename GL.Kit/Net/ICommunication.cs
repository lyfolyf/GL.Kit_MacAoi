using System;
using System.Text;

namespace GL.Kit.Net
{
    public delegate void CommunicationHandle(ICommunication communication);

    public interface ICommunication
    {
        event EventHandler Colsed;

        event EventHandler<DataEventArgs> DataReceived;

        bool Send(string msg);

        bool Send(string msg, Encoding encoding);

        bool Send(byte[] buffer);

        void ReceiveAsync();

        void Close();
    }

    public class DataEventArgs : EventArgs
    {
        public object Data { get; set; }

        public DataEventArgs(object data)
        {
            Data = data;
        }
    }
}
