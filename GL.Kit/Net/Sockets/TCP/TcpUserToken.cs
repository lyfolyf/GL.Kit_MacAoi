using GL.Kit.Log;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace GL.Kit.Net.Sockets
{
    public class TcpUserToken : ICommunication, IDisposable
    {
        readonly IGLog log;

        Socket socket;

        public event EventHandler Colsed;
        public event EventHandler<DataEventArgs> DataReceived;

        internal TcpUserToken(Socket socket, IGLog log)
        {
            this.socket = socket;

            this.log = log;
        }

        public EndPoint RemoteEndPoint
        {
            get { return socket.RemoteEndPoint; }
        }

        public bool Send(string msg)
        {
            return Send(msg, Encoding.UTF8);
        }

        public bool Send(string msg, Encoding encoding)
        {
            try
            {
                socket.Send(encoding.GetBytes(msg));

                log?.InfoFormat("发送指令：{0}", msg);

                return true;
            }
            catch (Exception e)
            {
                log?.Error($"发送数据失败。\r\n{msg}", e);

                return false;
            }
        }

        public bool Send(byte[] buffer)
        {
            try
            {
                socket.Send(buffer);

                //log.Info("\r\n - send [" + buffer.ToX2String() + "]");

                return true;
            }
            catch //(Exception e)
            {
                //log.Error("发送数据失败。\r\n - send fail [" + buffer.ToX2String() + "]", e);

                return false;
            }
        }

        /// <summary>
        /// 开始异步接收数据
        /// </summary>
        public void ReceiveAsync()
        {
            byte[] ReceiveBuffer = new byte[1024];

            try
            {
                // 实际情况是这句代码也会报错，错误信息如下：
                // System.Net.Sockets.SocketException (0x80004005): 远程主机强迫关闭了一个现有的连接
                socket.BeginReceive(ReceiveBuffer, 0, ReceiveBuffer.Length, SocketFlags.None, ReceivedCallBack, ReceiveBuffer);
            }
            catch (Exception e)
            {
                log?.Error("Socket 接收数据出错", e);
                Close();
            }
        }

        // 远端关闭连接，Connected = true，触发 ReceivedCallBack 方法，EndReceive 方法返回 0
        // 本地关闭连接，Connected = false，触发 ReceivedCallBack 方法，EndReceive 方法会报错
        void ReceivedCallBack(IAsyncResult ar)
        {
            // 防止本地关闭连接时 EndReceive 方法报错
            if (socket == null || !socket.Connected) return;

            try
            {
                int length = socket.EndReceive(ar);
                if (length > 0)
                {
                    byte[] buffer = new byte[length];
                    Array.Copy(ar.AsyncState as byte[], buffer, length);

                    //log.Info("\r\n - receive [" + buffer.ToX2String() + "]");

                    // 如果在 DataReceived 事件中关闭连接，继续调用 ReceiveAsync 就会出错
                    try
                    {
                        DataReceived?.Invoke(this, new DataEventArgs(buffer));
                    }
                    catch (Exception ex)
                    {
                        log?.Error("DataReceived Error", ex);
                    }

                    if (socket != null && socket.Connected)
                        ReceiveAsync();
                }
                else
                {
                    //进这里就表示远端关闭连接
                    //远端关闭连接，本地调用Close方法后不会再次进入本方法
                    Close();
                }
            }
            catch (Exception ex)
            {
                //进入这里就是真正的报错了
                log?.Error("Socket Error", ex);
                Close();
            }
        }

        public void Close()
        {
            Dispose(true);
        }

        protected bool m_CleanedUp = false;

        protected virtual void Dispose(bool disposing)
        {
            if (m_CleanedUp) return;

            if (disposing)
            {
                m_CleanedUp = true;

                if (socket != null)
                {
                    log?.Info($"{socket.RemoteEndPoint} 连接关闭");

                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                    socket = null;

                    //本地主动断开之后不能马上进行重连，否则有可能会因为关闭后 ReceivedCallBack 未触发而导致异常
                    System.Threading.Thread.Sleep(50);

                    Colsed?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
