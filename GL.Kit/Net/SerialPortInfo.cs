using System.IO.Ports;

namespace GL.Kit.Net
{
    /// <summary>
    /// 串口信息
    /// </summary>
    public class SerialInfo
    {
        /// <summary>
        /// COM口
        /// </summary>
        public string PortName { get; set; }
        /// <summary>
        /// 波特率
        /// </summary>
        public int BaudRate { get; set; }
        /// <summary>
        /// 奇偶校验位
        /// </summary>
        public Parity Parity { get; set; }
        /// <summary>
        /// 数据位
        /// </summary>
        public int DataBits { get; set; }
        /// <summary>
        /// 停止位
        /// </summary>
        public StopBits StopBits { get; set; }
    }
}
