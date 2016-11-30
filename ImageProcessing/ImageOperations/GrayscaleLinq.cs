using ImageProcessing.Enums;
using System;                     
using System.Drawing;
using System.Drawing.Imaging;  

namespace ImageProcessing.ImageOperations
{
    public static class GrayscaleLinq
    {
        public static Bitmap AsGrayscale(this Bitmap bmp)
        {
            Bitmap copy = (Bitmap)bmp.Clone();
            copy.Grayscale();
            return copy;
        }

        public static void Grayscale(this Bitmap bmp)
        {
            try
            {
                var pixelSize = System.Drawing.Image.GetPixelFormatSize(bmp.PixelFormat) / 8;
                if (bmp.PixelFormat == PixelFormat.Format24bppRgb && pixelSize == 3)
                {
                    var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                    var data = bmp.LockBits(rect, ImageLockMode.WriteOnly, bmp.PixelFormat);
                    byte[] bytes = data.GetBytesFromImage();

                    double red, green, blue;
                    byte gray;

                    int i = 0;
                    while (i < bytes.Length)
                    {
                        blue = bytes[i + ColorShift.Blue];
                        green = bytes[i + ColorShift.Green];
                        red = bytes[i + ColorShift.Red];
                        gray = (byte)(ColorSaturation.Red * red + ColorSaturation.Green * green + ColorSaturation.Blue * blue);
                        bytes[i + ColorShift.Blue] = bytes[i + ColorShift.Green] = bytes[i + ColorShift.Red] = gray;
                        i += pixelSize;
                    }
                    data.ReturnBytes(bytes);
                    bmp.UnlockBits(data);
                }                            
                else
                {
                    throw new BitmapOperationException(nameof(Grayscale));
                }

            }
            catch (Exception ex)
            {
                throw new BitmapOperationException("Processing Bitmap to grayscale has failed.", ex, nameof(Grayscale));
            }
        }
    }
}
