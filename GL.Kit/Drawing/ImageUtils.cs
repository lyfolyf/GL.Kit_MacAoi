using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Drawing
{
    public static class ImageUtils
    {
        /// <summary>
        /// 调整大小
        /// </summary>
        public static Bitmap Resize(Bitmap srcBmp, int newWidth, int newHeight)
        {
            Bitmap dstBmp = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb);

            Graphics g = Graphics.FromImage(dstBmp);

            // 插值算法的质量
            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality;

            g.DrawImage(srcBmp, new Rectangle(0, 0, newWidth, newHeight), new Rectangle(0, 0, srcBmp.Width, srcBmp.Height), GraphicsUnit.Pixel);
            g.Dispose();

            return dstBmp;
        }

        /// <summary>
        /// 缩放
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="rate">比率</param>
        public static Bitmap Zoom(Bitmap bmp, decimal rate)
        {
            int newWidth = (int)(bmp.Width * rate);
            int newHeight = (int)(bmp.Height * rate);

            return Resize(bmp, newWidth, newHeight);
        }

        /// <summary>
        /// 截屏
        /// <para>由于缩放比的问题，1920 * 1080 的屏幕大小，自动获取时只有 1536 * 864</para>
        /// <para>所以没有做自动获取而是手动设定</para>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static Bitmap ScreenCapture(int x, int y, int width, int height)
        {
            Rectangle rect = new Rectangle(0, 0, width, height);
            Bitmap bmp = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(x, y, 0, 0, rect.Size);
                g.DrawImage(bmp, 0, 0, rect, GraphicsUnit.Pixel);
            }

            return bmp;
        }

        #region Bitmap 转 Bytes

        // BitmapToBytes_2 转换的结果要比 BitmapToBytes 大一些，应该是多了一些 Bitmap 信息
        // 两个方法转换成的 byte[] 都可以再次转换为 Bitmap
        // BitmapToBytes_2 要比 BitmapToBytes 慢一倍

        public static byte[] BitmapToBytes_2(Bitmap bmp)
        {
            if (bmp == null) return null;

            using (var stream = new MemoryStream())
            {
                bmp.Save(stream, ImageFormat.Bmp);
                return stream.ToArray();
            }
        }

        public static byte[] BitmapToBytes(Bitmap bmp)
        {
            if (bmp == null) return null;

            BitmapData bmpdata = null;

            try
            {
                bmpdata = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);

                int numbytes = bmpdata.Stride * bmp.Height;
                byte[] bytedata = new byte[numbytes];
                IntPtr ptr = bmpdata.Scan0;

                Marshal.Copy(ptr, bytedata, 0, numbytes);

                return bytedata;
            }
            finally
            {
                bmp.UnlockBits(bmpdata);
            }
        }

        public static Bitmap BytesToBitmap(bool isGrey, int width, int height, byte[] buffer)
        {
            Bitmap bitmap = new Bitmap(width, height, isGrey ? PixelFormat.Format8bppIndexed : PixelFormat.Format24bppRgb);
            if (isGrey)
            {
                // 这里必须这么写，
                ColorPalette cp = bitmap.Palette;
                for (int i = 0; i < 256; i++)
                {
                    cp.Entries[i] = Color.FromArgb(i, i, i);
                }
                bitmap.Palette = cp;
            }

            BitmapData bmpData = null;
            try
            {
                bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);

                IntPtr src = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
                Kernel32Api.CopyMemory(bmpData.Scan0, src, (uint)(bmpData.Stride * bitmap.Height));
            }
            finally
            {
                bitmap.UnlockBits(bmpData);
            }

            return bitmap;
        }

        #endregion

        #region IntPtr 转 Bitmap

        /// <summary>
        /// 指针转 Bitmap
        /// </summary>
        public static Bitmap IntPtrToBitmap(bool isGrey, int width, int height, IntPtr scan0)
        {
            Bitmap bmp;
            if (isGrey)
            {
                bmp = new Bitmap(width, height, width, PixelFormat.Format8bppIndexed, scan0);

                // 这里必须这么写，
                ColorPalette cp = bmp.Palette;
                for (int i = 0; i < 256; i++)
                {
                    cp.Entries[i] = Color.FromArgb(i, i, i);
                }
                bmp.Palette = cp;
            }
            else
            {
                bmp = new Bitmap(width, height, width * 3, PixelFormat.Format24bppRgb, scan0);
            }

            return bmp;
        }

        /// <summary>
        /// 指针转 Bitmap（深拷贝）
        /// </summary>
        public static Bitmap DeepCopyIntPtrToBitmap(bool isGrey, int width, int height, IntPtr imagebuf)
        {
            Bitmap bitmap = new Bitmap(width, height, isGrey ? PixelFormat.Format8bppIndexed : PixelFormat.Format24bppRgb);
            if (isGrey)
            {
                ColorPalette cp = bitmap.Palette;
                for (int i = 0; i < 256; i++)
                {
                    cp.Entries[i] = Color.FromArgb(i, i, i);
                }
                bitmap.Palette = cp;
            }

            BitmapData bmpData = null;
            try
            {
                bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);

                Kernel32Api.CopyMemory(bmpData.Scan0, imagebuf, (uint)(bmpData.Stride * bitmap.Height));
            }
            finally
            {
                bitmap.UnlockBits(bmpData);
            }

            return bitmap;
        }

        #endregion

        #region Bitmap 转 Base64String

        public static string BitmapToBase64String(Bitmap bmp)
        {
            if (bmp == null) return null;

            byte[] buffer = BitmapToBytes(bmp);

            return Convert.ToBase64String(buffer);
        }

        public static Bitmap Base64StringToBitmap(bool isGrey, int width, int height, string base64Str)
        {
            if (base64Str == null) throw new ArgumentNullException();

            byte[] buffer = Convert.FromBase64String(base64Str);
            IntPtr src = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);

            return IntPtrToBitmap(isGrey, width, height, src);
        }

        #endregion

        /// <summary>
        /// 深度克隆
        /// </summary>
        public static Bitmap DeepClone(this Bitmap bmp)
        {
            PixelFormat pixelFormat = bmp.PixelFormat;

            Bitmap copy = new Bitmap(bmp.Width, bmp.Height, pixelFormat);
            if (pixelFormat == PixelFormat.Format8bppIndexed)
            {
                ColorPalette cp = copy.Palette;
                for (int i = 0; i < 256; i++)
                {
                    cp.Entries[i] = Color.FromArgb(i, i, i);
                }
                copy.Palette = cp;
            }

            BitmapData bmpData = null;
            BitmapData copyData = null;
            try
            {
                bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, pixelFormat);
                copyData = copy.LockBits(new Rectangle(0, 0, copy.Width, copy.Height), ImageLockMode.ReadWrite, pixelFormat);

                Kernel32Api.CopyMemory(copyData.Scan0, bmpData.Scan0, (uint)(bmpData.Stride * bmp.Height));
            }
            finally
            {
                bmp.UnlockBits(bmpData);
                copy.UnlockBits(copyData);
            }

            return copy;
        }

        public static ImageFormat ToImageFormat(string strFormat)
        {
            if (strFormat.Equals("Bmp", StringComparison.OrdinalIgnoreCase))
            {
                return ImageFormat.Bmp;
            }
            else if (strFormat.Equals("Jpeg", StringComparison.OrdinalIgnoreCase)
                || strFormat.Equals("Jpg", StringComparison.OrdinalIgnoreCase))
            {
                return ImageFormat.Jpeg;
            }
            else if (strFormat.Equals("Png", StringComparison.OrdinalIgnoreCase))
            {
                return ImageFormat.Png;
            }
            else if (strFormat.Equals("Gif", StringComparison.OrdinalIgnoreCase))
            {
                return ImageFormat.Gif;
            }
            else if (strFormat.Equals("Icon", StringComparison.OrdinalIgnoreCase))
            {
                return ImageFormat.Icon;
            }
            else if (strFormat.Equals("Tiff", StringComparison.OrdinalIgnoreCase))
            {
                return ImageFormat.Tiff;
            }
            else if (strFormat.Equals("Wmf", StringComparison.OrdinalIgnoreCase))
            {
                return ImageFormat.Wmf;
            }
            else if (strFormat.Equals("Emf", StringComparison.OrdinalIgnoreCase))
            {
                return ImageFormat.Emf;
            }
            else if (strFormat.Equals("Exif", StringComparison.OrdinalIgnoreCase))
            {
                return ImageFormat.Exif;
            }
            else if (strFormat.Equals("MemoryBMP", StringComparison.OrdinalIgnoreCase))
            {
                return ImageFormat.MemoryBmp;
            }

            throw new Exception("无效的图像格式");
        }

        public static Image LoadFromFile(string imagepath)
        {
            byte[] buffer;
            using (FileStream fs = new FileStream(imagepath, FileMode.Open))
            {
                buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();
            }

            MemoryStream ms = new MemoryStream(buffer);
            Image img = Image.FromStream(ms);
            return img;
        }
    }

    //add by LuoDian @ 20211126 用于保存图像的时候，做图像压缩
    public class ImageUtilsNotStatic
    {
        public ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (var item in codecs)
            {
                if (item.FormatID == format.Guid)
                    return item;
            }
            return null;
        }
    }
}
