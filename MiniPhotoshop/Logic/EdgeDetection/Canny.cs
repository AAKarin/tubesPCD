using System;
using System.Collections.Generic; // Wajib untuk Stack
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace MiniPhotoshop.Logic.EdgeDetection
{
    public class Canny
    {
        private float LowThreshold;
        private float HighThreshold;

        public Canny(float lowThresh = 20f, float highThresh = 100f)
        {
            this.LowThreshold = lowThresh;
            this.HighThreshold = highThresh;
        }

        public Bitmap Apply(Bitmap source)
        {
            // 0. Validasi Gambar
            if (source == null) return null;

            // 1. Gaussian Blur (5x5)
            Bitmap blurred = ApplyGaussian(source);

            // 2. Sobel (Gradient & Angle)
            float[,] gradient;
            float[,] angle;
            GetSobelData(blurred, out gradient, out angle);

            // 3. Non-Maximum Suppression
            float[,] nms = NonMaxSuppression(gradient, angle, source.Width, source.Height);

            // 4. Hysteresis Thresholding (Final)
            return Hysteresis(nms, source.Width, source.Height);
        }

        // -----------------------------------------------------------------------
        // TAHAP 1: GAUSSIAN BLUR
        // -----------------------------------------------------------------------
        private Bitmap ApplyGaussian(Bitmap src)
        {
            // Pastikan format 32bpp agar LockBits tidak error
            Bitmap dst = new Bitmap(src.Width, src.Height, PixelFormat.Format32bppArgb);

            // Clone source ke 32bpp temp jika perlu
            Bitmap src32 = src;
            if (src.PixelFormat != PixelFormat.Format32bppArgb)
            {
                src32 = new Bitmap(src.Width, src.Height, PixelFormat.Format32bppArgb);
                using (Graphics g = Graphics.FromImage(src32)) g.DrawImage(src, 0, 0, src.Width, src.Height);
            }

            BitmapData srcData = src32.LockBits(new Rectangle(0, 0, src32.Width, src32.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            BitmapData dstData = dst.LockBits(new Rectangle(0, 0, dst.Width, dst.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            int bytes = Math.Abs(srcData.Stride) * src32.Height;
            byte[] buffer = new byte[bytes];
            byte[] result = new byte[bytes];
            Marshal.Copy(srcData.Scan0, buffer, 0, bytes);

            int w = src32.Width; int h = src32.Height; int stride = srcData.Stride;
            double[,] kernel = { { 2, 4, 5, 4, 2 }, { 4, 9, 12, 9, 4 }, { 5, 12, 15, 12, 5 }, { 4, 9, 12, 9, 4 }, { 2, 4, 5, 4, 2 } };
            double div = 159;

            Parallel.For(2, h - 2, y =>
            {
                for (int x = 2; x < w - 2; x++)
                {
                    double sum = 0;
                    for (int ky = -2; ky <= 2; ky++)
                    {
                        for (int kx = -2; kx <= 2; kx++)
                        {
                            int pos = (y + ky) * stride + (x + kx) * 4;
                            sum += buffer[pos] * kernel[ky + 2, kx + 2];
                        }
                    }
                    byte val = (byte)Math.Min(255, Math.Max(0, sum / div));
                    int k = y * stride + x * 4;
                    result[k] = result[k + 1] = result[k + 2] = val;
                    result[k + 3] = 255;
                }
            });

            Marshal.Copy(result, 0, dstData.Scan0, bytes);
            src32.UnlockBits(srcData);
            dst.UnlockBits(dstData);

            if (src32 != src) src32.Dispose(); // Bersihkan temp jika ada

            return dst;
        }

        // -----------------------------------------------------------------------
        // TAHAP 2: SOBEL GRADIENT (DENGAN NORMALISASI 0-255)
        // -----------------------------------------------------------------------
        private void GetSobelData(Bitmap src, out float[,] gradient, out float[,] angle)
        {
            int w = src.Width;
            int h = src.Height;
            gradient = new float[w, h];
            angle = new float[w, h];

            BitmapData srcData = src.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            int bytes = Math.Abs(srcData.Stride) * h;
            byte[] buffer = new byte[bytes];
            Marshal.Copy(srcData.Scan0, buffer, 0, bytes);
            int stride = srcData.Stride;
            src.UnlockBits(srcData);

            int[,] gxK = { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
            int[,] gyK = { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } };

            float maxGradient = 0; // Untuk mencari nilai tertinggi

            // 1. Hitung Gradient Mentah
            for (int y = 1; y < h - 1; y++)
            {
                for (int x = 1; x < w - 1; x++)
                {
                    float sumX = 0;
                    float sumY = 0;

                    for (int ky = -1; ky <= 1; ky++)
                    {
                        for (int kx = -1; kx <= 1; kx++)
                        {
                            int pos = (y + ky) * stride + (x + kx) * 4;
                            float val = buffer[pos]; // Blue channel
                            sumX += val * gxK[ky + 1, kx + 1];
                            sumY += val * gyK[ky + 1, kx + 1];
                        }
                    }

                    float gVal = (float)Math.Sqrt(sumX * sumX + sumY * sumY);
                    gradient[x, y] = gVal;

                    if (gVal > maxGradient) maxGradient = gVal; // Simpan nilai max

                    double theta = Math.Atan2(sumY, sumX) * 180 / Math.PI;
                    if (theta < 0) theta += 180;
                    angle[x, y] = (float)theta;
                }
            }

            // 2. NORMALISASI (Kunci agar Canny Perfect!)
            // Kita ubah semua nilai agar berada di rentang 0-255
            // Jadi Threshold 20-100 akan bekerja sempurna.
            if (maxGradient > 0)
            {
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        gradient[x, y] = (gradient[x, y] / maxGradient) * 255f;
                    }
                }
            }
        }

        // -----------------------------------------------------------------------
        // TAHAP 3: NON-MAXIMUM SUPPRESSION (VERSI AMAN)
        // -----------------------------------------------------------------------
        private float[,] NonMaxSuppression(float[,] gradient, float[,] angle, int w, int h)
        {
            float[,] nms = new float[w, h];

            // Loop Aman (Hindari pinggir gambar)
            Parallel.For(1, h - 1, y =>
            {
                for (int x = 1; x < w - 1; x++)
                {
                    // Default 0 (Jangan 255, karena bisa mematikan garis secara tidak sengaja)
                    float q = 0, r = 0;
                    float ang = angle[x, y];

                    // Kuantisasi Sudut (4 Arah Utama)
                    // 0 Derajat (Horizontal) -> Cek Kiri & Kanan
                    if ((ang >= 0 && ang < 22.5) || (ang >= 157.5 && ang <= 180))
                    {
                        q = gradient[x + 1, y];
                        r = gradient[x - 1, y];
                    }
                    // 45 Derajat (Diagonal /) -> Cek Pojok Kanan Atas & Kiri Bawah
                    else if (ang >= 22.5 && ang < 67.5)
                    {
                        q = gradient[x + 1, y - 1];
                        r = gradient[x - 1, y + 1];
                    }
                    // 90 Derajat (Vertikal |) -> Cek Atas & Bawah
                    else if (ang >= 67.5 && ang < 112.5)
                    {
                        q = gradient[x, y + 1];
                        r = gradient[x, y - 1];
                    }
                    // 135 Derajat (Diagonal \) -> Cek Pojok Kiri Atas & Kanan Bawah
                    else if (ang >= 112.5 && ang < 157.5)
                    {
                        q = gradient[x - 1, y - 1];
                        r = gradient[x + 1, y + 1];
                    }

                    // Hanya simpan jika pixel ini adalah puncak (paling terang) dibanding tetangganya
                    if (gradient[x, y] >= q && gradient[x, y] >= r)
                        nms[x, y] = gradient[x, y];
                    else
                        nms[x, y] = 0;
                }
            });
            return nms;
        }

        // -----------------------------------------------------------------------
        // TAHAP 4: HYSTERESIS (FIX BACKGROUND HITAM)
        // -----------------------------------------------------------------------
        private Bitmap Hysteresis(float[,] nms, int w, int h)
        {
            Bitmap dst = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            BitmapData dstData = dst.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            int bytes = Math.Abs(dstData.Stride) * h;
            byte[] result = new byte[bytes];
            int stride = dstData.Stride;

            byte strong = 255; // Garis Kuat (Putih)
            byte weak = 50;    // Garis Lemah (Abu-abu)

            Stack<System.Drawing.Point> edgeStack = new Stack<System.Drawing.Point>();

            // 1. Thresholding Awal
            for (int y = 1; y < h - 1; y++)
            {
                for (int x = 1; x < w - 1; x++)
                {
                    int k = y * stride + x * 4;
                    float val = nms[x, y];

                    if (val >= HighThreshold)
                    {
                        result[k] = result[k + 1] = result[k + 2] = strong;
                        edgeStack.Push(new System.Drawing.Point(x, y));
                    }
                    else if (val >= LowThreshold)
                    {
                        result[k] = result[k + 1] = result[k + 2] = weak;
                    }
                    else
                    {
                        // Background = Hitam (0, 0, 0)
                        result[k] = result[k + 1] = result[k + 2] = 0;
                    }

                    // --- PERBAIKAN DI SINI ---
                    // Kita paksa Alpha (Transparency) selalu 255 (Pekat)
                    // Agar background hitamnya muncul, tidak tembus pandang.
                    result[k + 3] = 255;
                }
            }

            // 2. Flood Fill (Menyambungkan garis)
            while (edgeStack.Count > 0)
            {
                System.Drawing.Point p = edgeStack.Pop();
                int px = p.X;
                int py = p.Y;

                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0) continue;

                        int nx = px + j;
                        int ny = py + i;

                        if (nx > 0 && nx < w - 1 && ny > 0 && ny < h - 1)
                        {
                            int nk = ny * stride + nx * 4;
                            if (result[nk] == weak)
                            {
                                result[nk] = result[nk + 1] = result[nk + 2] = strong;
                                result[nk + 3] = 255; // Pastikan Alpha tetap 255
                                edgeStack.Push(new System.Drawing.Point(nx, ny));
                            }
                        }
                    }
                }
            }

            // 3. Bersihkan sisa Weak (Yang tidak tersambung jadi Hitam)
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int k = y * stride + x * 4;
                    if (result[k] == weak) // Jika masih weak (abu-abu), berarti sampah
                    {
                        result[k] = result[k + 1] = result[k + 2] = 0; // Hapus jadi hitam
                        result[k + 3] = 255; // Tetap Pekat
                    }
                }
            }

            Marshal.Copy(result, 0, dstData.Scan0, bytes);
            dst.UnlockBits(dstData);
            return dst;
        }
    }
}