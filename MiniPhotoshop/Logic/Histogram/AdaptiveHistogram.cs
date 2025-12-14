using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace MiniPhotoshop.Logic.Histogram
{
    public class AdaptiveHistogram
    {
        private int _gridRows;
        private int _gridCols;

        // Constructor: Default grid 8x8
        public AdaptiveHistogram(int gridRows = 8, int gridCols = 8)
        {
            _gridRows = gridRows;
            _gridCols = gridCols;
        }

        public Bitmap Apply(Bitmap source)
        {
            int w = source.Width;
            int h = source.Height;

            Bitmap dst = (Bitmap)source.Clone();
            BitmapData dstData = dst.LockBits(new Rectangle(0, 0, w, h),
                ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            int bytes = Math.Abs(dstData.Stride) * h;
            byte[] buffer = new byte[bytes];
            Marshal.Copy(dstData.Scan0, buffer, 0, bytes);
            int stride = dstData.Stride;

            // 1. Hitung Ukuran Blok
            int tileW = (int)Math.Ceiling((double)w / _gridCols);
            int tileH = (int)Math.Ceiling((double)h / _gridRows);

            // 2. Hitung CDF untuk SETIAP TILE (Disimpan di Array 3D)
            // maps[row, col, intensity]
            int[,,] mapsR = new int[_gridRows, _gridCols, 256];
            int[,,] mapsG = new int[_gridRows, _gridCols, 256];
            int[,,] mapsB = new int[_gridRows, _gridCols, 256];

            // Kita hitung dulu semua histogram grid-nya
            Parallel.For(0, _gridRows, r =>
            {
                for (int c = 0; c < _gridCols; c++)
                {
                    // Tentukan area tile
                    int startX = c * tileW;
                    int startY = r * tileH;
                    int endX = Math.Min(startX + tileW, w);
                    int endY = Math.Min(startY + tileH, h);

                    int[] histR = new int[256];
                    int[] histG = new int[256];
                    int[] histB = new int[256];
                    int count = 0;

                    for (int y = startY; y < endY; y++)
                    {
                        for (int x = startX; x < endX; x++)
                        {
                            int k = y * stride + x * 4;
                            histB[buffer[k]]++;
                            histG[buffer[k + 1]]++;
                            histR[buffer[k + 2]]++;
                            count++;
                        }
                    }

                    // Simpan CDF ke array global
                    int[] cdfR = CalculateCDF(histR, count);
                    int[] cdfG = CalculateCDF(histG, count);
                    int[] cdfB = CalculateCDF(histB, count);

                    for (int i = 0; i < 256; i++)
                    {
                        mapsR[r, c, i] = cdfR[i];
                        mapsG[r, c, i] = cdfG[i];
                        mapsB[r, c, i] = cdfB[i];
                    }
                }
            });

            // 3. PROSES INTERPOLASI (Menghaluskan batas)
            // Kita loop per pixel, cari 4 tile terdekat, lalu interpolasi nilainya.
            Parallel.For(0, h, y =>
            {
                for (int x = 0; x < w; x++)
                {
                    // Hitung posisi relatif terhadap grid
                    // Titik pusat grid dianggap ada di tengah tile
                    float gridY = (float)(y - tileH / 2.0) / tileH;
                    float gridX = (float)(x - tileW / 2.0) / tileW;

                    // Cari index tile kiri-atas (r1, c1)
                    int r1 = (int)Math.Floor(gridY);
                    int c1 = (int)Math.Floor(gridX);

                    // Cari index tile kanan-bawah (r2, c2)
                    int r2 = r1 + 1;
                    int c2 = c1 + 1;

                    // Batasi index agar tidak keluar array
                    if (r1 < 0) r1 = 0; if (r2 >= _gridRows) r2 = _gridRows - 1;
                    if (c1 < 0) c1 = 0; if (c2 >= _gridCols) c2 = _gridCols - 1;

                    // Hitung fraksi (jarak) untuk interpolasi (0.0 s.d 1.0)
                    float alpha = gridY - (float)Math.Floor(gridY); // Jarak vertikal
                    float beta = gridX - (float)Math.Floor(gridX);  // Jarak horizontal

                    // Clamping fraksi (jika di pinggir gambar)
                    if (r1 == r2) alpha = 0.5f; // Di pinggir atas/bawah
                    if (c1 == c2) beta = 0.5f;  // Di pinggir kiri/kanan

                    int k = y * stride + x * 4;

                    // Interpolasi B, G, R
                    buffer[k] = (byte)BilinearInterpolate(buffer[k], mapsB, r1, r2, c1, c2, alpha, beta);
                    buffer[k + 1] = (byte)BilinearInterpolate(buffer[k + 1], mapsG, r1, r2, c1, c2, alpha, beta);
                    buffer[k + 2] = (byte)BilinearInterpolate(buffer[k + 2], mapsR, r1, r2, c1, c2, alpha, beta);
                }
            });

            Marshal.Copy(buffer, 0, dstData.Scan0, bytes);
            dst.UnlockBits(dstData);

            return dst;
        }

        // Fungsi Interpolasi Bilinear
        // Mencampur nilai dari 4 kotak tetangga berdasarkan jarak
        private int BilinearInterpolate(int val, int[,,] maps, int r1, int r2, int c1, int c2, float alpha, float beta)
        {
            // Ambil nilai mapping dari 4 titik sudut
            int tl = maps[r1, c1, val]; // Top-Left
            int tr = maps[r1, c2, val]; // Top-Right
            int bl = maps[r2, c1, val]; // Bottom-Left
            int br = maps[r2, c2, val]; // Bottom-Right

            // Interpolasi Horizontal
            float t = (1 - beta) * tl + beta * tr; // Atas
            float b = (1 - beta) * bl + beta * br; // Bawah

            // Interpolasi Vertikal
            return (int)((1 - alpha) * t + alpha * b);
        }

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