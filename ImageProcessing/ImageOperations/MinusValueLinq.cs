using System;                     
using System.Drawing;
using System.Drawing.Imaging;  

namespace ImageProcessing.ImageOperations
{
    public static class MinusValueLinq
    {
        public static Bitmap AsMinusValue(this Bitmap bmp, ColorChoice color, byte value)
        {
            Bitmap copy = (Bitmap)bmp.Clone();
            copy.MinusValue(color, value);
            return copy;
        }

        public static void MinusValue(this Bitmap bmp, ColorChoice color, byte value)
        {
            try
            {
                var pixelSize = System.Drawing.Image.GetPixelFormatSize(bmp.PixelFormat) / 8;
                if (bmp.PixelFormat == PixelFormat.Format24bppRgb && pixelSize == 3 )
                {
                    var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                    var data = bmp.LockBits(rect, ImageLockMode.WriteOnly, bmp.PixelFormat);
                    byte[] bytes = data.GetBytesFromImage();

                    int i = 0;
                    byte zero = 0;
                    int c = (int)color;
                    while (i < bytes.Length)
                    {
                        bytes[i + c] = (byte)((bytes[i + c] - value) > zero ? bytes[i + c] - value : zero);
                        i += pixelSize;
                    }

                    data.ReturnBytes(bytes);
                    bmp.UnlockBits(data);
                }                            
                else
                {
                    throw new BitmapOperationException(nameof(MinusValue));
                }

            }
            catch (Exception ex)
            {
                throw new BitmapOperationException("Adjusting Bitmap contrast has failed.", ex, nameof(MinusValue));
            }
        }
    }
}
