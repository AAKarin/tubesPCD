using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MiniPhotoshop.Logic.Geometry
{
    public class Scaling
    {
        /// <summary>
        /// Melakukan Zoom In/Out dengan mempertahankan ukuran kanvas asli.
        /// Zoom In = Gambar membesar dan ter-crop.
        /// Zoom Out = Gambar mengecil dan ada border hitam.
        /// </summary>
        public Bitmap ProcessScaling(Bitmap sourceImage, float scaleFactor)
        {
            if (sourceImage == null) return null;

            // 1. Ukuran Kanvas TETAP (Sama dengan gambar asli)
            int canvasWidth = sourceImage.Width;
            int canvasHeight = sourceImage.Height;

            // 2. Hitung Ukuran Gambar yang akan ditempel (Hasil Zoom)
            int scaledWidth = (int)(sourceImage.Width * scaleFactor);
            int scaledHeight = (int)(sourceImage.Height * scaleFactor);

            // 3. Buat Bitmap baru seukuran KANVAS ASLI
            Bitmap newImage = new Bitmap(canvasWidth, canvasHeight);

            using (Graphics g = Graphics.FromImage(newImage))
            {
                // Isi background dengan hitam (agar terlihat saat Zoom Out)
                g.Clear(Color.Black);

                // Set kualitas tinggi
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;

                // 4. Hitung Posisi Tengah (Centering)
                // Rumus: (UkuranKanvas - UkuranGambarBaru) / 2
                // Jika Zoom In, hasilnya negatif (artinya gambar geser ke kiri atas -> ter-crop)
                // Jika Zoom Out, hasilnya positif (artinya gambar geser ke tengah -> ada border hitam)
                int x = (canvasWidth - scaledWidth) / 2;
                int y = (canvasHeight - scaledHeight) / 2;

                // 5. Gambar citra yang sudah di-resize ke posisi tengah
                g.DrawImage(sourceImage, x, y, scaledWidth, scaledHeight);
            }

            return newImage;
        }
    }
}