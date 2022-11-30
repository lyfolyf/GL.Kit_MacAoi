namespace System.Windows.Forms
{
    public static class TextBoxSettings
    {
        public static void AddEvent_KeyPress_InputNumber(this TextBox textBox)
        {
            textBox.KeyPress += ControlEventMethod.KeyPress_InputNumber;
        }

        public static void AddEvent_KeyPress_InputNumberAndPoint(this TextBox textBox)
        {
            textBox.KeyPress += ControlEventMethod.KeyPress_InputNumberAndPoint;
        }
    }

}
