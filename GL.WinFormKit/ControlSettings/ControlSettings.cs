using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
    public static class ControlSettings
    {
        /// <summary>
        /// 设置双缓存
        /// </summary>
        public static void SetDoubleBuffered(this Control control)
        {
            var pi = control.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(control, true, null);
        }

        /// <summary>
        /// 启用/禁用控件重绘
        /// </summary>
        /// <param name="control"></param>
        /// <param name="wParam">0: 禁用
        /// <para>1: 启用</para>
        /// </param>
        public static void SetRedraw(this Control control, int wParam)
        {
            User32Api.SendMessage(control.Handle, WinMsg.WM_SETREDRAW, wParam, IntPtr.Zero);
        }
    }

}
