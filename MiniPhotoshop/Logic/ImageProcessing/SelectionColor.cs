using System.Drawing.Drawing2D;


namespace MiniPhotoshop.Logic.ImageProcessing
{
    public static class SelectionColor
    {
        /// <summary>
        /// Logika murni untuk memproses gambar.
        /// Menerima gambar asli dan warna target, lalu mengembalikan gambar baru.
        /// </summary>
        public static Bitmap ApplySelection(Bitmap originalImage, Color selectedColor)
        {
            Bitmap resultImage = new Bitmap(originalImage.Width, originalImage.Height);
            int targetColorArgb = selectedColor.ToArgb(); // Perbandingan warna yang akurat

            // Loop melalui setiap piksel
            for (int y = 0; y < originalImage.Height; y++)
            {
                for (int x = 0; x < originalImage.Width; x++)
                {
                    Color currentColor = originalImage.GetPixel(x, y);

                    // Jika warna sama, pertahankan. Jika beda, ganti putih.
                    if (currentColor.ToArgb() == targetColorArgb)
                    {
                        resultImage.SetPixel(x, y, currentColor);
                    }
                    else
                    {
                        resultImage.SetPixel(x, y, Color.White);
                    }
                }
            }
            return resultImage;
        }
    }
}