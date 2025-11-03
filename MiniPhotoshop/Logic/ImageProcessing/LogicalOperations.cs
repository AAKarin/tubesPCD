using System;
using System.Drawing;

namespace MiniPhotoshop.Logic.ImageProcessing
{
    public static class LogicalOperations
    {
        // Helper untuk mengubah 1 piksel ke Biner (0 atau 255)
        private static int ToBinary(Color c)
        {
            int gray = (int)(c.R * 0.299 + c.G * 0.587 + c.B * 0.114);
            return (gray < 128) ? 0 : 255;
        }

        // Helper untuk mengubah 1 nilai biner (0/255) ke Warna
        private static Color ToBinaryColor(int val)
        {
            return Color.FromArgb(val, val, val);
        }

        // --- FUNGSI HELPER UNIVERSAL (BARU) ---
        private static Bitmap PerformOperation(Bitmap imgA, Bitmap imgB, Func<int, int, int> operation)
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
                        int valA = ToBinary(imgA.GetPixel(x, y));
                        int valB = ToBinary(imgB.GetPixel(x, y));
                        resultBmp.SetPixel(x, y, ToBinaryColor(operation(valA, valB)));
                    }
                    else if (inA)
                    {
                        // KASUS 2: Piksel hanya ada di A. Salin A (dijadikan biner).
                        int valA = ToBinary(imgA.GetPixel(x, y));
                        resultBmp.SetPixel(x, y, ToBinaryColor(valA));
                    }
                    else if (inB)
                    {
                        // KASUS 3: Piksel hanya ada di B. Salin B (dijadikan biner).
                        int valB = ToBinary(imgB.GetPixel(x, y));
                        resultBmp.SetPixel(x, y, ToBinaryColor(valB));
                    }
                    else
                    {
                        resultBmp.SetPixel(x, y, Color.Black);
                    }
                }
            }
            return resultBmp;
        }


        // --- FUNGSI PUBLIK (dipanggil oleh OperationManager) ---

        public static Bitmap And(Bitmap imgA, Bitmap imgB)
        {
            // Logika AND: 255 HANYA jika A=255 DAN B=255
            return PerformOperation(imgA, imgB, (valA, valB) => {
                return (valA == 255 && valB == 255) ? 255 : 0;
            });
        }

        public static Bitmap Or(Bitmap imgA, Bitmap imgB)
        {
            // Logika OR: 255 jika A=255 ATAU B=255
            return PerformOperation(imgA, imgB, (valA, valB) => {
                return (valA == 255 || valB == 255) ? 255 : 0;
            });
        }

        public static Bitmap Xor(Bitmap imgA, Bitmap imgB)
        {
            // Logika XOR: 255 jika A != B
            return PerformOperation(imgA, imgB, (valA, valB) => {
                return (valA != valB) ? 255 : 0;
            });
        }

        // FUNGSI NOT (Negasi) - Ini hanya berlaku untuk Gambar A
        public static Bitmap Not(Bitmap imgA)
        {
            Bitmap resultBmp = new Bitmap(imgA.Width, imgA.Height);
            for (int y = 0; y < imgA.Height; y++)
            {
                for (int x = 0; x < imgA.Width; x++)
                {
                    int valA = ToBinary(imgA.GetPixel(x, y));
                    int valRes = (valA == 255) ? 0 : 255; // Balikkan
                    resultBmp.SetPixel(x, y, ToBinaryColor(valRes));
                }
            }
            return resultBmp;
        }
    }
}