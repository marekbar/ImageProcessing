using ImageProcessing.Enums;
using ImageProcessing.Filters;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageProcessing.ImageOperations
{
    public static class FilterLinq
    {
        public static Bitmap AsGrayscale(this Bitmap bmp, FilterMatrix filter)
        {
            Bitmap copy = (Bitmap)bmp.Clone();
            copy.Filter(filter);
            return copy;
        }

        public static void Filter(this Bitmap bmp, FilterMatrix filter)
        {
            try
            {
                if (filter.Factor == 0) return;

                var pixelSize = System.Drawing.Image.GetPixelFormatSize(bmp.PixelFormat) / 8;
                if (bmp.PixelFormat == PixelFormat.Format24bppRgb && pixelSize == 3)
                {
                    var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                    var data = bmp.LockBits(rect, ImageLockMode.WriteOnly, bmp.PixelFormat);
                    byte[] bytes = data.GetBytesFromImage();

                    byte[] processed = new byte[bytes.Length];

                    double blue = 0.0, green = 0.0, red = 0.0;

                    int fo = filter.Offset;
                    int indexOffset = 0, byteOffset = 0, ds = data.Stride;
                    int y, x, fx, fy;
                    for (y = fo; y < data.Height - fo; y++)
                    {
                        for (x = fo; x < data.Width - fo; x++)
                        {
                            blue = green = red = 0;

                            byteOffset = y * ds + x * pixelSize;

                            for (fy = -fo; fy <= fo; fy++)
                            {
                                for (fx = -fo; fx <= fo; fx++)
                                {
                                    indexOffset = byteOffset + fx * pixelSize + fy * ds;

                                    blue += bytes[indexOffset + ColorShift.Blue] * filter[fy + fo, fx + fo];
                                    green += bytes[indexOffset + ColorShift.Green] * filter[fy + fo, fx + fo];
                                    red += bytes[indexOffset + ColorShift.Red] * filter[fy + fo, fx + fo];
                                }
                            }

                            blue = blue * filter.Factor + fo;
                            green = green * filter.Factor + fo;
                            red = red * filter.Factor + fo;
                            if (blue > 255) blue = 255; if (blue < 0) blue = 0;
                            if (green > 255) green = 255; if (green < 0) green = 0;
                            if (red > 255) red = 255; if (red < 0) red = 0;

                            processed[byteOffset + ColorShift.Blue] = (byte)(blue);
                            processed[byteOffset + ColorShift.Green] = (byte)(green);
                            processed[byteOffset + ColorShift.Red] = (byte)(red);
                        }
                    }

                    data.ReturnBytes(bytes);
                    bmp.UnlockBits(data);
                }                            
                else
                {
                    throw new BitmapOperationException(nameof(Filter));
                }

            }
            catch (Exception ex)
            {
                throw new BitmapOperationException("Filtering Bitmap has failed.", ex, nameof(Filter));
            }
        }
    }
}
