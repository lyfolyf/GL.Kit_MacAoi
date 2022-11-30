
namespace GL.NpoiKit
{
    public class NpoiStyle
    {
        /// <summary>
        /// 背景色
        /// </summary>
        public NpoiColor BackgroundColor = NpoiColor.White;
        /// <summary>
        /// 水平对齐
        /// </summary>
        public HorizontalAlignment HorizontalAlignment = HorizontalAlignment.Left;
        /// <summary>
        /// 垂直对齐
        /// </summary>
        public VerticalAlignment VerticalAlignment = VerticalAlignment.Center;

        /// <summary>
        /// 加粗
        /// </summary>
        public bool Bold = false;
        /// <summary>
        /// 删除线
        /// </summary>
        public bool Strikeout = false;
        /// <summary>
        /// 斜体
        /// </summary>
        public bool Italic = false;
        /// <summary>
        /// 自动换行
        /// </summary>
        public bool WrapText = false;
        /// <summary>
        /// 字体
        /// </summary>
        public string FontName = "宋体";
        /// <summary>
        /// 字号
        /// </summary>
        public short FontSize = 11;
        /// <summary>
        /// 字体颜色
        /// </summary>
        public NpoiColor FontColor = NpoiColor.Black;

        public FourBorders FourBorders = new FourBorders();
    }
}
