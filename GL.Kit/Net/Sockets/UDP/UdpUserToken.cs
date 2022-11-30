using GL.Kit.Log;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace GL.Kit.Net.Sockets
{
    public class UdpUserToken : ICommunication, IDisposable
    {
        public event EventHandler Colsed;

        public event EventHandler<DataEventArgs> DataReceived;

        readonly Socket socket;
        readonly IPEndPoint remote;
        readonly IGLog log;

        public NetworkInfo RemoteIP { get; }

        internal UdpUserToken(Socket socket, NetworkInfo networkInfo, IGLog log)
        {
            this.socket = socket;
            RemoteIP = networkInfo;
            remote = networkInfo.ToIPEndPoint();
            this.log = log;
        }

        public bool Send(string msg)
        {
            return Send(msg, Encoding.UTF8);
        }

        public bool Send(string msg, Encoding encoding)
        {
            if (msg == null || msg.Length == 0) return true;

            byte[] buffer = encoding.GetBytes(msg);

            return Send(buffer);
        }

        public bool Send(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0) return true;

            try
            {
                socket.SendTo(buffer, remote);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public void ReceiveAsync()
        {

        }

        public void ReceiveAsync(byte[] buffer)
        {
            DataReceived?.Invoke(this, new DataEventArgs(buffer));
        }

        public void Close()
        {

        }

        public void Dispose()
        {

        }
    }
}
