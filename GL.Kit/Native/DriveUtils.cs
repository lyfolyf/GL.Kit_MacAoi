using System.Linq;
using System.IO;
using System;

namespace GL.Kit.Native
{
    /// <summary>
    /// 硬盘相关方法
    /// </summary>
    public static class DriveUtils
    {
        /// <summary>
        /// 获取驱动器上存储空间的总大小(KB)
        /// </summary>
        /// <param name="driveName">盘符</param>
        public static long GetTotalSize(string driveName)
        {
            DriveInfo drive = GetDrive(driveName);

            return drive.TotalSize;
        }

        /// <summary>
        /// 获取驱动器上的可用空闲空间总量
        /// </summary>
        /// <param name="driveName">盘符</param>
        public static long GetTotalFreeSpace(string driveName)
        {
            DriveInfo drive = GetDrive(driveName);

            return drive.TotalFreeSpace;
        }

        private static DriveInfo GetDrive(string driveName)
        {
            if (string.IsNullOrEmpty(driveName))
                throw new ArgumentNullException(nameof(driveName));

            if (!driveName.EndsWith(":\\"))
                driveName += ":\\";

            DriveInfo drive = DriveInfo.GetDrives().FirstOrDefault(d => d.Name == driveName);

            if (drive == null)
                throw new Exception($"找不到 \"{driveName}\"");

            return drive;
        }
    }
}
