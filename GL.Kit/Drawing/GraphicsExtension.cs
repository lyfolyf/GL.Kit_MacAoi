using System.Drawing.Drawing2D;

namespace System.Drawing
{
    public static class GraphicsExtension
    {
        /// <summary>
        /// 圆角矩形
        /// </summary>
        /// <param name="rc"></param>
        /// <param name="r">圆角半径</param>
        public static GraphicsPath GetGraphicsPath(this Rectangle rc, int r)
        {
            int x = rc.X, y = rc.Y, w = rc.Width, h = rc.Height;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(x, y, r, r, 180, 90);
            path.AddArc(x + w - r, y, r, r, 270, 90);
            path.AddArc(x + w - r, y + h - r, r, r, 0, 90);
            path.AddArc(x, y + h - r, r, r, 90, 90);
            path.CloseFigure();
            return path;
        }

        #region FillRectangle

        /// <summary>
        /// 填充矩形
        /// </summary>
        public static void FillRectangle(this Graphics g, Color color, Rectangle rect)
        {
            using (SolidBrush brush = new SolidBrush(color))
            {
                g.FillRectangle(brush, rect);
            }
        }

        /// <summary>
        /// 填充矩形
        /// </summary>
        public static void FillRectangle(this Graphics g, Color color, RectangleF rect)
        {
            using (SolidBrush brush = new SolidBrush(color))
            {
                g.FillRectangle(brush, rect);
            }
        }

        /// <summary>
        /// 填充矩形
        /// </summary>
        public static void FillRectangle(this Graphics g, Color color, int x, int y, int width, int height)
        {
            using (SolidBrush brush = new SolidBrush(color))
            {
                g.FillRectangle(brush, x, y, width, height);
            }
        }

        /// <summary>
        /// 填充矩形
        /// </summary>
        public static void FillRectangle(this Graphics g, Color color, float x, float y, float width, float height)
        {
            using (SolidBrush brush = new SolidBrush(color))
            {
                g.FillRectangle(brush, x, y, width, height);
            }
        }

        /// <summary>
        /// 填充椭圆内部
        /// </summary>
        /// <remarks>椭圆的长短半径定了，椭圆也就定了</remarks>
        public static void FillEllipse(this Graphics g, Color color, RectangleF rect)
        {
            using (SolidBrush sb = new SolidBrush(color))
            {
                g.FillEllipse(sb, rect);
            }
        }

        /// <summary>
        /// 填充椭圆内部
        /// </summary>
        /// <remarks>椭圆的长短半径定了，椭圆也就定了</remarks>
        public static void FillEllipse(this Graphics g, Color color, float x, float y, float width, float height)
        {
            using (SolidBrush sb = new SolidBrush(color))
            {
                g.FillEllipse(sb, x, y, width, height);
            }
        }

        /// <summary>
        /// 填充 GraphicsPath 的内部
        /// </summary>
        public static void FillPath(this Graphics g, Color color, GraphicsPath path)
        {
            using (SolidBrush brush = new SolidBrush(color))
            {
                g.FillPath(brush, path);
            }
        }

        #endregion

        #region DrawRectangle

        /// <summary>
        /// 绘制矩形
        /// </summary>
        public static void DrawRectangle(this Graphics g, Color color, Rectangle rect, int borderWidth = 1)
        {
            using (Pen pen = new Pen(color, borderWidth))
            {
                g.DrawRectangle(pen, rect);
            }
        }

        /// <summary>
        /// 绘制矩形
        /// </summary>
        public static void DrawRectangle(this Graphics g, Color color, float x, float y, float width, float height, int borderWidth = 1)
        {
            using (Pen pen = new Pen(color, borderWidth))
            {
                g.DrawRectangle(pen, x, y, width, height);
            }
        }

        /// <summary>
        /// 绘制矩形
        /// </summary>
        public static void DrawRectangle(this Graphics g, Color color, int x, int y, int width, int height, int borderWidth = 1)
        {
            using (Pen pen = new Pen(color, borderWidth))
            {
                g.DrawRectangle(pen, x, y, width, height);
            }
        }

        /// <summary>
        /// 绘制 GraphicsPath
        /// </summary>
        public static void DrawPath(this Graphics g, Color color, GraphicsPath path, int borderWidth = 1)
        {
            using (Pen pen = new Pen(color, borderWidth))
            {
                g.DrawPath(pen, path);
            }
        }

        #endregion

        #region DrawString

        /// <summary>
        /// 绘制字符串
        /// </summary>
        public static void DrawString(this Graphics g, string s, Font font, Color color, float x, float y)
        {
            using (SolidBrush brush = new SolidBrush(color))
            {
                g.DrawString(s, font, brush, x, y);
            }
        }

        /// <summary>
        /// 绘制字符串
        /// </summary>
        public static void DrawString(this Graphics g, string s, Font font, Color color, PointF point)
        {
            using (SolidBrush brush = new SolidBrush(color))
            {
                g.DrawString(s, font, brush, point);
            }
        }

        #endregion



        #region 高质量

        /// <summary>
        /// 设置 GDI 高质量模式抗锯齿
        /// </summary>
        public static void SetHighQuality(this Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.HighQuality;
        }

        /// <summary>
        /// 设置 GDI 默认质量
        /// </summary>
        public static void SetDefaultQuality(this Graphics g)
        {
            g.SmoothingMode = SmoothingMode.Default;
            g.InterpolationMode = InterpolationMode.Default;
            g.CompositingQuality = CompositingQuality.Default;
        }

        #endregion
    }
}
