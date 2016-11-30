using System.Drawing;
using System.Drawing.Imaging;

namespace ImageProcessing
{
    internal static class BitmapDataExtensions
    {
        internal static byte[] GetBytesFromImage(this BitmapData data)
        {
            byte[] bytes = new byte[data.Height * data.Stride];
            System.Runtime.InteropServices.Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);
            return bytes;
        }

        internal static void ReturnBytes(this BitmapData data, byte[] bytes)
        {
            System.Runtime.InteropServices.Marshal.Copy(bytes, 0, data.Scan0, bytes.Length);
        }

        internal static Bitmap Copy(this Bitmap bmp)
        {
            return (Bitmap)bmp.Clone();
        }
    }
}
