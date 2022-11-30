using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;

namespace System.Windows.Forms
{
    /// <summary>
    /// 支持行合并和二维表头的 DataGridView
    /// </summary>
    public partial class DataGridViewRowMerge : DataGridView
    {
        public DataGridViewRowMerge()
        {
            InitializeComponent();

            // 启用双缓存的话，必须每个单元格都绘制
            // 否则滚动条拖动的时候，没有绘制的单元格就显示一个黑块块
            SetStyle(ControlStyles.AllPaintingInWmPaint     //不擦除背景 ,减少闪烁
                | ControlStyles.OptimizedDoubleBuffer       //双缓冲
                | ControlStyles.UserPaint                   //使用自定义的重绘事件,减少闪烁
                , true);
        }

        protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex == 1)
            {
                Rectangle r = GetCellDisplayRectangle(1, -1, true);
            }

            if (e.ColumnIndex > -1)
            {
                if (e.RowIndex == -1)
                {
                    titleBottom = e.CellBounds.Y + e.CellBounds.Height;
                }
                else
                {
                    DrawCell(e);
                }
            }
            else if (e.RowIndex == -1)
            {
                if (SpanRows.ContainsKey(e.ColumnIndex)) //被合并的列
                {
                    //画边框
                    Graphics g = e.Graphics;
                    e.Paint(e.CellBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);

                    int left = e.CellBounds.Left, top = e.CellBounds.Top + 2,
                    right = e.CellBounds.Right, bottom = e.CellBounds.Bottom;

                    switch (SpanRows[e.ColumnIndex].Position)
                    {
                        case 1:
                            left += 2;
                            break;
                        case 2:
                            break;
                        case 3:
                            right -= 2;
                            break;
                    }

                    //画上半部分底色
                    g.FillRectangle(new SolidBrush(this.MergeColumnHeaderBackColor), left, top,
                    right - left, (bottom - top) / 2);

                    //画中线
                    g.DrawLine(new Pen(this.GridColor), left, (top + bottom) / 2,
                    right, (top + bottom) / 2);

                    //写小标题
                    StringFormat sf = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };

                    g.DrawString(e.Value + "", e.CellStyle.Font, Brushes.Black,
                    new Rectangle(left, (top + bottom) / 2, right - left, (bottom - top) / 2), sf);
                    left = this.GetColumnDisplayRectangle(SpanRows[e.ColumnIndex].Left, true).Left - 2;

                    if (left < 0) left = this.GetCellDisplayRectangle(-1, -1, true).Width;
                    right = this.GetColumnDisplayRectangle(SpanRows[e.ColumnIndex].Right, true).Right - 2;
                    if (right < 0) right = this.Width;

                    g.DrawString(SpanRows[e.ColumnIndex].Text, e.CellStyle.Font, Brushes.Black,
                    new Rectangle(left, top, right - left, (bottom - top) / 2), sf);
                    e.Handled = true;
                }
            }
            base.OnCellPainting(e);
        }

        // 缓存，单元格内容，在指定列宽的情况下的自动换行
        Dictionary<(string line, int columnIndex), (float columnWidth, List<string> lines)> cache
            = new Dictionary<(string line, int columnIndex), (float columnWidth, List<string> lines)>();

        // 列头的底端坐标
        float titleBottom = 0;

        /// <summary>
        /// 画单元格
        /// <para>不显示的行，是不会触发这个方法的</para>
        /// </summary>
        void DrawCell(DataGridViewCellPaintingEventArgs e)
        {
            if (!MergeColumnNames.Contains(Columns[e.ColumnIndex].Name)) return;

            string curValue = e.Value?.ToString().Trim();
            Rectangle rectangle;

            // 都为空的单元格不合并
            // 当行设置了背景色，空单元格不改变背景色
            // 这逻辑其实是不对的，但是能满足当前需求
            if (string.IsNullOrEmpty(curValue))
            {
                DrawEmptyCell(e);
                e.Handled = true;
                return;
            }

            int upRows = GetUpRows(curValue, e.RowIndex, e.ColumnIndex);
            int downRows = GetDownRows(curValue, e.RowIndex, e.ColumnIndex);

            // 无需合并单元格
            if (upRows == 0 && downRows == 0) return;

            int textHeight = (int)e.Graphics.MeasureString(curValue, e.CellStyle.Font).Height - 1;

            // 计算自动换行
            List<string> lines;
            if (cache.ContainsKey((curValue, e.ColumnIndex))
                && cache[(curValue, e.ColumnIndex)].columnWidth == e.CellBounds.Width
                )
            {
                lines = cache[(curValue, e.ColumnIndex)].lines;
            }
            else
            {
                lines = TextControlUtils.WordWrap(curValue, e.Graphics, e.CellStyle.Font, e.CellBounds.Width);

                cache.Add((curValue, e.ColumnIndex), (e.CellBounds.Width, lines));
            }

            int count = upRows + downRows + 1;

            // 如果换行以后内容超出合并单元格的大小，则拉高合并的单元格
            int totalTextHeight = textHeight * lines.Count;

            int upHeight = 0;
            for (int i = 1; i <= upRows; i++)
            {
                upHeight += Rows[e.RowIndex - i].Height;
            }

            // 包含当前行
            int downHeight = 0;
            for (int i = 0; i <= downRows; i++)
            {
                downHeight += Rows[e.RowIndex + i].Height;
            }

            int totalCellHeight = upHeight + downHeight;

            if (totalCellHeight < totalTextHeight)
            {
                int offset = (totalTextHeight - totalCellHeight) / count;

                for (int i = 0; i < count; i++)
                {
                    Rows[e.RowIndex - upRows + i].Height = Rows[e.RowIndex - upRows + i].Height + offset;
                }

                totalCellHeight += offset * count;
            }

            int y1 = e.CellBounds.Y - upHeight;
            if (y1 > titleBottom)
                rectangle = new Rectangle(new Point(e.CellBounds.X, y1), new Size(e.CellBounds.Width, totalCellHeight));
            else
                rectangle = new Rectangle(new Point(e.CellBounds.X, (int)titleBottom), new Size(e.CellBounds.Width, totalCellHeight - ((int)titleBottom - y1)));
            // 填充背景色
            e.Graphics.FillRectangle(BackgroundColor, rectangle);

            using (SolidBrush gridBrush = new SolidBrush(GridColor),
                fontBrush = new SolidBrush(e.CellStyle.ForeColor))
            {
                int fontY = e.CellBounds.Y - upHeight + (totalCellHeight - totalTextHeight) / 2;

                foreach (string line in lines)
                {
                    if (fontY >= rectangle.Y)
                    {
                        e.Graphics.DrawString(line, e.CellStyle.Font, fontBrush, e.CellBounds.X, fontY);
                    }
                    fontY += textHeight;
                }

                // 画边框
                Pen gridLinePen = new Pen(gridBrush);

                // 画右边线
                e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1, e.CellBounds.Top - upHeight, e.CellBounds.Right - 1, e.CellBounds.Top + downHeight - 1);
                // 画下边线
                e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Top + downHeight - 1, e.CellBounds.Right - 1, e.CellBounds.Top + downHeight - 1);
            }

            e.Handled = true;
        }

        void DrawEmptyCell(DataGridViewCellPaintingEventArgs e)
        {
            Rectangle rectangle = new Rectangle(new Point(e.CellBounds.X, e.CellBounds.Y), new Size(e.CellBounds.Width, e.CellBounds.Height));
            // 填充背景色
            e.Graphics.FillRectangle(BackgroundColor, rectangle);

            // 画边框
            using (Pen gridLinePen = new Pen(GridColor))
            {
                // 画右边线
                e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Top + e.CellBounds.Height - 1);
                // 画下边线
                e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
            }
        }

        int GetUpRows(string curValue, int rowIndex, int colIndex)
        {
            int upRows = 0;
            for (int i = rowIndex - 1; i >= 0; i--)
            {
                if (curValue.Equals(Rows[i].Cells[colIndex].Value?.ToString()))
                {
                    upRows++;
                }
                else
                {
                    break;
                }
            }
            return upRows;
        }

        int GetDownRows(string curValue, int rowIndex, int colIndex)
        {
            int downRows = 0;
            for (int i = rowIndex + 1; i < Rows.Count; i++)
            {
                if (curValue.Equals(Rows[i].Cells[colIndex].Value?.ToString()))
                {
                    downRows++;
                }
                else
                    break;
            }

            return downRows;
        }

        #region 属性
        /// <summary>
        /// 设置或获取合并列的集合
        /// </summary>
        [MergableProperty(false)]
        [Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Localizable(true)]
        [Description("设置或获取合并列的集合"), Browsable(true), Category("单元格合并")]
        public List<string> MergeColumnNames { get; set; } = new List<string>();

        #endregion

        #region 二维表头

        private struct SpanInfo //表头信息
        {
            public SpanInfo(string Text, int Position, int Left, int Right)
            {
                this.Text = Text;
                this.Position = Position;
                this.Left = Left;
                this.Right = Right;
            }

            public string Text;     //列主标题
            public int Position;    //位置，1:左，2中，3右
            public int Left;        //对应左行
            public int Right;       //对应右行
        }

        private Dictionary<int, SpanInfo> SpanRows = new Dictionary<int, SpanInfo>();//需要2维表头的列

        /// <summary>
        /// 合并列
        /// </summary>
        /// <param name="ColIndex">列的索引</param>
        /// <param name="ColCount">需要合并的列数</param>
        /// <param name="Text">合并列后的文本</param>
        public void AddSpanHeader(int ColIndex, int ColCount, string Text)
        {
            if (ColCount < 2)
            {
                throw new Exception("行宽应大于等于2，合并1列无意义");
            }

            //将这些列加入列表
            int Right = ColIndex + ColCount - 1; //同一大标题下的最后一列的索引
            SpanRows[ColIndex] = new SpanInfo(Text, 1, ColIndex, Right); //添加标题下的最左列
            SpanRows[Right] = new SpanInfo(Text, 3, ColIndex, Right); //添加该标题下的最右列
            for (int i = ColIndex + 1; i < Right; i++) //中间的列
            {
                SpanRows[i] = new SpanInfo(Text, 2, ColIndex, Right);
            }
        }

        /// <summary>
        /// 清除合并的列
        /// </summary>
        public void ClearSpanInfo()
        {
            SpanRows.Clear();
        }

        /// <summary>
        /// 刷新显示表头
        /// </summary>
        public void ReDrawHead()
        {
            foreach (int si in SpanRows.Keys)
            {
                this.Invalidate(this.GetCellDisplayRectangle(si, -1, true));
            }
        }

        /// <summary>
        /// 二维表头的背景颜色
        /// </summary>
        [Description("二维表头的背景颜色"), Browsable(true), Category("二维表头")]
        public Color MergeColumnHeaderBackColor { get; set; } = SystemColors.Control;

        #endregion

        private void RowMergeDataGridView_Scroll(object sender, ScrollEventArgs e)
        {
            Refresh();
        }
    }
}

