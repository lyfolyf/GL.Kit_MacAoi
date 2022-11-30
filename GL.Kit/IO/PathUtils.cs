using System.Linq;
using System.Text.RegularExpressions;

namespace System.IO
{
    /// <summary>
    /// 路径扩展方法
    /// </summary>
    public static class PathUtils
    {
        private static string _currentDirectory;

        /// <summary>
        /// 当前应用程序根目录
        /// </summary>
        public static string CurrentDirectory
        {
            get
            {
                if (_currentDirectory == null)
                {
                    _currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                }
                return _currentDirectory;
            }
        }

        /// <summary>
        /// 获取绝对路径
        /// </summary>
        public static string GetAbsolutePath(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            path = path.Replace("/", "\\");

            return IsAbsolutePath(path) ? path : CurrentDirectory + path.TrimStart('\\');
        }

        /// <summary>
        /// 是否绝对路径
        /// </summary>
        public static bool IsAbsolutePath(string path)
        {
            //Path.IsPathRooted
            return path.IsMatch("^[c-zC-Z]:");
        }

        static string GetCopyName(string path, bool force = true)
        {
            path = path.Replace("/", "\\");

            string dir = Path.GetDirectoryName(path);
            if (!dir.EndsWith("\\"))
            {
                dir += "\\";
            }
            string filename = Path.GetFileNameWithoutExtension(path);
            string extension = Path.GetExtension(path);

            if (force)
            {
                filename += "(2)";
            }
            else
            {
                Match match = Regex.Match(filename, @"^(.*?)\((\d)\)$");
                if (match.Success)
                {
                    int index = int.Parse(match.Groups[2].Value);

                    filename = $"{match.Groups[1]}({index + 1})";
                }
                else
                {
                    filename += "(2)";
                }
            }

            return $@"{dir}{filename}{extension}";
        }

        /// <summary>
        /// 复制文件时，获取新的文件名
        /// <para>a.txt -> a(2).txt</para>
        /// <para>a(2).txt -> a(3).txt</para>
        /// </summary>
        /// <param name="path"></param>
        /// <param name="force">当为 true 时，a(2).txt 返回 a(2)(2).txt 而不是 a(3).txt
        /// <para>默认 false</para>
        /// </param>
        public static string GetCopyFileName(string path, bool force = true)
        {
            string dir = Path.GetDirectoryName(path);

            string[] filenames = Directory.GetFiles(dir);

            string copyFilename = GetCopyName(path, force);

            if (filenames.Contains(copyFilename))
            {
                copyFilename = GetCopyName(copyFilename, false);
            }

            return copyFilename;
        }

        /// <summary>
        /// 复制文件夹时，获取新的文件夹名
        /// <para>a -> a(2)</para>
        /// <para>a(2) -> a(3)</para>
        /// </summary>
        /// <param name="path"></param>
        /// <param name="force">当为 true 时，a(2) 返回 a(2)(2) 而不是 a(3)
        /// <para>默认 true</para>
        /// </param>
        public static string GetCopyDirectory(string path, bool force = true)
        {
            string dir = Path.GetDirectoryName(path);

            string[] subDirs = Directory.GetDirectories(dir);

            string copyDirectory = GetCopyName(path, force);

            if (subDirs.Contains(copyDirectory))
            {
                copyDirectory = GetCopyName(copyDirectory, false);
            }

            return copyDirectory;
        }

    }
}
