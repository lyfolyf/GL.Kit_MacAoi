namespace System.Windows.Forms
{
    /// <summary>
    /// 控件事件的方法
    /// </summary>
    public class ControlEventMethod
    {
        /// <summary>
        /// 只能输入数字
        /// </summary>
        public static void KeyPress_InputNumber(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < (char)Keys.D0 || e.KeyChar > (char)Keys.D9) 
                && e.KeyChar != (char)Keys.Back 
                && e.KeyChar != (char)Keys.Enter
                && e.KeyChar != (char)Keys.Insert)
                e.Handled = true;
        }

        /// <summary>
        /// 只能输入数字和.
        /// </summary>
        public static void KeyPress_InputNumberAndPoint(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < (char)Keys.D0 || e.KeyChar > (char)Keys.D9)
                   && e.KeyChar != (char)Keys.Delete   // 英文句号
                   && e.KeyChar != (char)Keys.Back
                   && e.KeyChar != (char)Keys.Insert)
            {
                e.Handled = true;
            }
        }
    }
}
