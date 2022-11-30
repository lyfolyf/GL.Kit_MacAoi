using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace System.Windows.Forms
{
    /// <summary>
    /// 文本类控件方法
    /// </summary>
    public class TextControlUtils
    {
        /// <summary>
        /// 根据字体和控件宽度，计算字符串在哪里换行
        /// </summary>
        public static List<string> WordWrap(string line, Graphics g, Font font, int maxWidth)
        {
            float totalTextWidth = g.MeasureString(line, font).Width;

            if (totalTextWidth <= maxWidth)
            {
                return new List<string> { line };
            }

            List<string> newLines = new List<string>();

            // 单词不切分
            StringBuilder wordBuilder = new StringBuilder();
            StringBuilder lineBuilder = new StringBuilder();

            foreach (char c in line)
            {
                if (c < 255 && c != ' ')
                {
                    wordBuilder.Append(c);
                    continue;
                }
                else
                {
                    float curLineWidth;

                    if (wordBuilder.Length > 0)
                    {
                        string word = wordBuilder.ToString();

                        // 如果一个 word 的长度就超出了单元格长度，就会被截断
                        // 这个 bug 暂时不考虑
                        curLineWidth = g.MeasureString(lineBuilder.ToString() + word, font).Width;

                        if (curLineWidth >= maxWidth)
                        {
                            newLines.Add(lineBuilder.ToString());
                            lineBuilder.Clear();

                            lineBuilder.Append(word);
                            wordBuilder.Clear();
                        }
                        else
                        {
                            lineBuilder.Append(word);
                            wordBuilder.Clear();
                        }
                    }

                    curLineWidth = g.MeasureString(lineBuilder.ToString() + c, font).Width;
                    if (curLineWidth >= maxWidth)
                    {
                        newLines.Add(lineBuilder.ToString());

                        lineBuilder.Clear();
                        lineBuilder.Append(c);
                    }
                    else
                    {
                        lineBuilder.Append(c);
                    }
                }
            }

            if (wordBuilder.Length > 0)
            {
                string word = wordBuilder.ToString();

                float curLineWidth = g.MeasureString(lineBuilder.ToString() + word, font).Width;

                if (curLineWidth >= maxWidth)
                {
                    newLines.Add(lineBuilder.ToString());
                    lineBuilder.Clear();

                    lineBuilder.Append(word);
                    wordBuilder.Clear();
                }
                else
                {
                    lineBuilder.Append(word);
                    wordBuilder.Clear();
                }
            }

            if (lineBuilder.Length > 0)
                newLines.Add(lineBuilder.ToString());

            return newLines;
        }
    }
}
