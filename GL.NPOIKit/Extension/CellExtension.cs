using NPOI.SS.Formula.Eval;
using NPOI.SS.UserModel;
using System;

namespace GL.NpoiKit
{
    static class CellExtension
    {
        public static object GetValue(this ICell cell)
        {
            if (cell.CellType == CellType.Formula)
            {
                return GetValue(cell, cell.CachedFormulaResultType);
            }
            else
                return GetValue(cell, cell.CellType);
        }

        static object GetValue(ICell cell, CellType cellType)
        {
            if (cellType == CellType.String)
                return cell.StringCellValue;
            else if (cellType == CellType.Numeric)
            {
                if (NpoiUtils.IsCellDateFormatted(cell))
                    return cell.DateCellValue;
                else
                    return cell.NumericCellValue;
            }
            else if (cellType == CellType.Boolean)
                return cell.BooleanCellValue;
            else if (cellType == CellType.Error)
                return ErrorEval.GetText(cell.ErrorCellValue);
            else
                return string.Empty;
        }

        public static void SetValue(this ICell cell, object value, ICellStyle dataStyle = null)
        {
            if (value == null)
            {
                cell.SetCellValue("");
                return;
            }

            switch (value)
            {
                case string stringValue:
                    cell.SetCellValue(stringValue); break;
                case char charValue:
                    cell.SetCellValue(charValue); break;

                case DateTime datetimeValue:
                    cell.CellStyle = dataStyle;
                    cell.SetCellValue(datetimeValue);
                    break;
                case bool boolValue: cell.SetCellValue(boolValue); break;

                case byte byteValue:
                    cell.SetCellValue(Convert.ToDouble(byteValue)); break;
                case short shortValue:
                    cell.SetCellValue(Convert.ToDouble(shortValue)); break;
                case int intValue:
                    cell.SetCellValue(Convert.ToDouble(intValue)); break;
                case long longValue:
                    cell.SetCellValue(Convert.ToDouble(longValue)); break;
                case float floatValue:
                    cell.SetCellValue(Convert.ToDouble(floatValue)); break;
                case double doubleValue:
                    cell.SetCellValue(doubleValue); break;
                case decimal decimalValue:
                    cell.SetCellValue(Convert.ToDouble(decimalValue)); break;

                case sbyte sbyteValue:
                    cell.SetCellValue(Convert.ToDouble(sbyteValue)); break;
                case ushort ushortValue:
                    cell.SetCellValue(Convert.ToDouble(ushortValue)); break;
                case uint uintValue:
                    cell.SetCellValue(Convert.ToDouble(uintValue)); break;
                case ulong ulongValue:
                    cell.SetCellValue(Convert.ToDouble(ulongValue)); break;
                default:
                    cell.SetCellValue(value.ToString()); break;
            }
        }
    }
}
