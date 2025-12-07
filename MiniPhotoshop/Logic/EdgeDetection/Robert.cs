using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace MiniPhotoshop.Logic.EdgeDetection
{
    public class Robert
    {
        public Bitmap Apply(Bitmap source)
        {
            // Clone source agar aman
            Bitmap bmp = new Bitmap(source);
            int width = bmp.Width;
            int height = bmp.Height;

            // Kunci memori (LockBits) untuk akses cepat
            BitmapData srcData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            int bytes = Math.Abs(srcData.Stride) * height;
            byte[] pixelBuffer = new byte[bytes];
            byte[] resultBuffer = new byte[bytes];

            // Salin data gambar ke array byte
            Marshal.Copy(srcData.Scan0, pixelBuffer, 0, bytes);
            int stride = srcData.Stride;

            // Loop Robert (Parallel)
            // Batas loop dikurangi 1 (height - 1 dan width - 1) karena Robert mengakses pixel tetangga (x+1, y+1)
            Parallel.For(0, height - 1, y =>
            {
                for (int x = 0; x < width - 1; x++)
                {
                    // Hitung indeks pixel dalam array 1D
                    int k = y * stride + x * 4;                 // Posisi (x, y)
                    int k_right = y * stride + (x + 1) * 4;     // Posisi (x+1, y)
                    int k_down = (y + 1) * stride + x * 4;      // Posisi (x, y+1)
                    int k_diag = (y + 1) * stride + (x + 1) * 4;// Posisi (x+1, y+1)

                    // Ambil nilai intensitas (Blue channel cukup karena gambar diasumsikan sudah Grayscale)
                    int p1 = pixelBuffer[k];        // Pixel Kiri-Atas
                    int p2 = pixelBuffer[k_right];  // Pixel Kanan-Atas
                    int p3 = pixelBuffer[k_down];   // Pixel Kiri-Bawah
                    int p4 = pixelBuffer[k_diag];   // Pixel Kanan-Bawah

                    // --- RUMUS ROBERT ---
                    // Gx (Silang 1): |p1 - p4|
                    // Gy (Silang 2): |p2 - p3|
                    int gx = Math.Abs(p1 - p4);
                    int gy = Math.Abs(p2 - p3);

                    // Magnitude = Gx + Gy (Pendekatan Manhattan Distance - Lebih Cepat)
                    // Bisa juga pakai sqrt(gx^2 + gy^2) untuk Euclidean
                    int magnitude = gx + gy;

                    // Clamp nilai agar tidak melebihi 255
                    if (magnitude > 255) magnitude = 255;
                    if (magnitude < 0) magnitude = 0;

                    // Simpan ke buffer hasil (Format BGRA)
                    resultBuffer[k] = (byte)magnitude;     // Blue
                    resultBuffer[k + 1] = (byte)magnitude; // Green
                    resultBuffer[k + 2] = (byte)magnitude; // Red
                    resultBuffer[k + 3] = 255;             // Alpha
                }
            });

            // Buka kunci memori source
            bmp.UnlockBits(srcData);

            // Buat gambar hasil dari array resultBuffer
            Bitmap resultImage = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            BitmapData resultData = resultImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(resultBuffer, 0, resultData.Scan0, bytes);
            resultImage.UnlockBits(resultData);

            return resultImage;
        }
    }
}