using System.Management;

namespace GL.Kit.Native
{
    public static class SysInfoUtils
    {
        /// <summary>
        /// 获取硬盘号
        /// <para>设备管理器 -> 磁盘驱动器</para>
        /// <para>CMD 查看指令：wmic diskdrive get model</para>
        /// </summary>
        public static string GetDiskID()
        {
            try
            {
                string diskID = string.Empty;

                using (ManagementClass mc = new ManagementClass("Win32_DiskDrive"))
                {
                    using (ManagementObjectCollection moc = mc.GetInstances())
                    {
                        foreach (ManagementObject mo in moc)
                        {
                            //Caption 属性的值 Model 属性的值是一样的
                            diskID = mo.Properties["Model"].Value?.ToString();
                        }
                    }
                }

                return diskID;
            }
            catch
            {
                return "unknown";
            }
        }

        /// <summary>
        /// 获取硬盘序列号
        /// <para>CMD 查看指令：wmic diskdrive get serialnumber</para>
        /// </summary>
        public static string GetDiskSerialNumber()
        {
            try
            {
                string diskID = string.Empty;

                using (ManagementClass mc = new ManagementClass("Win32_DiskDrive"))
                {
                    using (ManagementObjectCollection moc = mc.GetInstances())
                    {
                        foreach (ManagementObject mo in moc)
                        {
                            diskID = mo.Properties["SerialNumber"].Value?.ToString();
                        }
                    }
                }

                return diskID;
            }
            catch
            {
                return "unknown";
            }
        }

        /// <summary>
        /// 获取 CpuID
        /// <para>CMD 查看指令：wmic CPU get ProcessorID</para>
        /// </summary>
        public static string GetCpuID()
        {
            try
            {
                string cpuID = string.Empty;

                using (ManagementClass mc = new ManagementClass("Win32_Processor"))
                {
                    using (ManagementObjectCollection moc = mc.GetInstances())
                    {
                        foreach (ManagementObject mo in moc)
                        {
                            cpuID = mo.Properties["ProcessorId"].Value.ToString();
                        }
                    }
                }

                return cpuID;
            }
            catch
            {
                return "unknown";
            }
        }

        /// <summary>
        /// 获取主板序列号
        /// <para>CMD 查看指令：wmic baseboard get serialnumber</para>
        /// </summary>
        public static string GetBaseBoardID()
        {
            try
            {
                string strID = null;

                using (ManagementClass mc = new ManagementClass("Win32_BaseBoard"))
                {
                    using (ManagementObjectCollection moc = mc.GetInstances())
                    {
                        foreach (ManagementObject mo in moc)
                        {
                            strID = mo.Properties["SerialNumber"].Value.ToString();
                        }
                    }
                }

                return strID;
            }
            catch
            {
                return "unknown";
            }
        }

        /// <summary>
        /// 获取 MAC 地址
        /// <para>CMD 查看指令：ipconfig/all</para>
        /// </summary>
        public static string GetMacAddress()
        {
            try
            {
                string mac = string.Empty;

                using (ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration"))
                {
                    using (ManagementObjectCollection moc = mc.GetInstances())
                    {
                        foreach (ManagementObject mo in moc)
                        {
                            if ((bool)mo["IPEnabled"] == true)
                            {
                                mac = mo["MacAddress"].ToString();
                            }
                        }
                    }
                }

                return mac;
            }
            catch
            {
                return "unknown";
            }
        }
    }
}
