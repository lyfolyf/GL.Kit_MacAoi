using NPOI.SS.UserModel;
using System.Text;
using System.Text.RegularExpressions;

namespace GL.NpoiKit
{
    public class NpoiUtils
    {
        static int lastFormatIndex = -1;
        static string lastFormatString = null;
        static bool cached = false;
        static readonly string syncIsADateFormat = "IsADateFormat";

        static readonly Regex date_ptrn1 = new Regex("^\\[\\$\\-.*?\\]");
        static readonly Regex date_ptrn2 = new Regex("^\\[[a-zA-Z]+\\]");
        static readonly Regex date_ptrn3a = new Regex("[yYmMdDhHsS]");
        static readonly Regex date_ptrn3b = new Regex("^[\\[\\]yYmMdDhHsS\\-T/,. :\"\\\\]+0*[ampAMP/]*$");
        static readonly Regex date_ptrn4 = new Regex("^\\[([hH]+|[mM]+|[sS]+)\\]$");
        static readonly Regex data_ptrn5 = new Regex("[年|月|日|时|分|秒|毫秒|微秒]");

        // DateUtil 中 IsCellDateFormatted 方法有点问题
        // 1. 不支持带中文的日期格式
        // 2. 无法解析自定义的日期格式

        public static bool IsCellDateFormatted(ICell cell)
        {
            if (cell == null) return false;
            bool bDate = false;

            double d = cell.NumericCellValue;
            if (DateUtil.IsValidExcelDate(d))
            {
                ICellStyle style = cell.CellStyle;
                if (style == null)
                    return false;
                int i = style.DataFormat;
                string f = style.GetDataFormatString();
                if (f == null)
                    f = getDataFormatString(i);
                bDate = IsADateFormat(i, f);
            }
            return bDate;
        }

        static bool IsADateFormat(int formatIndex, string formatString)
        {
            lock (syncIsADateFormat)
            {
                if (formatString != null && formatIndex == lastFormatIndex && formatString.Equals(lastFormatString))
                {
                    return cached;
                }

                if (IsInternalDateFormat(formatIndex))
                {
                    lastFormatIndex = formatIndex;
                    lastFormatString = formatString;
                    cached = true;
                    return true;
                }

                if (formatString == null || formatString.Length == 0)
                {
                    lastFormatIndex = formatIndex;
                    lastFormatString = formatString;
                    cached = false;
                    return false;
                }

                string fs = formatString;

                fs = Regex.Replace(fs, ";@", "");
                StringBuilder sb = new StringBuilder(fs.Length);
                for (int i = 0; i < fs.Length; i++)
                {
                    char c = fs[i];
                    if (i < fs.Length - 1)
                    {
                        char nc = fs[i + 1];
                        if (c == '\\')
                        {
                            switch (nc)
                            {
                                case '-':
                                case ',':
                                case '.':
                                case ' ':
                                case '\\':
                                    // skip current '\' and continue to the next char
                                    continue;
                            }
                        }
                        else if (c == ';' && nc == '@')
                        {
                            i++;
                            // skip ";@" duplets
                            continue;
                        }
                    }
                    sb.Append(c);
                }
                fs = sb.ToString();

                if (date_ptrn4.IsMatch(fs))
                {
                    lastFormatIndex = formatIndex;
                    lastFormatString = formatString;
                    cached = true;
                    return true;
                }

                fs = date_ptrn1.Replace(fs, "");

                fs = date_ptrn2.Replace(fs, "");

                fs = data_ptrn5.Replace(fs, "");

                if (fs.IndexOf(';') > 0 && fs.IndexOf(';') < fs.Length - 1)
                {
                    fs = fs.Substring(0, fs.IndexOf(';'));
                }

                if (!date_ptrn3a.Match(fs).Success)
                {
                    return false;
                }

                fs = Regex.Replace(fs, @"""[^""\\]*(?:\\.[^""\\]*)*""", "");

                bool result = date_ptrn3b.IsMatch(fs);
                lastFormatIndex = formatIndex;
                lastFormatString = formatString;
                cached = result;
                return result;
            }
        }

        static bool IsInternalDateFormat(int format)
        {
            bool retval;

            switch (format)
            {
                case 0x0e:
                case 0x0f:
                case 0x10:
                case 0x11:
                case 0x12:
                case 0x13:
                case 0x14:
                case 0x15:
                case 0x16:
                case 0x2d:
                case 0x2e:
                case 0x2f:
                    retval = true;
                    break;
                default:
                    retval = false;
                    break;
            }
            return retval;
        }

        // 这个方法用来解决自定义的日期格式
        // formatIndex 值对应的格式是实测得到的，不保证所有环境下可用
        static string getDataFormatString(int formatIndex)
        {
            if (formatIndex == 58)
                return "m\"月\"d\"日\"";

            return null;
        }
    }
}
