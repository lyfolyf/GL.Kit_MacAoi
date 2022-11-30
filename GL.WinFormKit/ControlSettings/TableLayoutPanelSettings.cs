namespace System.Windows.Forms
{
    public static class TableLayoutPanelSettings
    {
        /// <summary>
        /// 均分设置行列
        /// </summary>
        public static void SetRowColumnCount(this TableLayoutPanel tableLayoutPanel, int rowCount, int columnCount)
        {
            float rowPercent = 100F / rowCount;
            float colPercent = 100F / columnCount;

            tableLayoutPanel.RowStyles.Clear();
            tableLayoutPanel.ColumnStyles.Clear();

            tableLayoutPanel.RowCount = rowCount;
            tableLayoutPanel.ColumnCount = columnCount;

            for (int i = 0; i < rowCount; i++)
            {
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, rowPercent));
            }

            for (int j = 0; j < columnCount; j++)
            {
                tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, colPercent));
            }
        }

        /// <summary>
        /// 按指定大小设置行列
        /// </summary>
        public static void SetRowColumnCount(this TableLayoutPanel tableLayoutPanel, int rowCount, int columnCount, int width, int height)
        {
            tableLayoutPanel.RowStyles.Clear();
            tableLayoutPanel.ColumnStyles.Clear();

            tableLayoutPanel.RowCount = rowCount;
            tableLayoutPanel.ColumnCount = columnCount;

            for (int i = 0; i < rowCount; i++)
            {
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, height));
            }

            for (int j = 0; j < columnCount; j++)
            {
                tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, width));
            }
        }
    }
}
