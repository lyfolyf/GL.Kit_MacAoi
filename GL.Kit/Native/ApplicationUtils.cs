using System.Diagnostics;
using System.Windows.Forms;

namespace GL.Kit.Native
{
    public static class ApplicationUtils
    {
        public static void RestartProcess()
        {
            Application.Exit();
            Process.Start(Application.ExecutablePath);
        }
    }
}
