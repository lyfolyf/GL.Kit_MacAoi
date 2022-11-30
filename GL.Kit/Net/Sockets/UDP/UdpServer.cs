using GL.Kit.Log;
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;

namespace GL.Kit.Net.Sockets
{
    /* UDP是一种简单、面向数据报(SocketType.Dgram)的无连接协议，提供的是不一定可靠的传输服务。
     * “无连接”是指在正式通信前不必与对方先建立连接，不管对方状态如何都可以直接发送过去。
     * */

    /* UDP 不会粘包，但会丢包和乱序 */

    // UDP 接口和 TCP 接口不一样，TCP 建立连接以后收发都无需 EndPoint 参数，而 UDP 必须带 EndPoint 参数。

    public delegate void UdpDataReceived(EndPoint point, byte[] buffer);

    public class UdpServer : IDisposable
    {
        readonly IGLog log;

        const int MaxUDPSize = 4096;

        Socket _udpServer;

        public int ListenPort { get; }

        public UdpServer(int listenPort, IGLog log = null)
        {
            ListenPort = listenPort;

            this.log = log;

            _udpServer = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _udpServer.Bind(new IPEndPoint(IPAddress.Any, listenPort));
        }

        ConcurrentDictionary<string, UdpUserToken> tokens = new ConcurrentDictionary<string, UdpUserToken>();

        public UdpUserToken Connect(NetworkInfo networkInfo)
        {
            UdpUserToken token = new UdpUserToken(_udpServer, networkInfo, log);
            try
            {
                // 不管对端是否开启监听， 这里都不会报错
                _udpServer.Connect(networkInfo.ToIPEndPoint());
            }
            catch (Exception)
            {
                throw;
            }

            tokens.AddOrUpdate(networkInfo.ToString(), token, (key, oldValue) => token);

            return token;
        }

        public void Disconnect(UdpUserToken udpUserToken)
        {
            string key = udpUserToken.RemoteIP.ToString();

            tokens.TryRemove(key, out _);

            udpUserToken.Dispose();
        }

        public void ReciveAsync()
        {
            EndPoint remotePoint = new IPEndPoint(IPAddress.Any, 0);//用来保存发送方的ip和端口号

            byte[] m_buffer = new byte[MaxUDPSize];

            _udpServer.BeginReceiveFrom(m_buffer, 0, MaxUDPSize, SocketFlags.None, ref remotePoint, RequestCallback, m_buffer);
        }

        private void RequestCallback(IAsyncResult iar)
        {
            if (_udpServer == null) return;

            EndPoint remotePoint = new IPEndPoint(IPAddress.Any, 0);//用来保存发送方的ip和端口号

            int recvCount = _udpServer.EndReceiveFrom(iar, ref remotePoint);

            if (recvCount > 0)
            {
                byte[] newBuffer = new byte[recvCount];
                Buffer.BlockCopy(iar.AsyncState as byte[], 0, newBuffer, 0, recvCount);

                //log.Info("From: " + remotePoint.ToString() + "\r\n - receive [" + newBuffer.ToX2String() + "]");

                string key = remotePoint.ToString();
                if (tokens.ContainsKey(key))
                {
                    tokens[key].ReceiveAsync(newBuffer);
                }
            }

            if (!m_CleanedUp)
            {
                ReciveAsync();
            }
        }

        public void Close()
        {
            Dispose(true);
        }

        private bool m_CleanedUp = false;

        protected virtual void Dispose(bool disposing)
        {
            if (m_CleanedUp)
            {
                return;
            }

            if (disposing)
            {
                m_CleanedUp = true;
                if (_udpServer != null)
                {
                    _udpServer.Shutdown(SocketShutdown.Both);
                    _udpServer.Close();
                    _udpServer = null;
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
