using System;
using System.Diagnostics;

namespace GL.Kit.Native
{
    public static class ProcessUtils
    {
        /// <summary>
        /// 重启
        /// </summary>
        public static void Restart()
        {
            Process.Start("shutdown", @"/r /t 0");
        }

        public static void StartAsAdmin()
        {
            // 非管理员权限进程打开需要管理员权限的进程时,可以通过触发 UAC 来执行提权操作
            // 1、通过 ProcessStartInfo.Verb = "runas" 来触发 UAC，执行提权
            // 2、UAC 只能通过操作系统 Shell 启动进程来触发，所以，设置 ProcessStartInfo.UseShellExecute = true

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                UseShellExecute = true,
                WorkingDirectory = Environment.CurrentDirectory,
                FileName = Process.GetCurrentProcess().MainModule.FileName,
                Verb = "runas"
            };

            Process.Start(startInfo);
        }
    }
}
