using ImageProcessing.Enums;
using System;                    
using System.Drawing;
using System.Drawing.Imaging;   

namespace ImageProcessing.ImageOperations
{
    public static class BlackWhiteLinq
    {
        public static Bitmap AsBlackWhite(this Bitmap bmp, byte treshold)
        {
            Bitmap copy = (Bitmap)bmp.Clone();
            copy.BlackWhite(treshold);
            return copy;
        }

        public static void BlackWhite(this Bitmap bmp, byte treshold)
        {
            try
            {
                var pixelSize = System.Drawing.Image.GetPixelFormatSize(bmp.PixelFormat) / 8;
                if (bmp.PixelFormat == PixelFormat.Format24bppRgb && pixelSize == 3)
                {
                    var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                    var data = bmp.LockBits(rect, ImageLockMode.WriteOnly, bmp.PixelFormat);
                    byte[] bytes = data.GetBytesFromImage();
                 
                    int i = 0;
                    while (i < bytes.Length)
                    {
                        bytes[i + ColorShift.Blue] = bytes[i + ColorShift.Green] = bytes[i + ColorShift.Red] =
                            (byte)(
                            (
                                ColorSaturation.Red * bytes[i + ColorShift.Red] +
                                ColorSaturation.Green * bytes[i + ColorShift.Green] +
                                ColorSaturation.Blue * bytes[i + ColorShift.Blue]
                            ) > treshold ? 255 : 0);
                        i += pixelSize;
                    }
                    data.ReturnBytes(bytes);
                    bmp.UnlockBits(data);
                }                            
                else
                {
                    throw new BitmapOperationException(nameof(BlackWhite));
                }

            }
            catch (Exception ex)
            {
                throw new BitmapOperationException("Processing Bitmap to grayscale has failed.", ex, nameof(BlackWhite));
            }
        }
    }
}
