namespace System.Windows.Forms
{
    public partial class DataGridViewProcessCmdKey : DataGridView
    {
        public DataGridViewProcessCmdKey()
        {
            InitializeComponent();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
                return base.ProcessCmdKey(ref msg, keyData);

            OnKeyPress(new KeyPressEventArgs((char)keyData));
            return true;
        }
    }
}
