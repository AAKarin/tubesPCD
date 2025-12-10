using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
namespace MiniPhotoshop.Logic.EdgeDetection
{
    public class Sobel
    {
        public Bitmap Apply(Bitmap source)
        {
            // 1. Setup Awal (LockBits)
            Bitmap bmp = new Bitmap(source);
            int width = bmp.Width;
            int height = bmp.Height;

            BitmapData srcData = bmp.LockBits(new Rectangle(0, 0, width, height),
            ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            int bytes = Math.Abs(srcData.Stride) * height;
            byte[] pixelBuffer = new byte[bytes];
            byte[] resultBuffer = new byte[bytes];
            Marshal.Copy(srcData.Scan0, pixelBuffer, 0, bytes);
            int stride = srcData.Stride;
            // 2. Definisi Kernel Sobel (3x3)
            // Gx = Deteksi Garis Vertikal
            int[,] gx = {
                            { -1, 0, 1 },
                            { -2, 0, 2 },
                            { -1, 0, 1 }
                         };
            // Gy = Deteksi Garis Horizontal
            int[,] gy = {
                            { -1, -2, -1 },
                            { 0, 0, 0 },
                            { 1, 2, 1 }
                         };
            // 3. Proses Loop Parallel (Mulai index 1 sampai width-1 karena kernel 3x3 butuh tepi)
            Parallel.For(1, height - 1, y =>
            {
                for (int x = 1; x < width - 1; x++)
                {
                    double sumX = 0;
                    double sumY = 0;
                    // Konvolusi 3x3
                    for (int ky = -1; ky <= 1; ky++)
                    {
                        for (int kx = -1; kx <= 1; kx++)
                        {
                            // Ambil pixel tetangga
                            int pos = (y + ky) * stride + (x + kx) * 4;
                            int val = pixelBuffer[pos]; // Ambil Blue channel (karena Grayscale)
                                                        // Kalikan dengan Kernel
                            sumX += val * gx[ky + 1, kx + 1];
                            sumY += val * gy[ky + 1, kx + 1];
                        }
                    }
                    // 4. Hitung Magnitude (Kekuatan Tepi)
                    // Rumus: Akar(Gx^2 + Gy^2)
                    int magnitude = (int)Math.Sqrt((sumX * sumX) + (sumY * sumY));
                    // Clamping (Batas 0-255)
                    if (magnitude > 255) magnitude = 255;
                    if (magnitude < 0) magnitude = 0;
                    // Simpan ke Result
                    int k = y * stride + x * 4;
                    resultBuffer[k] = (byte)magnitude; // B
                    resultBuffer[k + 1] = (byte)magnitude; // G
                    resultBuffer[k + 2] = (byte)magnitude; // R
                    resultBuffer[k + 3] = 255; // Alpha (Pekat)
                }
            });
            // 4. Finalisasi
            bmp.UnlockBits(srcData);
            Bitmap resultImage = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            BitmapData resultData = resultImage.LockBits(new Rectangle(0, 0, width, height),
            ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(resultBuffer, 0, resultData.Scan0, bytes);
            resultImage.UnlockBits(resultData);
            return resultImage;
        }
    }
}
