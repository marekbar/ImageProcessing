using System;                     
using System.Drawing;
using System.Drawing.Imaging;  

namespace ImageProcessing.ImageOperations
{
    public static class ColorRemoveLinq
    {
        public static Bitmap AsColorRemove(this Bitmap bmp, ColorChoice color)
        {
            Bitmap copy = (Bitmap)bmp.Clone();
            copy.ColorRemove(color);
            return copy;
        }

        public static void ColorRemove(this Bitmap bmp, ColorChoice color)
        {
            try
            {
                var pixelSize = Image.GetPixelFormatSize(bmp.PixelFormat) / 8;
                if (bmp.PixelFormat == PixelFormat.Format24bppRgb && pixelSize == 3 )
                {
                    var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                    var data = bmp.LockBits(rect, ImageLockMode.WriteOnly, bmp.PixelFormat);
                    byte[] bytes = data.GetBytesFromImage();

                    int i = 0;
                    int c = (int)color;
                    while (i < bytes.Length)
                    {
                        bytes[i + c] = 0;
                        i += pixelSize;
                    }

                    data.ReturnBytes(bytes);
                    bmp.UnlockBits(data);
                }                            
                else
                {
                    throw new BitmapOperationException(nameof(ColorRemove));
                }

            }
            catch (Exception ex)
            {
                throw new BitmapOperationException("Adjusting Bitmap contrast has failed.", ex, nameof(ColorRemove));
            }
        }
    }
}
