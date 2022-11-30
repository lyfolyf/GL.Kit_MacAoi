namespace System.Windows.Forms
{
    public static class ControlExtension
    {
        public static void AsyncAction(this Control control, Action<Control> action)
        {
            if (control.InvokeRequired)
            {
                IAsyncResult async = control.BeginInvoke(action, control);

                control.EndInvoke(async);
            }
            else
            {
                action(control);
            }
        }

        public static void AsyncAction(this ToolStrip toolStrip, Action<Control> action)
        {
            if (toolStrip.InvokeRequired)
            {
                IAsyncResult async = toolStrip.BeginInvoke(action, toolStrip);

                toolStrip.EndInvoke(async);
            }
            else
            {
                action(toolStrip);
            }
        }
    }
}
