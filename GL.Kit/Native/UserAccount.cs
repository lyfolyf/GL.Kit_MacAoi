using System.Management;
using System.Security.Principal;

namespace GL.Kit.Native
{
    public static class UserAccount
    {
        /// <summary>  
        /// 获取当前登录用户的用户名  
        /// </summary>  
        public static string CurrentUser()
        {
            return WindowsIdentity.GetCurrent().Name;

            // 这个方法返回的结果是一样的
            //try
            //{
            //    string username = string.Empty;

            //    using (ManagementClass mc = new ManagementClass("Win32_ComputerSystem"))
            //    {
            //        using (ManagementObjectCollection moc = mc.GetInstances())
            //        {
            //            foreach (ManagementObject mo in moc)
            //            {
            //                username = mo["UserName"].ToString();
            //            }
            //        }
            //    }

            //    return username;
            //}
            //catch
            //{
            //    return "unknown";
            //}
        }

        /// <summary>
        /// 判断当前登录用户是否为管理员账户
        /// </summary>
        public static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);

            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
