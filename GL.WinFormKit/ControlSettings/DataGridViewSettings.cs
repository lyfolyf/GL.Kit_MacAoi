using System.Drawing;

namespace System.Windows.Forms
{
    public static class DataGridViewSettings
    {
        /// <summary>
        /// 设置排序模式，仅 TextBoxColumn 有效
        /// </summary>
        public static void SetSortMode(this DataGridView dgv, DataGridViewColumnSortMode sortMode)
        {
            foreach (var col in dgv.Columns)
            {
                if (col is DataGridViewTextBoxColumn)
                {
                    (col as DataGridViewTextBoxColumn).SortMode = sortMode;
                }
            }
        }

        #region 事件

        /// <summary>
        /// 按住 Ctrl 键，滚轮缩放字体事件
        /// </summary>
        public static void AddEvent_MouseWheel_ZoomFontSize(this DataGridView dgv)
        {
            dgv.MouseWheel += (sender, e) =>
            {
                if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                {
                    float size = dgv.DefaultCellStyle.Font.Size;

                    if (e.Delta < 0)
                        size += 1;
                    else
                        size -= 1;

                    dgv.DefaultCellStyle.Font = new Font(dgv.DefaultCellStyle.Font.Name, size);
                }
            };
        }

        #endregion
    }
}
