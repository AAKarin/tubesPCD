using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks; // Wajib untuk Parallel Processing

namespace MiniPhotoshop.Logic.Helpers
{
    public class ContrastHelper
    {
        // =========================================================
        // 1. ARAS TITIK (POINT CONTRAST)
        // Rumus: P_baru = (P_lama - 128) * alpha + 128
        // =========================================================
        public Bitmap ApplyPointContrast(Bitmap source, double contrastFactor)
        {
            if (source == null) return null;

            int w = source.Width;
            int h = source.Height;
            Bitmap dst = new Bitmap(w, h, PixelFormat.Format32bppArgb);

            BitmapData srcData = source.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            BitmapData dstData = dst.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            int bytes = Math.Abs(srcData.Stride) * h;
            byte[] buffer = new byte[bytes];
            Marshal.Copy(srcData.Scan0, buffer, 0, bytes);

            // Gunakan Parallel Loop agar cepat
            Parallel.For(0, bytes / 4, i =>
            {
                int idx = i * 4;
                for (int c = 0; c < 3; c++) // B, G, R
                {
                    double val = buffer[idx + c];

                    // Rumus Peregangan Titik (Pivot di tengah/128)
                    val = ((val - 128) * contrastFactor) + 128;

                    // Batasi 0-255 (Clipping)
                    buffer[idx + c] = (byte)Math.Max(0, Math.Min(255, val));
                }
                buffer[idx + 3] = 255; // Alpha penuh
            });

            Marshal.Copy(buffer, 0, dstData.Scan0, bytes);
            source.UnlockBits(srcData);
            dst.UnlockBits(dstData);

            return dst;
        }

        // =========================================================
        // 2. ARAS LOKAL (LOCAL STRETCHING)
        // Mencari Min & Max di area tetangga (Window), bukan global.
        // =========================================================
        public Bitmap ApplyLocalContrast(Bitmap source, int windowSize)
        {
            if (source == null) return null;

            int w = source.Width;
            int h = source.Height;
            Bitmap dst = new Bitmap(w, h, PixelFormat.Format32bppArgb);

            BitmapData srcData = source.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            BitmapData dstData = dst.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            int stride = srcData.Stride;
            int bytes = Math.Abs(stride) * h;
            byte[] srcBuffer = new byte[bytes];
            byte[] dstBuffer = new byte[bytes];

            Marshal.Copy(srcData.Scan0, srcBuffer, 0, bytes);

            int radius = windowSize / 2;

            // Loop setiap pixel secara parallel
            Parallel.For(0, h, y =>
            {
                for (int x = 0; x < w; x++)
                {
                    int idx = y * stride + x * 4;

                    // A. Cari Min & Max di area LOKAL (Window)
                    byte localMin = 255;
                    byte localMax = 0;

                    for (int wy = -radius; wy <= radius; wy++)
                    {
                        for (int wx = -radius; wx <= radius; wx++)
                        {
                            int py = y + wy;
                            int px = x + wx;

                            // Cek batas gambar
                            if (py >= 0 && py < h && px >= 0 && px < w)
                            {
                                int nIdx = py * stride + px * 4;
                                // Ambil channel Hijau sebagai referensi kecerahan (opsional bisa dirata-rata)
                                byte val = srcBuffer[nIdx + 1];
                                if (val < localMin) localMin = val;
                                if (val > localMax) localMax = val;
                            }
                        }
                    }

                    // B. Hitung Faktor Skala Lokal
                    double scale = (localMax == localMin) ? 1 : (255.0 / (localMax - localMin));

                    // C. Terapkan rumus stretching ke pixel ini
                    for (int c = 0; c < 3; c++)
                    {
                        double val = srcBuffer[idx + c];
                        double res = (val - localMin) * scale;
                        dstBuffer[idx + c] = (byte)Math.Max(0, Math.Min(255, res));
                    }
                    dstBuffer[idx + 3] = 255;
                }
            });

            Marshal.Copy(dstBuffer, 0, dstData.Scan0, bytes);
            source.UnlockBits(srcData);
            dst.UnlockBits(dstData);

            return dst;
        }
    }
}