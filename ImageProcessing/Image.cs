using ImageProcessing.Enums;
using ImageProcessing.Filters;
using System.Drawing;
using System.Drawing.Imaging;
namespace ImageProcessing
{  
    public class Image
    {    
        private string filename;
        private Bitmap bmp;
        public Bitmap OriginalImage { get; private set; }
        private BitmapData data;
        private byte[] bytes;

        public Image(string filename)
        {
            this.filename = filename;
            bmp = new Bitmap(this.filename);
            OriginalImage = (Bitmap)bmp.Clone();
            PixelSize = System.Drawing.Image.GetPixelFormatSize(bmp.PixelFormat) / 8;
            rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
        }

        public Bitmap GetCopy()
        {
            return (Bitmap)this.bmp.Clone();
        }

        private int PixelSize;
        private Rectangle rect;

        private void AccessImageData()
        {
            data = bmp.LockBits(rect, ImageLockMode.WriteOnly, bmp.PixelFormat);
            bytes = data.GetBytesFromImage();
        }

        private void ReleaseImageData(byte[] byteArray = null)
        {
            data.ReturnBytes(byteArray != null ? byteArray : bytes);
            bmp.UnlockBits(data);
        }

        public void ToGrayScale()
        {
            if (bmp.PixelFormat == PixelFormat.Format24bppRgb && PixelSize == 3)
            {
                AccessImageData();

                double red;
                double green;
                double blue;
                byte gray;

                int i = 0;
                while (i < bytes.Length)
                {
                    blue = bytes[i + ColorShift.Blue];
                    green = bytes[i + ColorShift.Green];
                    red = bytes[i + ColorShift.Red];
                    gray = (byte)(ColorSaturation.Red * red + ColorSaturation.Green * green + ColorSaturation.Blue * blue);
                    bytes[i + ColorShift.Blue] = bytes[i + ColorShift.Green] = bytes[i + ColorShift.Red] = gray;
                    i += PixelSize;
                }
                ReleaseImageData();
            }
        }

        public void ToBlackOrWhiteScale(byte treshold)
        {
            if (bmp.PixelFormat == PixelFormat.Format24bppRgb && PixelSize == 3)
            {
                AccessImageData();
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
                    i += PixelSize;
                }
                ReleaseImageData();
            }
        }

        public void AdjustContrast(float value)
        {
            if (bmp.PixelFormat == PixelFormat.Format24bppRgb && PixelSize == 3)
            {
                AccessImageData();
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

                    i += PixelSize;
                }

                ReleaseImageData();
            }
        }

        public void AdjustBrightness(int level)
        {
            if (bmp.PixelFormat == PixelFormat.Format24bppRgb && PixelSize == 3)
            {
                AccessImageData();
                byte[] LUT = BuildBrightnessLUT(level);
                int i = 0;
                while (i < bytes.Length)
                {
                    bytes[i] = LUT[bytes[i]];
                    i += PixelSize;
                }
                ReleaseImageData();
            }
        }

        private static byte[] BuildBrightnessLUT(int level)
        {
            byte[] LUT = new byte[256];
            int i = 0;
            while (i < LUT.Length)
            {
                LUT[i] = (byte)(((level + i) > 255) ? 255 : (((level + i) < 0) ? 0 : (level + i)));
                i++;
            }
            return LUT;
        }

        public void RemoveColor(ColorChoice color)
        {
            if (bmp.PixelFormat == PixelFormat.Format24bppRgb && PixelSize == 3)
            {
                AccessImageData();
                int i = 0;
                int c = (int)color;
                while (i < bytes.Length)
                {
                    bytes[i + c] = 0;
                    i += PixelSize;
                }
                ReleaseImageData();
            }
        }

        public void MinusValue(ColorChoice color, byte value)
        {
            if (bmp.PixelFormat == PixelFormat.Format24bppRgb && PixelSize == 3)
            {
                AccessImageData();
                int i = 0;
                byte zero = 0;
                int c = (int)color;
                while (i < bytes.Length)
                {
                    bytes[i + c] = (byte)((bytes[i + c] - value) > zero ? bytes[i + c] - value : zero);
                    i += PixelSize;
                }
                ReleaseImageData();
            }
        }

        public void FilterImage(FilterMatrix filter)
        {
            if (bmp.PixelFormat == PixelFormat.Format24bppRgb && PixelSize == 3)
            {
                if (filter.Factor == 0)
                    return;
                AccessImageData();
                byte[] processed = new byte[bytes.Length];

                double blue = 0.0, green = 0.0, red = 0.0;

                int fo = filter.Offset;
                int indexOffset = 0,byteOffset = 0,ds = data.Stride;
                int y, x, fx, fy;
                for (y = fo; y < data.Height - fo; y++)
                {
                    for (x = fo; x < data.Width - fo; x++)
                    {
                        blue = green = red = 0;

                        byteOffset = y * ds + x * PixelSize;

                        for (fy = -fo; fy <= fo; fy++)
                        {
                            for (fx = -fo; fx <= fo; fx++)
                            {
                                indexOffset = byteOffset + fx * PixelSize + fy * ds;

                                blue += bytes[indexOffset + ColorShift.Blue] * filter[fy + fo, fx + fo];
                                green += bytes[indexOffset + ColorShift.Green] * filter[fy + fo, fx + fo];
                                red += bytes[indexOffset + ColorShift.Red] * filter[fy + fo, fx + fo];
                            }
                        }

                        blue = blue * filter.Factor + fo;
                        green = green * filter.Factor + fo;
                        red = red * filter.Factor + fo;
                        if (blue > 255) blue = 255;if (blue < 0) blue = 0;
                        if (green > 255) green = 255; if (green < 0) green = 0;
                        if (red > 255) red = 255; if (red < 0) red = 0;                      

                        processed[byteOffset + ColorShift.Blue] = (byte)(blue);
                        processed[byteOffset + ColorShift.Green] = (byte)(green);
                        processed[byteOffset + ColorShift.Red] = (byte)(red);
                    }
                }
                ReleaseImageData(processed);
            }
        }

        public void Save(string filename)
        {
            bmp.Save(filename);
        }
    }
}
