using System;
using System.Collections.Generic;
using System.Drawing;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace GL.NpoiKit
{
    /* 行列坐标均从0开始
     * */

    public class Sheet
    {
        readonly IWorkbook _workbook;
        readonly ISheet _sheet;

        internal Sheet(IWorkbook workbook, string sheetname)
        {
            _workbook = workbook;

            _sheet = workbook.GetOrCreateISheet(sheetname);
        }

        /// <summary>
        /// 设置列宽
        /// </summary>
        /// <param name="columnIndex">列坐标</param>
        /// <param name="width">宽度</param>
        public void SetColumnWidth(int columnIndex, int width)
        {
            //_sheet.SetColumnWidth(columnIndex, width * 256);
            _sheet.SetColumnWidth(columnIndex, (int)(width * 20 * 2.44));
        }

        /// <summary>
        /// 设置行高
        /// </summary>
        /// <param name="rowIndex">行坐标</param>
        /// <param name="height">行高</param>
        public void SetRowHeight(int rowIndex, short height)
        {
            IRow row = getRow(rowIndex);
            row.Height = (short)(height * 20);
        }

        /// <summary>
        /// 设置单元格值
        /// </summary>
        /// <param name="rowIndex">行坐标</param>
        /// <param name="columnIndex">列坐标</param>
        /// <param name="value">值</param>
        public void SetCellValue(int rowIndex, int columnIndex, object value)
        {
            ICell cell = getCell(rowIndex, columnIndex);

            cell.SetValue(value);
        }

        /// <summary>
        /// 设置单元格样式
        /// </summary>
        /// <param name="rowIndex">行坐标</param>
        /// <param name="columnIndex">列坐标</param>
        /// <param name="style">样式</param>
        public void SetCellStyle(int rowIndex, int columnIndex, NpoiStyle style)
        {
            ICell cell = getCell(rowIndex, columnIndex);
            cell.CellStyle = setCellStyle(style);
        }

        /// <summary>
        /// 将源单元格的样式应用于目标单元格
        /// </summary>
        /// <param name="srcRowIndex">源单元格行坐标</param>
        /// <param name="srcColunmIndex">源单元格列坐标</param>
        /// <param name="desRowIndex">目标单元格行坐标</param>
        /// <param name="desColumnIndex">目标单元格列坐标</param>
        public void CloneStyle(int srcRowIndex, int srcColunmIndex, int desRowIndex, int desColumnIndex)
        {
            ICell srcCell = getCell(srcRowIndex, srcColunmIndex);
            ICell desCell = getCell(desRowIndex, desColumnIndex);
            desCell.CellStyle = srcCell.CellStyle;
        }

        /// <summary>
        /// 批量设置单元格样式
        /// </summary>
        /// <param name="ps">单元格坐标集合，X坐标是ColumnIndex，Y坐标是RowIndex</param>
        /// <param name="style">样式</param>
        public void SetMultipleStyle(Point[] ps, NpoiStyle style)
        {
            ICellStyle icellStyle = setCellStyle(style);
            foreach (Point p in ps)
            {
                getCell(p.Y, p.X).CellStyle = icellStyle;
            }
        }

        /// <summary>
        /// 批量设置单元格样式
        /// </summary>
        /// <param name="firstRow">起始行</param>
        /// <param name="lastRow">结束行</param>
        /// <param name="firstCol">起始列</param>
        /// <param name="lastCol">结束列</param>
        /// <param name="style">样式</param>
        public void SetMultipleStyle(int firstRow, int lastRow, int firstCol, int lastCol, NpoiStyle style)
        {
            ICellStyle icellStyle = setCellStyle(style);
            for (int i = firstRow; i <= lastRow; i++)
            {
                for (int j = firstCol; j <= lastCol; j++)
                {
                    getCell(i, j).CellStyle = icellStyle;
                }
            }
        }

        /// <summary>
        /// 设置行样式
        /// </summary>
        /// <param name="rowIndex">行坐标</param>
        /// <param name="style">样式</param>
        public void SetRowStyle(int rowIndex, NpoiStyle style)
        {
            IRow row = getRow(rowIndex);
            row.RowStyle = setCellStyle(style);
        }

        /// <summary>
        /// 合并单元格
        /// </summary>
        /// <param name="firstRow">起始行</param>
        /// <param name="lastRow">结束行</param>
        /// <param name="firstCol">起始列</param>
        /// <param name="lastCol">结束列</param>
        public void AddMergedRegion(int firstRow, int lastRow, int firstCol, int lastCol)
        {
            _sheet.AddMergedRegion(new CellRangeAddress(firstRow, lastRow, firstCol, lastCol));
        }

        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="isColumnWritten">是否要导入列名</param>
        /// <param name="rowIndex">起始行坐标</param>
        /// <param name="columnIndex">起始列坐标</param>
        public void Export<T>(IEnumerable<T> data, bool isColumnWritten = true, int rowIndex = 0, int columnIndex = 0)
        {
            if (data == null) throw new ArgumentNullException("data");

            NPOIExcelHelper.FillSheet<T>(_sheet, data, isColumnWritten, rowIndex, columnIndex);
        }

        private IRow getRow(int rowIndex)
        {
            IRow row = _sheet.GetRow(rowIndex);
            if (row == null)
            {
                row = _sheet.CreateRow(rowIndex);
            }
            return row;
        }

        private ICell getCell(int rowIndex, int columnIndex)
        {
            IRow row = getRow(rowIndex);

            ICell cell = row.GetCell(columnIndex);
            if (cell == null)
            {
                cell = row.CreateCell(columnIndex);
            }
            return cell;
        }

        private ICellStyle getCellStyle(ICell cell)
        {
            return null;
        }

        private ICellStyle setCellStyle(NpoiStyle style)
        {
            ICellStyle _style = _workbook.CreateCellStyle();
            _style.Alignment = (NPOI.SS.UserModel.HorizontalAlignment)(int)style.HorizontalAlignment;
            _style.VerticalAlignment = (NPOI.SS.UserModel.VerticalAlignment)(int)style.VerticalAlignment;
            _style.WrapText = style.WrapText;
            _style.FillBackgroundColor = (short)style.BackgroundColor;

            IFont font = _workbook.CreateFont();
            font.IsBold = style.Bold;
            font.IsItalic = style.Italic;
            font.IsStrikeout = style.Strikeout;
            font.FontName = style.FontName;
            font.FontHeightInPoints = style.FontSize;
            font.Color = (short)style.FontColor;
            _style.SetFont(font);

            _style.BorderBottom = (BorderStyle)style.FourBorders.BorderBottom.BorderStyle;
            _style.BorderLeft = (BorderStyle)style.FourBorders.BorderLeft.BorderStyle;
            _style.BorderRight = (BorderStyle)style.FourBorders.BorderRight.BorderStyle;
            _style.BorderTop = (BorderStyle)style.FourBorders.BorderTop.BorderStyle;
            _style.BottomBorderColor = (short)style.FourBorders.BorderBottom.BorderColor;
            _style.LeftBorderColor = (short)style.FourBorders.BorderLeft.BorderColor;
            _style.RightBorderColor = (short)style.FourBorders.BorderRight.BorderColor;
            _style.TopBorderColor = (short)style.FourBorders.BorderTop.BorderColor;

            return _style;
        }
    }
}
