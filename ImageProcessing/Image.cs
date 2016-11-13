using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageProcessing
{
    public enum ColorChoice
    {
        Red = 2,
        Green = 1,
        Blue = 0
    }
    public class Image
    {
        private const double RedSaturation = 0.299;
        private const double GreenSaturation = 0.587;
        private const double BlueSaturation = 0.114;
        private const float FloatValue255 = 255.0f;
        private string filename;
        private Bitmap bmp;
        public Image(string filename)
        {
            this.filename = filename;
            bmp = new Bitmap(this.filename);
            PixelSize = System.Drawing.Image.GetPixelFormatSize(bmp.PixelFormat) / 8;
            rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
        }

        public Bitmap GetCopy()
        {
            return (Bitmap)this.bmp.Clone();
        }

        private int PixelSize;
        private Rectangle rect;

        public void ToGrayScale()
        {
            if (bmp.PixelFormat == PixelFormat.Format24bppRgb && PixelSize == 3)
            {
                BitmapData data = bmp.LockBits(rect, ImageLockMode.WriteOnly, bmp.PixelFormat);
                byte[] bytes = data.GetBytesFromImage();

                double red;
                double green;
                double blue;
                byte gray;

                int i = 0;
                while (i < bytes.Length)
                {
                    blue = bytes[i];
                    green = bytes[i + 1];
                    red = bytes[i + 2];
                    gray = (byte)(RedSaturation * red + GreenSaturation * green + BlueSaturation * blue);
                    bytes[i] = bytes[i + 1] = bytes[i + 2] = gray;
                    i += 3;
                }

                data.ReturnBytes(bytes);
                bmp.UnlockBits(data);
            }
        }

        public void ToBlackOrWhiteScale(byte treshold)
        {
            if (bmp.PixelFormat == PixelFormat.Format24bppRgb && PixelSize == 3)
            {
                BitmapData data = bmp.LockBits(rect, ImageLockMode.WriteOnly, bmp.PixelFormat);
                byte[] bytes = data.GetBytesFromImage();

                int i = 0;
                while (i < bytes.Length)
                {
                    bytes[i] = bytes[i + 1] = bytes[i + 2] =
                        (byte)(
                        (
                            RedSaturation * bytes[i + 2] +
                            GreenSaturation * bytes[i + 1] +
                            BlueSaturation * bytes[i]
                        ) > treshold ? 255 : 0);
                    i += 3;
                }

                data.ReturnBytes(bytes);
                bmp.UnlockBits(data);
            }
        }

        public void AdjustContrast(float value)
        {
            if (bmp.PixelFormat == PixelFormat.Format24bppRgb && PixelSize == 3)
            {
                BitmapData data = bmp.LockBits(rect, ImageLockMode.WriteOnly, bmp.PixelFormat);
                byte[] bytes = data.GetBytesFromImage();

                int i = 0;
                float red, green, blue;                
                value = (100.0f + value) / 100.0f;
                value *= value;
                while (i < bytes.Length)
                {
                    red = bytes[i + 2]/ FloatValue255;
                    green = bytes[i + 1]/ FloatValue255;
                    blue = bytes[i]/ FloatValue255;
                   
                    red = (((red - 0.5f) * value) + 0.5f) * FloatValue255;
                    green = (((green - 0.5f) * value) + 0.5f) * FloatValue255;
                    blue = (((blue - 0.5f) * value) + 0.5f) * FloatValue255;
                                       
                    bytes[i + 2] = (byte)(red > 255 ? 255 : red < 0 ? 0 : red);                                       
                    bytes[i + 1] = (byte)(green > 255 ? 255 : green < 0 ? 0 : green);                                     
                    bytes[i] = (byte)(blue > 255 ? 255 : blue < 0 ? 0 : blue);                                                                       

                    i += 3;
                }

                data.ReturnBytes(bytes);
                bmp.UnlockBits(data);
            }
        }

        public void RemoveColor(ColorChoice color)
        {
            if (bmp.PixelFormat == PixelFormat.Format24bppRgb && PixelSize == 3)
            {
                BitmapData data = bmp.LockBits(rect, ImageLockMode.WriteOnly, bmp.PixelFormat);                
                byte[] bytes = data.GetBytesFromImage();
                int i = 0;
                int c = (int)color;
                while (i < bytes.Length)
                {
                    bytes[i + c] = 0;
                    i += 3;
                }
                data.ReturnBytes(bytes);                                                        

                bmp.UnlockBits(data);
            }
        }

        public void MinusValue(ColorChoice color, byte value)
        {
            if (bmp.PixelFormat == PixelFormat.Format24bppRgb && PixelSize == 3)
            {
                BitmapData data = bmp.LockBits(rect, ImageLockMode.WriteOnly, bmp.PixelFormat);
                byte[] bytes = data.GetBytesFromImage();
                int i = 0;
                byte zero = 0;
                int c = (int)color;
                while (i < bytes.Length)
                {
                    bytes[i + c] = (byte)((bytes[i+c]-value)> zero ? bytes[i+c] - value : zero);
                    bytes[i+c] = (byte)((bytes[i+c] - value) > zero ? bytes[i+c] - value : zero);
                    bytes[i+c] = (byte)((bytes[i+c] - value) > zero ? bytes[i+c] - value : zero);
                    i += 3;
                }
                data.ReturnBytes(bytes);

                bmp.UnlockBits(data);
            }
        }



        public void Save(string filename)
        {
            this.bmp.Save(filename);
        }
    }
}
