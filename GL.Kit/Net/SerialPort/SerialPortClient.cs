using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Kit.Net.Ports
{
    public class SerialPortClient
    {
        public static string[] GetPortNames()
        {
            return SerialPort.GetPortNames();
        }
    }

}
