namespace System.Windows.Forms
{
    public static class TextBoxExtension
    {
        public static int IntValue(this TextBox textBox)
        {
            return int.Parse(textBox.Text);
        }
    }
}
