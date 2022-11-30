using System.Net;

namespace GL.Kit.Net.Sockets
{
    /// <summary>
    /// 网口信息
    /// </summary>
    public class NetworkInfo
    {
        public string IP { get; set; }

        public int Port { get; set; }

        /// <summary>
        /// 子网掩码
        /// </summary>
        public string SubnetMask { get; set; }

        /// <summary>
        /// 默认网关
        /// </summary>
        public string DefaultGateway { get; set; }

        public IPEndPoint ToIPEndPoint()
        {
            return new IPEndPoint(IPAddress.Parse(IP), Port);
        }

        public override string ToString()
        {
            return $"{IP}:{Port}";
        }
    }
}
