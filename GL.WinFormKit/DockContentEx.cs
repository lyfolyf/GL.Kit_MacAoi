using System.Windows.Forms;

namespace WeifenLuo.WinFormsUI.Docking
{
    public partial class DockContentEx : DockContent
    {
        public DockContentEx()
        {
            InitializeComponent();

            ToolStripMenuItem tsmiClose = new ToolStripMenuItem("关闭");
            tsmiClose.Click += (sender, e) =>
            {
                if (HideOnClose)
                    Hide();
                else
                    Close();
            };

            TabPageContextMenuStrip = new ContextMenuStrip();
            TabPageContextMenuStrip.Items.Add(tsmiClose);
        }
    }
}
