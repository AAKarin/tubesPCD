using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


//namespace MiniPhotoshop.Logic.ImageProcessing
//{
//    internal class BlackWhite
//    {
//    }
//}


// Pastikan namespace ini cocok dengan proyek Anda
namespace MiniPhotoshop.Logic.ImageProcessing
{
    public static class BlackWhite
    {
        /// <summary>
        /// Menerapkan filter Binarization (Thresholding) ke gambar.
        /// </summary>
        /// <param name="originalImage">Gambar yang akan diproses.</param>
        /// <param name="sliderStep">Nilai dari slider (0-4).</param>
        /// <returns>Gambar Bitmap baru yang hanya berisi hitam & putih.</returns>
        public static Bitmap ApplyBinarization(Bitmap originalImage, int sliderStep)
        {
            // 1. Tentukan nilai ambang (threshold) berdasarkan 5 langkah slider
            int threshold;
            switch (sliderStep)
            {
                case 0:
                    threshold = 50; // Lebih banyak hitam
                    break;
                case 1:
                    threshold = 100;
                    break;
                case 2:
                    threshold = 127; // "Normal"
                    break;
                case 3:
                    threshold = 150;
                    break;
                case 4:
                    threshold = 200; // Lebih banyak putih
                    break;
                default:
                    threshold = 127;
                    break;
            }
            Bitmap resultImage = new Bitmap(originalImage.Width, originalImage.Height);
            // 2. Loop melalui setiap piksel
            for (int y = 0; y < originalImage.Height; y++)
            {
                for (int x = 0; x < originalImage.Width; x++)
                {
                    Color originalColor = originalImage.GetPixel(x, y);
                    // 3. Ubah piksel menjadi Grayscale (menggunakan formula Luminosity)
                    int gray = (int)(originalColor.R * 0.299 +
                    originalColor.G * 0.587 +
                    originalColor.B * 0.114);
                    // 4. Terapkan threshold
                    if (gray < threshold)
                    {
                        resultImage.SetPixel(x, y, Color.Black); // Jadi hitam
                    }
                    else
                    {
                        resultImage.SetPixel(x, y, Color.White); // Jadi putih
                    }
                }
            }
            return resultImage;
        }
    }
}