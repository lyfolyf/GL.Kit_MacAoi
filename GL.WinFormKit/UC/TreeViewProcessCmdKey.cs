namespace System.Windows.Forms
{
    /// <summary>
    /// 可以触发 KeyPress 事件的 TreeView
    /// <para>KeyPressEventArgs 中无法区分字母大小写，一律会认为是大写字母</para>
    /// </summary>
    public partial class TreeViewProcessCmdKey : TreeView
    {
        public TreeViewProcessCmdKey()
        {
            InitializeComponent();
        }

        // keyData 无法反应小写字母，会转换为大写
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            OnKeyPress(new KeyPressEventArgs((char)keyData));

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
