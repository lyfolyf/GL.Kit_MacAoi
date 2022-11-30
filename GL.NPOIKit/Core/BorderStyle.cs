
namespace GL.NpoiKit
{
    /// <summary>
    /// 边框样式
    /// </summary>
    public enum CellBorderStyle
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,
        /// <summary>
        /// 实线
        /// </summary>
        Thin = 1,
        /// <summary>
        /// 中实线
        /// </summary>
        Medium = 2,
        /// <summary>
        /// 虚线
        /// </summary>
        Dashed = 3,
        /// <summary>
        /// 点虚线
        /// </summary>
        Dotted = 4,
        /// <summary>
        /// 粗实线
        /// </summary>
        Thick = 5,
        /// <summary>
        /// 双实线
        /// </summary>
        Double = 6,
        /// <summary>
        /// 
        /// </summary>
        Hair = 7,
        /// <summary>
        /// 中虚线
        /// </summary>
        MediumDashed = 8,
        /// <summary>
        /// 点划线
        /// </summary>
        DashDot = 9,
        /// <summary>
        /// 中点划线
        /// </summary>
        MediumDashDot = 10,
        /// <summary>
        /// 双点划线
        /// </summary>
        DashDotDot = 11,
        /// <summary>
        /// 中双点划线
        /// </summary>
        MediumDashDotDot = 12,
        /// <summary>
        /// 斜点划线
        /// </summary>
        SlantedDashDot = 13,
    }

    /// <summary>
    /// 边框
    /// </summary>
    public struct Border
    {
        /// <summary>
        /// 边框样式
        /// </summary>
        public CellBorderStyle BorderStyle;

        /// <summary>
        /// 边框颜色
        /// </summary>
        public NpoiColor BorderColor;
    }

    /// <summary>
    /// 四边框样式
    /// </summary>
    public class FourBorders
    {
        /// <summary>
        /// 下边框
        /// </summary>
        public Border BorderBottom { get; set; }
        /// <summary>
        /// 左边框
        /// </summary>
        public Border BorderLeft { get; set; }
        /// <summary>
        /// 右边框
        /// </summary>
        public Border BorderRight { get; set; }
        /// <summary>
        /// 上边框
        /// </summary>
        public Border BorderTop { get; set; }

        public Border All
        {
            set
            {
                BorderBottom = value;
                BorderLeft = value;
                BorderRight = value;
                BorderTop = value;
            }
        }
    }
}
