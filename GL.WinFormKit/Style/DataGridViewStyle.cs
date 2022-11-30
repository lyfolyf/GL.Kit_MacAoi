using System.Drawing;

namespace System.Windows.Forms.Style
{
    public class DataGridViewStyle
    {
        public static void SetBlueStyle(DataGridView dgv)
        {
            // 列标题样式
            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(80, 160, 255),
                Font = dgv.Font,
                ForeColor = Color.White,
                SelectionBackColor = SystemColors.Highlight,
                SelectionForeColor = SystemColors.HighlightText,
                WrapMode = DataGridViewTriState.True
            };

            // 奇数行样式（下标从 0 开始，所以第二行才是奇数行）
            DataGridViewCellStyle oddRowsStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(235, 243, 255)
            };

            // 默认样式，如果设置了奇数行样式，那么这就是偶数行的样式
            DataGridViewCellStyle defaultRowsStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White
            };

            dgv.BackgroundColor = Color.White;
            dgv.GridColor = Color.FromArgb(80, 160, 255);

            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.ColumnHeadersDefaultCellStyle = headerStyle;
            dgv.EnableHeadersVisualStyles = false;

            dgv.AlternatingRowsDefaultCellStyle = oddRowsStyle;
            dgv.RowsDefaultCellStyle = defaultRowsStyle;
        }
    }
}
