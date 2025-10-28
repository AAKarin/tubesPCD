using System;
using System.Drawing;

namespace MiniPhotoshop.Logic.ImageProcessing
{
    public static class Brightness
    {
        /// <summary>
        /// Menyesuaikan tingkat kecerahan gambar berdasarkan nilai slider.
        /// </summary>
        /// <param name="originalImage">Gambar sumber yang akan diubah.</param>
        /// <param name="adjustment">Nilai brightness dari -255 sampai 255.</param>
        /// <returns>Gambar Bitmap baru dengan tingkat kecerahan yang disesuaikan.</returns>
        public static Bitmap AdjustBrightness(Bitmap originalImage, int adjustment)
        {
            Bitmap resultImage = new Bitmap(originalImage.Width, originalImage.Height);

            for (int y = 0; y < originalImage.Height; y++)
            {
                for (int x = 0; x < originalImage.Width; x++)
                {
                    Color originalColor = originalImage.GetPixel(x, y);

                    // Tambahkan nilai brightness ke setiap kanal warna
                    int newR = originalColor.R + adjustment;
                    int newG = originalColor.G + adjustment;
                    int newB = originalColor.B + adjustment;

                    // Pastikan nilai tetap dalam rentang 0–255 (clamping)
                    newR = Math.Clamp(newR, 0, 255);
                    newG = Math.Clamp(newG, 0, 255);
                    newB = Math.Clamp(newB, 0, 255);

                    Color newColor = Color.FromArgb(originalColor.A, newR, newG, newB);
                    resultImage.SetPixel(x, y, newColor);
                }
            }

            return resultImage;
        }
    }
}