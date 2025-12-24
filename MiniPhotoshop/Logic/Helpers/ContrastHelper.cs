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
        // Deskripsi: Mengubah kontras secara global menggunakan rumus linear.
        // Rumus: P_baru = (P_lama - 128) * alpha + 128
        // =========================================================
        public Bitmap ApplyPointContrast(Bitmap source, double contrastFactor)
        {
            if (source == null) return null;

            int w = source.Width;
            int h = source.Height;

            // Buat bitmap tujuan kosong
            Bitmap dst = new Bitmap(w, h, PixelFormat.Format32bppArgb);

            // Kunci bit di memori agar akses cepat (Direct Memory Access)
            BitmapData srcData = source.LockBits(new Rectangle(0, 0, w, h),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            BitmapData dstData = dst.LockBits(new Rectangle(0, 0, w, h),
                ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            int bytes = Math.Abs(srcData.Stride) * h;
            byte[] buffer = new byte[bytes];

            // Salin data dari pointer memori ke array byte managed
            Marshal.Copy(srcData.Scan0, buffer, 0, bytes);

            // Proses Parallel Loop (Multithreading)
            // Loop berjalan per PIKSEL (4 byte: B, G, R, A)
            Parallel.For(0, bytes / 4, i =>
            {
                int idx = i * 4;

                for (int c = 0; c < 3; c++) // Loop untuk warna B, G, R
                {
                    double val = buffer[idx + c];

                    // Rumus Peregangan Titik (Pivot di tengah/128)
                    val = ((val - 128) * contrastFactor) + 128;

                    // Batasi 0-255 (Clipping) agar tidak overflow
                    buffer[idx + c] = (byte)Math.Max(0, Math.Min(255, val));
                }

                buffer[idx + 3] = 255; // Alpha set penuh (tidak transparan)
            });

            // Salin balik array byte yang sudah dimodifikasi ke bitmap tujuan
            Marshal.Copy(buffer, 0, dstData.Scan0, bytes);

            // Lepaskan kunci memori
            source.UnlockBits(srcData);
            dst.UnlockBits(dstData);

            return dst;
        }

        // =========================================================
        // 2. ARAS LOKAL (LOCAL STRETCHING)
        // Deskripsi: Mencari Min & Max di area tetangga (Window) untuk
        // melakukan stretching kontras adaptif.
        // =========================================================
        public Bitmap ApplyLocalContrast(Bitmap source, int windowSize)
        {
            if (source == null) return null;

            int w = source.Width;
            int h = source.Height;

            Bitmap dst = new Bitmap(w, h, PixelFormat.Format32bppArgb);

            BitmapData srcData = source.LockBits(new Rectangle(0, 0, w, h),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            BitmapData dstData = dst.LockBits(new Rectangle(0, 0, w, h),
                ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            int stride = srcData.Stride;
            int bytes = Math.Abs(stride) * h;

            byte[] srcBuffer = new byte[bytes];
            byte[] dstBuffer = new byte[bytes];

            // Copy source data ke buffer
            Marshal.Copy(srcData.Scan0, srcBuffer, 0, bytes);

            int radius = windowSize / 2;

            // Loop setiap baris (Height) secara parallel
            Parallel.For(0, h, y =>
            {
                for (int x = 0; x < w; x++)
                {
                    int idx = y * stride + x * 4;

                    // A. Cari Min & Max di area LOKAL (Window)
                    byte localMin = 255;
                    byte localMax = 0;

                    // Nested Loop untuk area tetangga
                    for (int wy = -radius; wy <= radius; wy++)
                    {
                        for (int wx = -radius; wx <= radius; wx++)
                        {
                            int py = y + wy;
                            int px = x + wx;

                            // Cek batas gambar (Boundary Check)
                            if (py >= 0 && py < h && px >= 0 && px < w)
                            {
                                int nIdx = py * stride + px * 4;

                                // Mengambil channel Hijau (+1) sebagai referensi kecerahan
                                // (Green channel sering digunakan karena mata manusia paling sensitif terhadap hijau)
                                byte val = srcBuffer[nIdx + 1];

                                if (val < localMin) localMin = val;
                                if (val > localMax) localMax = val;
                            }
                        }
                    }

                    // B. Hitung Faktor Skala Lokal
                    // Jika flat (max == min), skala = 1 (tidak berubah)
                    double scale = (localMax == localMin) ? 1 : (255.0 / (localMax - localMin));

                    // C. Terapkan rumus stretching ke pixel ini
                    for (int c = 0; c < 3; c++)
                    {
                        double val = srcBuffer[idx + c];

                        // Rumus: (Nilai Asli - Min Lokal) * Skala
                        double res = (val - localMin) * scale;

                        dstBuffer[idx + c] = (byte)Math.Max(0, Math.Min(255, res));
                    }

                    dstBuffer[idx + 3] = 255; // Alpha
                }
            });

            // Salin hasil ke destinasi
            Marshal.Copy(dstBuffer, 0, dstData.Scan0, bytes);

            source.UnlockBits(srcData);
            dst.UnlockBits(dstData);

            return dst;
        }
    }
}
