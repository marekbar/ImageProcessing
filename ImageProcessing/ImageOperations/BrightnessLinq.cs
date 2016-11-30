using ImageProcessing.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing.ImageOperations
{
    public static class BrightnessLinq
    {
        public static Bitmap AsBrightness(this Bitmap bmp, int level)
        {
            Bitmap copy = (Bitmap)bmp.Clone();
            copy.Brightness(level);
            return copy;
        }

        public static void Brightness(this Bitmap bmp, int level)
        {
            try
            {
                var pixelSize = System.Drawing.Image.GetPixelFormatSize(bmp.PixelFormat) / 8;
                if (bmp.PixelFormat == PixelFormat.Format24bppRgb && pixelSize == 3)
                {
                    var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                    var data = bmp.LockBits(rect, ImageLockMode.WriteOnly, bmp.PixelFormat);
                    byte[] bytes = data.GetBytesFromImage();

                    byte[] LUT = new byte[256];
                    int i = 0;
                    while (i < LUT.Length)
                    {
                        LUT[i] = (byte)(((level + i) > 255) ? 255 : (((level + i) < 0) ? 0 : (level + i)));
                        i++;
                    }
                    i = 0;
                    while (i < bytes.Length)
                    {
                        bytes[i] = LUT[bytes[i]];
                        i += pixelSize;
                    }

                    data.ReturnBytes(bytes);
                    bmp.UnlockBits(data);
                }                            
                else
                {
                    throw new BitmapOperationException(nameof(Brightness));
                }

            }
            catch (Exception ex)
            {
                throw new BitmapOperationException("Adjusting Bitmap brightness has failed.", ex, nameof(Brightness));
            }
        }
    }
}
