using System;                     
using System.Drawing;
using System.Drawing.Imaging;  

namespace ImageProcessing.ImageOperations
{
    public static class ContrastLinq
    {
        public static Bitmap AsContrast(this Bitmap bmp, float value)
        {
            Bitmap copy = (Bitmap)bmp.Clone();
            copy.Contrast(value);
            return copy;
        }

        public static void Contrast(this Bitmap bmp, float value)
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
                    float red, green, blue;
                    value = (100.0f + value) / 100.0f;
                    value *= value;
                    while (i < bytes.Length)
                    {
                        red = bytes[i + ColorShift.Red] / 255.0f;
                        green = bytes[i + ColorShift.Green] / 255.0f;
                        blue = bytes[i + ColorShift.Blue] / 255.0f;

                        red = (((red - 0.5f) * value) + 0.5f) * 255.0f;
                        green = (((green - 0.5f) * value) + 0.5f) * 255.0f;
                        blue = (((blue - 0.5f) * value) + 0.5f) * 255.0f;

                        bytes[i + ColorShift.Red] = (byte)(red > 255 ? 255 : red < 0 ? 0 : red);
                        bytes[i + ColorShift.Green] = (byte)(green > 255 ? 255 : green < 0 ? 0 : green);
                        bytes[i + ColorShift.Blue] = (byte)(blue > 255 ? 255 : blue < 0 ? 0 : blue);

                        i += pixelSize;
                    }

                    data.ReturnBytes(bytes);
                    bmp.UnlockBits(data);
                }                            
                else
                {
                    throw new BitmapOperationException(nameof(Contrast));
                }

            }
            catch (Exception ex)
            {
                throw new BitmapOperationException("Adjusting Bitmap contrast has failed.", ex, nameof(Contrast));
            }
        }
    }
}
