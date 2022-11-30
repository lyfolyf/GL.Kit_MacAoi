using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.GControl
{
    public partial class GComboBox : ComboBox
    {
        public GComboBox()
        {
            InitializeComponent();
        }

        #region 自定义属性

        Color m_borderColor = Color.FromArgb(80, 160, 255);
        /// <summary>
        /// 获得或设置控件的边框颜色
        /// <para>下拉框边框颜色不会修改</para>
        /// </summary>
        [Browsable(true)]
        [Category("自定义")]
        [DefaultValue(typeof(Color), "80, 160, 255")]
        [Description("获得或设置控件的边框颜色，仅当 BorderStyle = BorderStyle.FixedSingle 时有效")]
        public Color BorderColor
        {
            get { return m_borderColor; }
            set
            {
                m_borderColor = value;
                Invalidate();
            }
        }

        #endregion

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 0x0F || m.Msg == 0x0133)
            {
                IntPtr hDC = User32Api.GetWindowDC(m.HWnd);

                if (hDC.ToInt32() == 0)
                    return;

                Graphics g = Graphics.FromHdc(hDC);
                g.SetHighQuality();

                //绘制边框
                g.DrawRectangle(m_borderColor, 0, 0, Width - 1, Height - 1);

                //返回结果   
                m.Result = IntPtr.Zero;
                //释放   
                User32Api.ReleaseDC(m.HWnd, hDC);
            }
        }
    }
}
