using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;

namespace GL.EmguCVKit
{
    public static class EmguCVUtils
    {
        public static Bitmap CvCopy(this Bitmap bmp)
        {
            using (Image<Gray, byte> grayImage = bmp.ToImage<Gray, byte>())
            {
                using (var copy = grayImage.Copy())
                {
                    return copy.ToBitmap();
                }
            }
        }

        public static Bitmap CvResize(this Bitmap bmp, bool isGrey, decimal rate)
        {
            if (isGrey)
            {
                using (Image<Gray, byte> grayImage = bmp.ToImage<Gray, byte>())
                {
                    using (Image<Gray, byte> newImage = grayImage.Resize((int)(bmp.Width * rate), (int)(bmp.Height * rate), Emgu.CV.CvEnum.Inter.Cubic))
                    {
                        return newImage.ToBitmap();
                    }
                }
            }
            else
            {
                using (Image<Bgr, byte> grayImage = bmp.ToImage<Bgr, byte>())
                {
                    using (Image<Bgr, byte> newImage = grayImage.Resize((int)(bmp.Width * rate), (int)(bmp.Height * rate), Emgu.CV.CvEnum.Inter.Cubic))
                    {
                        return newImage.ToBitmap();
                    }
                }
            }
        }

        public static void CvSave(this Bitmap bmp, bool isGrey, string filename)
        {
            if (isGrey)
            {
                using (Image<Gray, byte> grayImage = bmp.ToImage<Gray, byte>())
                {
                    grayImage.Save(filename);
                }
            }
            else
            {
                using (Image<Bgr, byte> grayImage = bmp.ToImage<Bgr, byte>())
                {
                    grayImage.Save(filename);
                }
            }
        }
    }
}
