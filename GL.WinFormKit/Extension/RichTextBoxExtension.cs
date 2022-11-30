using System.Drawing;

namespace System.Windows.Forms
{
    public static class RichTextBoxExtension
    {
        public static void AppendLine(this RichTextBox rtb, string text)
        {
            rtb.AppendText(text + Environment.NewLine);
        }

        public static void AppendLine(this RichTextBox rtb, string text, Color color)
        {
            int selectionStart = rtb.SelectionStart;
            int selectionLength = rtb.SelectionLength;
            Color selectionColor = rtb.SelectionColor;

            int contentLength = rtb.Text.Length;

            rtb.SelectionStart = contentLength;
            rtb.SelectionColor = color;
            rtb.AppendText(text + Environment.NewLine);

            // AppendText 之后，光标会自动移动到末尾
            // 如果之前光标不在末尾，则移动回去
            if (selectionStart != contentLength)
            {
                rtb.SelectionStart = selectionStart;
                rtb.SelectionLength = selectionLength;
                rtb.SelectionColor = selectionColor;
            }
            else
            {
                rtb.ScrollToCaret();
            }
        }
    }
}
