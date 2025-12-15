using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace MiniPhotoshop.Logic.Histogram
{
    public class GlobalHistogram
    {
        public Bitmap Apply(Bitmap source)
        {
            int w = source.Width;
            int h = source.Height;
            int numPixels = w * h;
            Bitmap dst = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            BitmapData srcData = source.LockBits(new Rectangle(0, 0, w, h),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            BitmapData dstData = dst.LockBits(new Rectangle(0, 0, w, h),
                ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            int bytes = Math.Abs(srcData.Stride) * h;
            byte[] buffer = new byte[bytes];
            byte[] result = new byte[bytes];
            Marshal.Copy(srcData.Scan0, buffer, 0, bytes);

            // 1. Hitung Histogram (Frekuensi kemunculan warna)
            int[] histR = new int[256];
            int[] histG = new int[256];
            int[] histB = new int[256];

            for (int i = 0; i < bytes; i += 4)
            {
                histB[buffer[i]]++;
                histG[buffer[i + 1]]++;
                histR[buffer[i + 2]]++;
            }

            // 2. Hitung CDF (Cumulative Distribution Function)
            int[] mapR = CalculateCDF(histR, numPixels);
            int[] mapG = CalculateCDF(histG, numPixels);
            int[] mapB = CalculateCDF(histB, numPixels);

            // 3. Mapping Pixel Lama ke Nilai Baru
            Parallel.For(0, bytes / 4, i =>
            {
                int k = i * 4;
                result[k] = (byte)mapB[buffer[k]]; // Map Blue
                result[k + 1] = (byte)mapG[buffer[k + 1]]; // Map Green
                result[k + 2] = (byte)mapR[buffer[k + 2]]; // Map Red
                result[k + 3] = buffer[k + 3]; // Alpha
            });

            Marshal.Copy(result, 0, dstData.Scan0, bytes);
            source.UnlockBits(srcData);
            dst.UnlockBits(dstData);
            return dst;
        }

        // Helper: Menghitung Distribusi Kumulatif
        private int[] CalculateCDF(int[] histogram, int totalPixels)
        {
            int[] map = new int[256];
            long sum = 0;
            float scale = 255.0f / totalPixels;

            for (int i = 0; i < 256; i++)
            {
                sum += histogram[i];
                int val = (int)(sum * scale);
                if (val > 255) val = 255;
                map[i] = val;
            }
            return map;
        }
    }
}
