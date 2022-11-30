using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
    public partial class GTextBox : TextBox
    {
        public GTextBox()
        {
            InitializeComponent();

            BorderStyle = BorderStyle.FixedSingle;
        }

        #region 自定义属性

        Color borderColor = Color.FromArgb(80, 160, 255);
        /// <summary>
        /// 获得或设置控件的边框颜色
        /// <para>仅当 BorderStyle = BorderStyle.FixedSingle 时有效</para>
        /// </summary>
        [Browsable(true)]
        [Category("自定义")]
        [DefaultValue(typeof(Color), "80, 160, 255")]
        [Description("获得或设置控件的边框颜色，仅当 BorderStyle = BorderStyle.FixedSingle 时有效")]
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
                Invalidate();
            }
        }

        #endregion

        /* MSDN: 那些由 Windows 完成其所有绘图的控件（例如 Textbox）从不调用它们的 OnPaint 方法。
         * 请参见您要修改的控件的文档，查看 OnPaint 方法是否可用。
         * 如果某个控件未将 OnPaint 作为成员方法列出，则您无法通过重写此方法改变其外观。
         * */

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 0xf || m.Msg == 0x133)
            {
                IntPtr hDC = User32Api.GetWindowDC(m.HWnd);

                if (hDC.ToInt32() == 0) return;

                //只有在边框样式为FixedSingle时自定义边框样式才有效   
                if (BorderStyle == BorderStyle.FixedSingle)
                {
                    Graphics g = Graphics.FromHdc(hDC);
                    g.SetHighQuality();

                    //绘制边框
                    g.DrawRectangle(borderColor, 0, 0, Width - 1, Height - 1);

                    g.SetDefaultQuality();
                }

                //返回结果   
                m.Result = IntPtr.Zero;
                //释放   
                User32Api.ReleaseDC(m.HWnd, hDC);
            }
        }
    }
}
