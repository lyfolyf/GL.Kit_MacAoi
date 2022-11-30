using GL.Kit.Log;
using System;
using System.Net;
using System.Net.Sockets;

namespace GL.Kit.Net.Sockets
{
    public class TcpClient
    {
        public int SendTimeout { get; set; } = 500;

        public int ReceiveTimeout { get; set; } = 500;

        readonly IGLog log;

        readonly NetworkInfo m_networkInfo;

        Socket socketClient;

        public TcpClient(NetworkInfo networkInfo, IGLog log = null)
        {
            m_networkInfo = networkInfo;
            this.log = log;
        }

        public IPEndPoint ClientIP { get; set; }

        /// <summary>
        /// 建立与远程主机的连接
        /// </summary>
        /// <returns>True:连接成功; False:连接失败</returns>
        public TcpUserToken Connect()
        {
            try
            {
                socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                {
                    SendTimeout = SendTimeout,
                    ReceiveTimeout = ReceiveTimeout
                };
                socketClient.Bind(ClientIP ?? new IPEndPoint(IPAddress.Any, 0));

                socketClient.Connect(IPAddress.Parse(m_networkInfo.IP), m_networkInfo.Port);

                log?.Info($"连接 {m_networkInfo.IP}:{m_networkInfo.Port} 成功");

                return new TcpUserToken(socketClient, log);
            }
            catch (Exception e)
            {
                log?.Error($"连接 {m_networkInfo.IP}:{m_networkInfo.Port} 失败", e);

                throw;
            }
        }

    }
}
