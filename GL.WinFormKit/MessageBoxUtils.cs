namespace System.Windows.Forms
{
    public class MessageBoxUtils
    {
        public static void ShowInfo(string msg)
        {
            MessageBox.Show(msg, "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void ShowError(string msg)
        {
            MessageBox.Show(msg, "出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ShowWarn(string msg)
        {
            MessageBox.Show(msg, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static DialogResult ShowQuestion(string msg)
        {
            return MessageBox.Show(msg, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        public static DialogResult ShowQuestionCanCancel(string msg)
        {
            return MessageBox.Show(msg, "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        }
    }
}
