using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    public partial class GTabControl : TabControl
    {
        public GTabControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint
                | ControlStyles.ResizeRedraw | ControlStyles.DoubleBuffer, true);

            ItemSize = new Size(150, 40);
            DrawMode = TabDrawMode.OwnerDrawFixed;
            Size = new Size(450, 270);
        }

        #region 自定义属性

        Color tabBackColor = Color.FromArgb(56, 56, 56);
        /// <summary>
        /// 背景色
        /// <para>包括 Tab 页的背景色</para>
        /// </summary>
        [Browsable(true)]
        [Category("自定义")]
        [DefaultValue(typeof(Color), "56, 56, 56")]
        [Description("背景色")]
        public Color TabBackColor
        {
            get => tabBackColor;
            set
            {
                tabBackColor = value;
                Invalidate();
            }
        }

        Color selectedTabColor = Color.FromArgb(36, 36, 36);
        /// <summary>
        /// 选中 Tab 页的背景色
        /// </summary>
        [Browsable(true)]
        [Category("自定义")]
        [DefaultValue(typeof(Color), "36, 36, 36")]
        [Description("选中 Tab 页的背景色")]
        public Color SelectedTabColor
        {
            get => selectedTabColor;
            set
            {
                selectedTabColor = value;
                Invalidate();
            }
        }

        Color selectedTabForeColor = Color.FromArgb(80, 160, 255);
        /// <summary>
        /// 选中 Tab 页的字体颜色
        /// </summary>
        [Browsable(true)]
        [Category("自定义")]
        [DefaultValue(typeof(Color), "80, 160, 255")]
        [Description("选中 Tab 页的字体颜色")]
        public Color TabSelectedForeColor
        {
            get => selectedTabForeColor;
            set
            {
                selectedTabForeColor = value;
                Invalidate();
            }
        }

        Color unSelectedTabForeColor = Color.FromArgb(240, 240, 240);
        /// <summary>
        /// 未选中 Tab 页的字体颜色
        /// </summary>
        [Browsable(true)]
        [Category("自定义")]
        [DefaultValue(typeof(Color), "240, 240, 240")]
        [Description("未选中 Tab 页的字体颜色")]
        public Color UnSelectedTabForeColor
        {
            get => unSelectedTabForeColor;
            set
            {
                unSelectedTabForeColor = value;
                Invalidate();
            }
        }

        Color selectedTabUnderlineColor = Color.FromArgb(80, 160, 255);
        /// <summary>
        /// 选中 Tab 页字体下方下划线颜色
        /// </summary>
        [Browsable(true)]
        [Category("自定义")]
        [DefaultValue(typeof(Color), "80, 160, 255")]
        [Description("选中 Tab 页字体下方下划线颜色")]
        public Color SelectedTabUnderlineColor
        {
            get => selectedTabUnderlineColor;
            set
            {
                selectedTabUnderlineColor = value;
                Invalidate();
            }
        }

        #endregion

        protected override void CreateHandle()
        {
            base.CreateHandle();

            SizeMode = TabSizeMode.Fixed;
            Appearance = TabAppearance.Normal;
            Alignment = TabAlignment.Top;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;

            g.SetHighQuality();

            // 绘制 Tab 页背景色
            Rectangle tabsRect = new Rectangle(0, 0, Width, ItemSize.Height);
            g.FillRectangle(TabBackColor, tabsRect);

            // 绘制 Tab 页头
            for (int index = 0; index < TabCount; index++)
            {
                Rectangle tabRect = GetTabRect(index);

                Rectangle myRect = new Rectangle(tabRect.X - 2, tabRect.Y - 2, ItemSize.Width, ItemSize.Height);

                SizeF fontSize = e.Graphics.MeasureString(TabPages[index].Text, Font);

                if (index == SelectedIndex)
                {
                    // 绘制选中 Tab 页背景色
                    g.FillRectangle(SelectedTabColor, myRect);

                    // 绘制选中 Tab 页字体下方下划线
                    g.FillRectangle(SelectedTabUnderlineColor, myRect.Left, myRect.Height - 4, myRect.Width, 4);
                }

                // 绘制 Tab 页字体

                Color fontColor = index == SelectedIndex ? selectedTabForeColor : UnSelectedTabForeColor;
                float textX = myRect.Left + (myRect.Width - fontSize.Width) / 2.0f;
                float textY = myRect.Top + (myRect.Height - fontSize.Height) / 2.0f;
                g.DrawString(TabPages[index].Text, Font, fontColor, textX, textY);
            }

            g.SetDefaultQuality();
        }
    }
}
