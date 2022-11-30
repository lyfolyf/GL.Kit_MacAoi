using GL.Kit.Log;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace GL.Kit.Net.Sockets
{
    public class TcpServer
    {
        public int SendTimeout { get; set; } = 500;

        public int ReceiveTimeout { get; set; } = 500;

        /// <summary>
        /// 远程连接接入后发生
        /// </summary>
        public event CommunicationHandle Accepted;

        int m_connectedClientCount;

        public int ConnectedCount
        {
            get { return m_connectedClientCount; }
        }

        public NetworkInfo NetworkInfo { get; }

        Socket listenSocket;

        readonly IGLog log;

        public TcpServer(NetworkInfo networkInfo, IGLog log = null)
        {
            NetworkInfo = networkInfo;
            this.log = log;
        }

        public void Listen()
        {
            if (listenSocket == null)
            {
                listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                {
                    SendTimeout = SendTimeout,
                    ReceiveTimeout = ReceiveTimeout
                };
            }

            listenSocket.Bind(new IPEndPoint(IPAddress.Parse(NetworkInfo.IP), NetworkInfo.Port));

            try
            {
                listenSocket.Listen(10);

                log?.Info($"开始监听：{NetworkInfo.IP}:{NetworkInfo.Port}");
            }
            catch (SocketException e)
            {
                Stop();

                log?.Error($"开始监听 {NetworkInfo.IP}:{NetworkInfo.Port} 失败，", e);

                throw;
            }

            StartAccept();
        }

        void StartAccept()
        {
            listenSocket.BeginAccept(AcceptCallBack, listenSocket);
        }

        void AcceptCallBack(IAsyncResult ar)
        {
            if (listenSocket == null) return;

            try
            {
                Socket acceptSocket = listenSocket.EndAccept(ar);
                Interlocked.Increment(ref m_connectedClientCount);

                log?.Info($"远程连接接入 {acceptSocket.RemoteEndPoint}");

                TcpUserToken socketUserToken = new TcpUserToken(acceptSocket, log);

                socketUserToken.Colsed += SocketUserToken_Colsed; ;

                Accepted?.Invoke(socketUserToken);

                StartAccept();
            }
            catch
            {
                // 关闭时有时会报：无法访问已释放的对象
                // 应该是 Close 后立即触发了此方法，还未来得及将 listenSocket 设为 null
            }
        }

        private void SocketUserToken_Colsed(object sender, EventArgs e)
        {
            Interlocked.Decrement(ref m_connectedClientCount);
        }

        public void Stop()
        {
            if (listenSocket != null)
            {
                listenSocket.Close();
                listenSocket = null;

                log?.Info($"停止监听：{NetworkInfo.IP}:{NetworkInfo.Port}");
            }
        }
    }
}
