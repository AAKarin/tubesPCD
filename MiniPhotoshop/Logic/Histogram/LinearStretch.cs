using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace MiniPhotoshop.Logic.Histogram
{
    public class LinearStretch
    {
        public Bitmap Apply(Bitmap source)
        {
            int w = source.Width;
            int h = source.Height;
            Bitmap dst = new Bitmap(w, h, PixelFormat.Format32bppArgb);

            BitmapData srcData = source.LockBits(
                new Rectangle(0, 0, w, h),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);

            BitmapData dstData = dst.LockBits(
                new Rectangle(0, 0, w, h),
                ImageLockMode.WriteOnly,
                PixelFormat.Format32bppArgb);

            int bytes = Math.Abs(srcData.Stride) * h;
            byte[] buffer = new byte[bytes];
            byte[] result = new byte[bytes];
            Marshal.Copy(srcData.Scan0, buffer, 0, bytes);

            byte minR = 255, maxR = 0;
            byte minG = 255, maxG = 0;
            byte minB = 255, maxB = 0;

            for (int i = 0; i < bytes; i += 4)
            {
                byte b = buffer[i];
                byte g = buffer[i + 1];
                byte r = buffer[i + 2];

                if (b < minB) minB = b; if (b > maxB) maxB = b;
                if (g < minG) minG = g; if (g > maxG) maxG = g;
                if (r < minR) minR = r; if (r > maxR) maxR = r;
            }

            if (maxB == minB) maxB++;
            if (maxG == minG) maxG++;
            if (maxR == minR) maxR++;

            Parallel.For(0, bytes / 4, i =>
            {
                int k = i * 4;
                result[k] = (byte)((buffer[k] - minB) * 255 / (maxB - minB));
                result[k + 1] = (byte)((buffer[k + 1] - minG) * 255 / (maxG - minG));
                result[k + 2] = (byte)((buffer[k + 2] - minR) * 255 / (maxR - minR));
                result[k + 3] = buffer[k + 3];
            });

            Marshal.Copy(result, 0, dstData.Scan0, bytes);
            source.UnlockBits(srcData);
            dst.UnlockBits(dstData);
            return dst;
        }
    }
}
