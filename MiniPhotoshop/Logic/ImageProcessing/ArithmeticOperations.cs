using System;
using System.Drawing;

namespace MiniPhotoshop.Logic.ImageProcessing
{
    public static class ArithmeticOperations
    {
        private static int Clamp(int value)
        {
            if (value < 0) return 0;
            if (value > 255) return 255;
            return value;
        }

        // --- FUNGSI HELPER UNIVERSAL (BARU) ---
        private static Bitmap PerformOperation(Bitmap imgA, Bitmap imgB, Func<Color, Color, Color> operation)
        {
            // 1. Tentukan ukuran kanvas baru (terbesar)
            int newWidth = Math.Max(imgA.Width, imgB.Width);
            int newHeight = Math.Max(imgA.Height, imgB.Height);
            Bitmap resultBmp = new Bitmap(newWidth, newHeight);

            // 2. Loop melalui SETIAP piksel di kanvas baru
            for (int y = 0; y < newHeight; y++)
            {
                for (int x = 0; x < newWidth; x++)
                {
                    // 3. Cek lokasi piksel (x, y)
                    bool inA = (x < imgA.Width && y < imgA.Height);
                    bool inB = (x < imgB.Width && y < imgB.Height);

                    if (inA && inB)
                    {
                        // KASUS 1: Piksel saling bertemu. Lakukan operasi.
                        Color cA = imgA.GetPixel(x, y);
                        Color cB = imgB.GetPixel(x, y);
                        resultBmp.SetPixel(x, y, operation(cA, cB));
                    }
                    else if (inA)
                    {
                        // KASUS 2: Piksel hanya ada di A. Salin A.
                        resultBmp.SetPixel(x, y, imgA.GetPixel(x, y));
                    }
                    else if (inB)
                    {
                        // KASUS 3: Piksel hanya ada di B. Salin B.
                        resultBmp.SetPixel(x, y, imgB.GetPixel(x, y));
                    }
                    else
                    {
                        // KASUS 4: Area kosong (jika ada)
                        resultBmp.SetPixel(x, y, Color.Black);
                    }
                }
            }
            return resultBmp;
        }

        // --- FUNGSI PUBLIK (dipanggil oleh OperationManager) ---

        public static Bitmap Add(Bitmap imgA, Bitmap imgB)
        {
            return PerformOperation(imgA, imgB, (cA, cB) => {
                int r = Clamp(cA.R + cB.R);
                int g = Clamp(cA.G + cB.G);
                int b = Clamp(cA.B + cB.B);
                return Color.FromArgb(r, g, b);
            });
        }

        public static Bitmap Subtract(Bitmap imgA, Bitmap imgB)
        {
            return PerformOperation(imgA, imgB, (cA, cB) => {
                int r = Clamp(cA.R - cB.R);
                int g = Clamp(cA.G - cB.G);
                int b = Clamp(cA.B - cB.B);
                return Color.FromArgb(r, g, b);
            });
        }

        public static Bitmap Multiply(Bitmap imgA, Bitmap imgB)
        {
            return PerformOperation(imgA, imgB, (cA, cB) => {
                int r = Clamp((int)((cA.R / 255.0) * (cB.R / 255.0) * 255.0));
                int g = Clamp((int)((cA.G / 255.0) * (cB.G / 255.0) * 255.0));
                int b = Clamp((int)((cA.B / 255.0) * (cB.B / 255.0) * 255.0));
                return Color.FromArgb(r, g, b);
            });
        }

        public static Bitmap Divide(Bitmap imgA, Bitmap imgB)
        {
            return PerformOperation(imgA, imgB, (cA, cB) => {
                double divR = (cB.R == 0) ? 1 : (cB.R / 255.0);
                double divG = (cB.G == 0) ? 1 : (cB.G / 255.0);
                double divB = (cB.B == 0) ? 1 : (cB.B / 255.0);

                int r = Clamp((int)((cA.R / 255.0) / divR * 255.0));
                int g = Clamp((int)((cA.G / 255.0) / divG * 255.0));
                int b = Clamp((int)((cA.B / 255.0) / divB * 255.0));
                return Color.FromArgb(r, g, b);
            });
        }
    }
}