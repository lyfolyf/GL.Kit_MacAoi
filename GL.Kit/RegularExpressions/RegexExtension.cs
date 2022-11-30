using System.Collections.Generic;

namespace System.Text.RegularExpressions
{
    public static class RegexExtension
    {
        public static string[] ToArray(this MatchCollection matchs)
        {
            string[] result = new string[matchs.Count];
            int i = 0;
            foreach (Match match in matchs)
            {
                result[i++] = match.Value;
            }
            return result;
        }

        public static List<string> ToList(this MatchCollection matchs)
        {
            List<string> result = new List<string>(matchs.Count);
            foreach (Match match in matchs)
            {
                result.Add(match.Value);
            }
            return result;
        }

        /// <summary>
        /// 在指定的输入字符串中搜索 pattern 参数中指定的正则表达式的匹配项
        /// </summary>
        /// <param name="str">要搜索匹配项的字符串</param>
        /// <param name="pattern">要匹配的正则表达式模式</param>
        /// <param name="options"><see cref="System.Text.RegularExpressions.RegexOptions"/>枚举值的按位或组合</param>
        public static string Match(this string str, string pattern, RegexOptions options = RegexOptions.None)
        {
            return Regex.Match(str, pattern, options).Value;
        }

        /// <summary>
        /// 在指定的输入字符串中搜索 pattern 参数中指定的正则表达式的所有匹配项
        /// </summary>
        /// <param name="str">要搜索匹配项的字符串</param>
        /// <param name="pattern">要匹配的正则表达式模式</param>
        /// <param name="options"><see cref="System.Text.RegularExpressions.RegexOptions"/>枚举值的按位或组合</param>
        public static string[] Matches(this string str, string pattern, RegexOptions options = RegexOptions.None)
        {
            return Regex.Matches(str, pattern, options).ToArray();
        }

        /// <summary>
        /// 返回一个值，该值指示指定的字符串是否满足正则表达式
        /// </summary>
        /// <param name="str">要搜索匹配项的字符串</param>
        /// <param name="pattern">要匹配的正则表达式模式</param>
        /// <param name="options"><see cref="System.Text.RegularExpressions.RegexOptions"/>枚举值的按位或组合</param>
        public static bool IsMatch(this string str, string pattern, RegexOptions options = RegexOptions.None)
        {
            return Regex.IsMatch(str, pattern, options);
        }

        /// <summary>
        /// 在指定的输入字符串内，使用指定的替换字符串替换与指定正则表达式匹配的所有字符串
        /// </summary>
        /// <param name="str">要搜索匹配项的字符串</param>
        /// <param name="pattern">要匹配的正则表达式模式</param>
        /// <param name="replacement">替换字符串</param>
        /// <param name="options"><see cref="System.Text.RegularExpressions.RegexOptions"/>枚举值的按位或组合</param>
        public static string RegReplace(this string str, string pattern, string replacement, RegexOptions options = RegexOptions.None)
        {
            return Regex.Replace(str, pattern, replacement, options);
        }

        /// <summary>
        /// 划分字符串
        /// </summary>
        /// <param name="str">要划分的字符串</param>
        /// <param name="pattern">要匹配的正则表达式模式</param>
        /// <param name="options"><see cref="System.Text.RegularExpressions.RegexOptions"/>枚举值的按位或组合</param>
        public static string[] RegSplit(this string str, string pattern, RegexOptions options = RegexOptions.None)
        {
            return Regex.Split(str, pattern, options);
        }

        /// <summary>
        /// 通过替换为转义码来转义最小的元字符集（\、*、+、?、|、{、[、(、)、^、$、.、# 和空白）
        /// </summary>
        public static string Escape(this string str)
        {
            return Regex.Escape(str);
        }

        /// <summary>
        /// 取消转义输入字符串中的任何转义字符
        /// </summary>
        public static string Unescape(this string str)
        {
            return Regex.Unescape(str);
        }

    }
}
