using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing
{
    public static class BitmapDataExtensions
    {
        public static byte[] GetBytesFromImage(this BitmapData data)
        {
            byte[] bytes = new byte[data.Height * data.Stride];
            System.Runtime.InteropServices.Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static void ReturnBytes(this BitmapData data, byte[] bytes)
        {
            System.Runtime.InteropServices.Marshal.Copy(bytes, 0, data.Scan0, bytes.Length);
        }
    }
}
