using System.Collections.Generic;

namespace System.IO
{
    public static class FileUtils
    {
        /// <summary>
        /// 重命名文件
        /// </summary>
        /// <param name="sourceFileName">要重命名的文件，全路径</param>
        /// <param name="newname">新的文件名，仅需要文件名和后缀名</param>
        public static void Rename(string sourceFileName, string newname)
        {
            File.Move(sourceFileName, Path.Combine(Path.GetDirectoryName(sourceFileName), newname));
        }

        /// <summary>
        /// 获取指令目录下的所有文件和文件夹信息
        /// </summary>
        public static FileSystemInfo[] GetFileSystems(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);

            return GetFileSystems(directoryInfo);
        }

        /// <summary>
        /// 获取指令目录下的所有文件和文件夹信息
        /// </summary>
        public static FileSystemInfo[] GetFileSystems(DirectoryInfo directoryInfo)
        {
            List<FileSystemInfo> fileInofs = new List<FileSystemInfo>();

            FileInfo[] fileInfos = directoryInfo.GetFiles();
            fileInofs.AddRange(fileInfos);

            DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories();
            fileInofs.AddRange(directoryInfos);

            return fileInofs.ToArray();
        }
    }
}
