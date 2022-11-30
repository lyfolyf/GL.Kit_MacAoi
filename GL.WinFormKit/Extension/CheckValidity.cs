using System.Text.RegularExpressions;

namespace System.Windows.Forms
{
    /// <summary>
    /// 控件验证
    /// </summary>
    public static class CheckValidity
    {
        /// <summary>
        /// 验证控件的 Text 属性不为空
        /// </summary>
        public static bool CheckNotEmpty(this Control control, string errmsg)
        {
            if (control.Text.Length == 0)
            {
                MessageBoxUtils.ShowWarn(errmsg);
                control.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 验证控件的 Text 属性不为空
        /// </summary>
        public static bool CheckNotWhiteSpace(this Control control, string errmsg)
        {
            if (control.Text.Trim().Length == 0)
            {
                MessageBoxUtils.ShowWarn(errmsg);
                control.Focus();
                return false;
            }

            return true;
        }

        public static bool Check(this Control control, Func<Control, bool> func)
        {
            return func(control);
        }

        /// <summary>
        /// 验证 TextBox 输入是否匹配指定正则
        /// </summary>
        public static bool CheckRegex(this TextBox textBox, string pattern, string errmsg)
        {
            if (!textBox.Text.IsMatch(pattern))
            {
                MessageBoxUtils.ShowWarn(errmsg);
                textBox.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 验证 TextBox 输入字符串最大长度
        /// </summary>
        public static bool CheckMaxLength(this TextBox textBox, int maxLength, string errmsg)
        {
            if (textBox.Text.Length > maxLength)
            {
                MessageBoxUtils.ShowWarn(errmsg);
                textBox.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 验证 TextBox 输入字符串最小长度
        /// </summary>
        public static bool CheckMinLength(this TextBox textBox, int minLength, string errmsg)
        {
            if (textBox.Text.Length < minLength)
            {
                MessageBoxUtils.ShowWarn(errmsg);
                textBox.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 验证 TextBox 输入字符串长度范围
        /// </summary>
        public static bool CheckLengthRange(this TextBox textBox, int minLength, int maxLength, string errmsg)
        {
            int length = textBox.Text.Length;
            if (length < minLength || length > maxLength)
            {
                MessageBoxUtils.ShowWarn(errmsg);
                textBox.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 验证 TextBox 输入字符串是否为数字，并且该数字在指定范围之内
        /// </summary>
        public static bool CheckIntRange(this TextBox textBox, int min, int max, string errmsg)
        {
            if (int.TryParse(textBox.Text, out int value))
            {
                if (value >= min && value <= max)
                {
                    return true;
                }
            }

            MessageBoxUtils.ShowWarn(errmsg);
            textBox.Focus();
            return false;
        }

        /// <summary>
        /// 验证两个 TextBox 的 Text 属性是否一致
        /// </summary>
        public static bool CheckSame(this TextBox textBox1, TextBox textBox2, string errmsg)
        {
            if (textBox1.Text != textBox2.Text)
            {
                MessageBoxUtils.ShowWarn(errmsg);
                textBox2.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 验证两个 TextBox 的 Text 属性是否不一致
        /// </summary>
        public static bool CheckNotSame(this TextBox textBox1, TextBox textBox2, string errmsg)
        {
            if (textBox1.Text == textBox2.Text)
            {
                MessageBoxUtils.ShowWarn(errmsg);
                textBox2.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 验证 ComboBox 是否下拉选择
        /// </summary>
        public static bool CheckSelected(this ComboBox comboBox, string errmsg)
        {
            if (comboBox.SelectedIndex == -1)
            {
                MessageBoxUtils.ShowWarn(errmsg);
                comboBox.Focus();
                return false;
            }

            return true;
        }

    }
}
