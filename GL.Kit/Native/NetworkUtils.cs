using System.Net;
using System.Net.NetworkInformation;

namespace GL.Kit.Native
{
    public static class NetworkUtils
    {
        public static bool Ping(string ip, int timeout = 1000)
        {
            Ping p = new Ping();
            PingReply reply = p.Send(ip, timeout);
            return reply.Status == IPStatus.Success;
        }

        public static bool Ping(IPAddress ip, int timeout = 1000)
        {
            Ping p = new Ping();
            PingReply reply = p.Send(ip, timeout);
            return reply.Status == IPStatus.Success;
        }
    }
}
