using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

namespace GL.NpoiKit
{
    public class NPOIExcelHelper
    {
        #region 导出

        /// <summary>
        /// 将 DataTable 数据导出到 Excel
        /// </summary>
        /// <param name="datatable">要导出的数据</param>
        /// <param name="filename">导出的文件名</param>
        /// <param name="sheetname">导出的Excel的Sheet的名称。
        /// <para>如果为空，则会取默认名称 Sheet1，而不会取 DataTable 的 Name 属性作为 sheetname。</para>
        /// <para>如果 Sheet 不存在，则会新建，如果 Sheet 已存在，则会被覆盖。</para>
        /// </param>
        /// <param name="isColumnWritten">是否要导入列名</param>
        /// <param name="dateFormat">导出时日期类型的格式</param>
        public static void Export(DataTable datatable, string filename, string sheetname = "Sheet1", bool isColumnWritten = true, string dateFormat = null)
        {
            if (datatable == null) throw new ArgumentNullException(nameof(datatable));
            if (string.IsNullOrEmpty(filename)) throw new ArgumentNullException(nameof(filename));

            if (string.IsNullOrEmpty(sheetname))
            {
                sheetname = "Sheet1";
            }

            IWorkbook workbook = WorkbookExtension.GetOrCreateIWorkbook(filename);
            ISheet sheet = workbook.GetOrCreateISheet(sheetname);

            FillSheet(sheet, datatable, isColumnWritten, dateFormat: dateFormat);

            workbook.Save(filename);
        }

        /// <summary>
        /// 将实体类集合导出到 Excel
        /// </summary>
        /// <param name="data">要导出的数据</param>
        /// <param name="filename">导出的文件名</param>
        /// <param name="sheetname">导出的Excel的Sheet的名称。如果为空，则会取默认名称Sheet1，如果Sheet1不存在，则会新建，如果Sheet1已存在，则会被覆盖。</param>
        /// <param name="isColumnWritten">是否要导入列名</param>
        public static void Export<T>(IEnumerable<T> data, string filename, string sheetname = "Sheet1", bool isColumnWritten = true, string dateFormat = null)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (string.IsNullOrEmpty(filename)) throw new ArgumentNullException(nameof(filename));

            if (sheetname == null || sheetname.Length == 0)
            {
                sheetname = "Sheet1";
            }

            IWorkbook workbook = WorkbookExtension.GetOrCreateIWorkbook(filename);
            ISheet sheet = workbook.GetOrCreateISheet(sheetname);

            FillSheet<T>(sheet, data, isColumnWritten, dateFormat: dateFormat);

            workbook.Save(filename);
        }

        #endregion

        #region 根据模板导出数据到 Excel

        /// <summary>
        /// 根据模板导出数据到 Excel
        /// </summary>
        /// <param name="datatable">要导出的数据</param>
        /// <param name="filename">导出的文件名</param>
        /// <param name="templatePath">模板路径</param>
        /// <param name="sheetName">模板文件中的模板Sheet名称</param>
        /// <param name="startRowIndex">起始数据行索引</param>
        /// <param name="startColumnIndex">起始数据列索引</param>
        public static void ExportByTemplate(DataTable datatable, string filename, string templatePath,
            string sheetName = "Sheet1", int startRowIndex = 0, short startColumnIndex = 0, string dateFormat = null)
        {
            if (datatable == null) throw new ArgumentNullException(nameof(datatable));
            if (string.IsNullOrEmpty(filename)) throw new ArgumentNullException(nameof(filename));
            if (string.IsNullOrEmpty(templatePath)) throw new ArgumentNullException(nameof(templatePath));
            if (string.IsNullOrEmpty(sheetName)) throw new ArgumentNullException(nameof(sheetName));
            if (!File.Exists(templatePath)) throw new FileNotFoundException("模板文件不存在。", templatePath);

            IWorkbook workbook = WorkbookExtension.GetIWorkbook(templatePath);
            ISheet sheet = workbook.GetSheet(sheetName);
            if (sheet == null) throw new KeyNotFoundException("未找到指定名称的Sheet。");

            FillSheet(sheet, datatable, false, startRowIndex, startColumnIndex, dateFormat);

            workbook.Save(filename);
        }

        #endregion

        #region 导入

        /// <summary>
        /// 将Excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="filename">要导入的Excel路径</param>
        /// <param name="sheetname">要导入的sheet名称</param>
        /// <param name="isFirstRowColumnName">第一行是否是DataTable的列名</param>
        public static DataTable Import(string filename, string sheetname, bool isFirstRowColumnName = true)
        {
            if (string.IsNullOrEmpty(filename)) throw new ArgumentNullException(nameof(filename));
            if (!File.Exists(filename)) throw new FileNotFoundException("Excel文件不存在。", filename);
            if (string.IsNullOrEmpty(sheetname)) throw new ArgumentNullException(nameof(sheetname));

            IWorkbook workbook = WorkbookExtension.GetIWorkbook(filename);
            ISheet sheet = workbook.GetSheet(sheetname);
            if (sheet == null) throw new KeyNotFoundException("未找到指定名称的Sheet。");

            return FillDataTable(sheet, isFirstRowColumnName);
        }

        /// <summary>
        /// 将Excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="filename">要导入的Excel路径</param>
        /// <param name="sheetIndex">要导入的sheet索引</param>
        /// <param name="isFirstRowColumnName">第一行是否是DataTable的列名</param>
        public static DataTable Import(string filename, int sheetIndex = 0, bool isFirstRowColumnName = true)
        {
            if (string.IsNullOrEmpty(filename)) throw new ArgumentNullException(nameof(filename));
            if (!File.Exists(filename)) throw new FileNotFoundException("Excel文件不存在。", filename);
            if (sheetIndex < 0) throw new IndexOutOfRangeException(nameof(sheetIndex));

            IWorkbook workbook = WorkbookExtension.GetIWorkbook(filename);

            int sheetCount = workbook.NumberOfSheets;
            if (sheetIndex >= sheetCount) throw new IndexOutOfRangeException(nameof(sheetIndex));
            ISheet sheet = workbook.GetSheetAt(sheetIndex);

            return FillDataTable(sheet, isFirstRowColumnName);
        }

        /// <summary>
        /// 将Excel中的数据导入到指定的类集合中
        /// </summary>
        /// <param name="filename">要导入的Excel路径</param>
        /// <param name="sheetname">要导入的sheet名称</param>
        public static T[] Import<T>(string filename, string sheetname) where T : new()
        {
            if (string.IsNullOrEmpty(filename)) throw new ArgumentNullException(nameof(filename));
            if (!File.Exists(filename)) throw new FileNotFoundException("Excel文件不存在。", filename);
            if (string.IsNullOrEmpty(sheetname)) throw new ArgumentNullException(nameof(sheetname));

            IWorkbook workbook = WorkbookExtension.GetIWorkbook(filename);
            ISheet sheet = workbook.GetSheet(sheetname);
            if (sheet == null) throw new KeyNotFoundException("未找到指定名称的Sheet。");

            return Fill<T>(sheet);
        }

        /// <summary>
        /// 将Excel中的数据导入到指定的类集合中
        /// </summary>
        /// <param name="filename">要导入的Excel路径</param>
        /// <param name="sheetname">要导入的sheet名称</param>
        public static T[] Import<T>(string filename, int sheetIndex) where T : new()
        {
            if (string.IsNullOrEmpty(filename)) throw new ArgumentNullException(nameof(filename));
            if (!File.Exists(filename)) throw new FileNotFoundException("Excel文件不存在。", filename);
            if (sheetIndex < 0) throw new IndexOutOfRangeException(nameof(sheetIndex));

            IWorkbook workbook = WorkbookExtension.GetIWorkbook(filename);

            int sheetCount = workbook.NumberOfSheets;
            if (sheetIndex >= sheetCount) throw new IndexOutOfRangeException(nameof(sheetIndex));
            ISheet sheet = workbook.GetSheetAt(sheetIndex);

            return Fill<T>(sheet);
        }

        #endregion

        #region DataTable 导出填充

        static void FillSheet(ISheet sheet, DataTable datatable, bool isColumnWritten, int rowIndex = 0, int columnIndex = 0, string dateFormat = null)
        {
            if (isColumnWritten == true)
            {
                IRow row = sheet.CreateRow(rowIndex);
                for (int j = 0, k = datatable.Columns.Count; j < k; j++)
                {
                    row.CreateCell(j + columnIndex).SetCellValue(datatable.Columns[j].ColumnName);
                }
                rowIndex += 1;
            }

            ICellStyle dataStyle = sheet.Workbook.GetCellStyleDataFormat(dateFormat);

            for (int i = 0, k = datatable.Rows.Count; i < k; i++)
            {
                IRow row = sheet.CreateRow(i + rowIndex);
                for (int j = 0, h = datatable.Columns.Count; j < h; j++)
                {
                    ICell cell = row.CreateCell(j + columnIndex);

                    cell.SetValue(datatable.Rows[i][j], dataStyle);
                }
            }
        }

        #endregion

        class ColumnInfo
        {
            public DataLabelAttribute Label { get; set; }

            public PropertyInfo PropertyInfo { get; set; }
        }

        static ColumnInfo[] CreateColumnInfo<T>()
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            return properties.Select(p => new ColumnInfo
            {
                Label = p.IsDefined(typeof(DataLabelAttribute), false) ? p.GetCustomAttributes(typeof(DataLabelAttribute), false)[0] as DataLabelAttribute : new DataLabelAttribute { Name = p.Name },
                PropertyInfo = p
            }).Where(m => m.Label.NotMapped == false).OrderBy(m => m.Label.Order).ToArray();
        }

        #region Entity 导出填充

        internal static void FillSheet<T>(ISheet sheet, IEnumerable<T> data, bool isColumnWritten, int rowIndex = 0, int columnIndex = 0, string dateFormat = null)
        {
            ICellStyle dataStyle = sheet.Workbook.GetCellStyleDataFormat(dateFormat);

            ColumnInfo[] columns = CreateColumnInfo<T>();

            if (isColumnWritten == true)
            {
                IRow row = sheet.CreateRow(rowIndex);
                for (int j = 0, k = columns.Length; j < k; j++)
                {
                    row.CreateCell(j + columnIndex).SetCellValue(columns[j].Label.Name);
                }
                rowIndex += 1;
            }

            foreach (T t in data)
            {
                IRow row = sheet.CreateRow(rowIndex);
                for (int j = 0, h = columns.Length; j < h; j++)
                {
                    object value = columns[j].PropertyInfo.GetValue(t, null);
                    ICell cell = row.CreateCell(j + columnIndex);

                    cell.SetValue(value, dataStyle);
                }
                rowIndex++;
            }
        }

        #endregion

        #region 导入填充 DataTable

        static DataTable FillDataTable(ISheet sheet, bool isFirstRowColumnName)
        {
            DataTable dt = new DataTable();

            int firstRowIndex = sheet.FirstRowNum;
            int lastRowIndex = sheet.LastRowNum;

            IRow firstRow = sheet.GetRow(firstRowIndex);
            int firstColumnIndex = firstRow.FirstCellNum;
            int lastColumnIndex = firstRow.LastCellNum;

            if (isFirstRowColumnName)
            {
                FillDataTableColumnName(dt, firstRow, firstColumnIndex, lastColumnIndex);
                firstRowIndex += 1;
            }
            else
            {
                FillDataTableColumnName(dt, firstColumnIndex, lastColumnIndex);
            }

            for (int i = firstRowIndex; i <= lastRowIndex; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row == null) continue;

                if (row.All(cell => cell.ToString().Length == 0)) continue;

                DataRow dataRow = dt.NewRow();
                int dtColumnIndex = 0;
                for (int j = firstColumnIndex; j < lastColumnIndex; j++)
                {
                    ICell cell = row.GetCell(j);

                    if (cell != null)
                    {
                        dataRow[dtColumnIndex] = cell.GetValue();
                    }
                    dtColumnIndex++;
                }

                dt.Rows.Add(dataRow);
            }

            return dt;
        }

        static void FillDataTableColumnName(DataTable dt, IRow firstRow, int firstColumnIndex, int lastColumnIndex)
        {
            for (int i = firstColumnIndex; i < lastColumnIndex; i++)
            {
                ICell cell = firstRow.GetCell(i);

                dt.Columns.Add(new DataColumn(cell.GetValue().ToString()));
            }
        }

        static void FillDataTableColumnName(DataTable dt, int firstColumnIndex, int lastColumnIndex)
        {
            for (int i = firstColumnIndex; i < lastColumnIndex; i++)
            {
                dt.Columns.Add(new DataColumn("Column" + i.ToString()));
            }
        }

        #endregion

        #region 导入填充 Entity

        static T[] Fill<T>(ISheet sheet) where T : new()
        {
            int firstRowIndex = sheet.FirstRowNum;
            int lastRowIndex = sheet.LastRowNum;

            IRow firstRow = sheet.GetRow(firstRowIndex);

            List<(int ColumnIndex, PropertyInfo Property)> list = MapColumnName<T>(firstRow);

            List<T> entities = new List<T>();

            for (int i = firstRowIndex + 1; i <= lastRowIndex; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row == null) continue;

                if (row.All(cell => cell.ToString().Length == 0)) continue;

                T t = new T();

                foreach (var (ColumnIndex, Property) in list)
                {
                    ICell cell = row.GetCell(ColumnIndex);
                    PropertyInfo pInfo = Property;

                    object cellvalue = cell?.GetValue();

                    pInfo.SetValue(t, cellvalue.ChanageType(pInfo.PropertyType), null);
                }

                entities.Add(t);
            }

            return entities.ToArray();
        }

        // 列名和属性映射
        static List<(int ColumnIndex, PropertyInfo Property)> MapColumnName<T>(IRow firstRow)
        {
            ColumnInfo[] columnInfos = CreateColumnInfo<T>();

            int firstColumnIndex = firstRow.FirstCellNum;
            int lastColumnIndex = firstRow.LastCellNum;

            Dictionary<int, PropertyInfo> dict = new Dictionary<int, PropertyInfo>();

            List<(int ColumnIndex, PropertyInfo Property)> list = new List<(int ColumnIndex, PropertyInfo Property)>();

            for (int i = firstColumnIndex; i < lastColumnIndex; i++)
            {
                ICell cell = firstRow.GetCell(i);
                if (cell != null)
                {
                    string name = cell.GetValue().ToString();
                    var pl = columnInfos.FirstOrDefault(m => m.Label.Name == name);
                    if (pl != null)
                    {
                        list.Add((i, pl.PropertyInfo));
                    }
                    else
                    {
                        throw new Exception($"Excel 中列名[{name}]有误。");
                    }
                }
            }

            return list;
        }

        #endregion

    }
}
