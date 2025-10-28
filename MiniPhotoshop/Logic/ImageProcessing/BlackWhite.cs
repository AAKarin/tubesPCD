using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using System.Drawing;
using System.Drawing.Imaging; // <- TAMBAHKAN INI
using System.Runtime.InteropServices; // <- TAMBAHKAN INI

// Pastikan namespace ini cocok
namespace MiniPhotoshop.Logic.ImageProcessing
{
    public static class BlackWhite
    {
        // Fungsi ini tetap sama, hanya memanggil fungsi ApplyThreshold
        public static Bitmap ApplyBinarization(Bitmap originalImage, int sliderStep)
        {
            int threshold;
            switch (sliderStep)
            {
                case 0: threshold = 50; break;
                case 1: threshold = 100; break;
                case 2: threshold = 127; break;
                case 3: threshold = 150; break;
                case 4: threshold = 200; break;
                default: threshold = 127; break;
            }
            // Panggil fungsi baru yang cepat
            return ApplyThreshold(originalImage, threshold);
        }

        // --- INI FUNGSI BARU YANG MENGGUNAKAN LOCKBITS ---
        private static Bitmap ApplyThreshold(Bitmap originalImage, int threshold)
        {
            // Buat gambar hasil dari gambar asli
            Bitmap resultImage = new Bitmap(originalImage);
            Rectangle rect = new Rectangle(0, 0, resultImage.Width, resultImage.Height);

            // Kunci memori gambar
            BitmapData bmpData = resultImage.LockBits(rect, ImageLockMode.ReadWrite,
                                                      resultImage.PixelFormat);

            IntPtr ptr = bmpData.Scan0;
            int bytes = Math.Abs(bmpData.Stride) * resultImage.Height;
            byte[] rgbValues = new byte[bytes];

            // Salin SEMUA data piksel ke array
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            int bytesPerPixel = Image.GetPixelFormatSize(resultImage.PixelFormat) / 8;
            int stride = bmpData.Stride;

            // Proses array (super cepat)
            for (int y = 0; y < resultImage.Height; y++)
            {
                int rowOffset = y * stride;
                for (int x = 0; x < resultImage.Width; x++)
                {
                    int i = rowOffset + (x * bytesPerPixel);

                    // 1. Grayscale (urutan BGR di memori)
                    int gray = (int)(rgbValues[i + 2] * 0.299 +  // R
                                       rgbValues[i + 1] * 0.587 +  // G
                                       rgbValues[i] * 0.114);   // B

                    // 2. Threshold
                    byte bwValue = (gray < threshold) ? (byte)0 : (byte)255;

                    // Setel kembali nilainya
                    rgbValues[i] = bwValue;
                    rgbValues[i + 1] = bwValue;
                    rgbValues[i + 2] = bwValue;
                }
            }

            // Salin SEMUA data dari array kembali ke gambar
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Buka kunci memori
            resultImage.UnlockBits(bmpData);
            return resultImage;
        }
    }
}