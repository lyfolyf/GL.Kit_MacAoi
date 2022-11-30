using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.IO;

namespace GL.NpoiKit
{
    static class WorkbookExtension
    {
        public static ISheet GetOrCreateISheet(this IWorkbook workbook, string sheetName)
        {
            if (string.IsNullOrEmpty(sheetName))
                throw new ArgumentNullException(nameof(sheetName));

            ISheet sheet = workbook.GetSheet(sheetName);
            if (sheet == null)
                sheet = workbook.CreateSheet(sheetName);
            return sheet;
        }

        public static ISheet GetOrCreateISheet(this IWorkbook workbook, int sheetIndex)
        {
            if (sheetIndex < 0)
                throw new IndexOutOfRangeException(nameof(sheetIndex));

            int sheetCount = workbook.NumberOfSheets;

            ISheet sheet;
            if (sheetCount > sheetIndex)
                sheet = workbook.GetSheetAt(sheetIndex);
            else
                sheet = workbook.CreateSheet();
            return sheet;
        }

        /// <summary>
        /// 获取所有工作簿名称
        /// </summary>
        /// <param name="filename"></param>
        public static string[] GetSheetNames(this IWorkbook workbook)
        {
            int count = workbook.NumberOfSheets;

            string[] names = new string[count];
            for (int i = 0; i < count; i++)
            {
                names[i] = workbook.GetSheetName(i);
            }
            return names;
        }

        public static ICellStyle GetCellStyleDataFormat(this IWorkbook workbook, string dataFormat)
        {
            if (dataFormat == null)
                return null;

            ICellStyle dataStyle = workbook.CreateCellStyle();
            IDataFormat format = workbook.CreateDataFormat();
            dataStyle.DataFormat = format.GetFormat(dataFormat);

            return dataStyle;
        }

        public static void Save(this IWorkbook workbook, string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                workbook.Write(fs);
            }
        }

        public static IWorkbook GetOrCreateIWorkbook(string filename)
        {
            if (File.Exists(filename))
            {
                return GetIWorkbook(filename);
            }
            else
            {
                return CreateIWorkbook(filename);
            }
        }

        public static IWorkbook CreateIWorkbook(string filename)
        {
            string extension = Path.GetExtension(filename).ToLower();

            IWorkbook workbook;

            if (extension == ".xlsx")
                workbook = new XSSFWorkbook();
            else if (extension == ".xls")
                workbook = new HSSFWorkbook();
            else
                throw new Exception("无效的扩展名。");

            return workbook;
        }

        public static IWorkbook GetIWorkbook(string filename)
        {
            string extension = Path.GetExtension(filename).ToLower();

            IWorkbook workbook;

            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                if (extension == ".xlsx")
                    workbook = new XSSFWorkbook(fs);
                else if (extension == ".xls")
                    workbook = new HSSFWorkbook(fs);
                else
                    throw new Exception("无效的扩展名。");
            }

            return workbook;
        }
    }
}
