namespace System.Windows.Forms
{
    /// <summary>
    /// 禁用双击事件的 TreeView
    /// <para>TreeView 的勾选框在双击的时候，仅会触发一次 AfterCheck 事件</para>
    /// <para>导致界面的显示和实际 Node 的 Checked 属性会不一致。</para>
    /// </summary>
    public partial class TreeViewDisableDoubleClick : TreeView
    {
        public TreeViewDisableDoubleClick()
        {
            InitializeComponent();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x203)
            {
                m.Result = IntPtr.Zero;
            }

            else base.WndProc(ref m);
        }
    }
}
