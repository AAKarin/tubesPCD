using System;
using System.Drawing;

namespace MiniPhotoshop.Logic.ImageProcessing
{
    public static class ConstantOperations
    {
        // Helper Clamp
        private static int Clamp(int value)
        {
            if (value < 0) return 0;
            if (value > 255) return 255;
            return value;
        }

        // --- KALI KONSTANTA ---
        public static Bitmap Multiply(Bitmap img, double valueK)
        {
            Bitmap resultBmp = new Bitmap(img.Width, img.Height);
            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                {
                    Color c = img.GetPixel(x, y);
                    int newR = Clamp((int)(c.R * valueK));
                    int newG = Clamp((int)(c.G * valueK));
                    int newB = Clamp((int)(c.B * valueK));
                    resultBmp.SetPixel(x, y, Color.FromArgb(newR, newG, newB));
                }
            }
            return resultBmp;
        }

        // --- BAGI KONSTANTA ---
        public static Bitmap Divide(Bitmap img, double valueK)
        {
            // Hindari pembagian dengan nol
            if (valueK == 0) valueK = 1;
            // Bagi sama dengan mengalikan dengan 1/K
            return Multiply(img, 1.0 / valueK);
        }
    }
}