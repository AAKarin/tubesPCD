using System;
using System.Drawing;

namespace MiniPhotoshop.Logic.ImageProcessing
{
    /// <summary>
    /// Kelas ini menyediakan fungsi utama untuk operasi logika biner antara dua gambar.
    /// Mendukung operasi AND, OR, XOR, dan operasi tunggal NOT.
    /// </summary>
    public static class LogicalOperations
    {
        // -------------------------------
        // 1. Metode Pembantu Internal
        // -------------------------------

        /// <summary>
        /// Mengonversi satu piksel berwarna menjadi nilai biner (0 atau 255)
        /// berdasarkan tingkat kecerahan (grayscale).
        /// </summary>
        private static int ToBinary(Color c)
        {
            int gray = (int)(c.R * 0.299 + c.G * 0.587 + c.B * 0.114);
            return (gray < 128) ? 0 : 255;
        }

        /// <summary>
        /// Mengonversi nilai biner kembali menjadi warna (hitam/putih).
        /// </summary>
        private static Color ToBinaryColor(int val)
        {
            return Color.FromArgb(val, val, val);
        }

        // -------------------------------
        // 2. Metode Privat Utama
        // -------------------------------

        /// <summary>
        /// Menjalankan operasi logika antara dua gambar.
        /// </summary>
        private static Bitmap PerformOperation(Bitmap imgA, Bitmap imgB, Func<int, int, int> operation)
        {
            int newWidth = Math.Max(imgA.Width, imgB.Width);
            int newHeight = Math.Max(imgA.Height, imgB.Height);
            Bitmap resultBmp = new Bitmap(newWidth, newHeight);

            for (int y = 0; y < newHeight; y++)
            {
                for (int x = 0; x < newWidth; x++)
                {
                    bool inA = (x < imgA.Width && y < imgA.Height);
                    bool inB = (x < imgB.Width && y < imgB.Height);

                    if (inA && inB)
                    {
                        // KASUS 1: Piksel saling bertemu
                        int valA = ToBinary(imgA.GetPixel(x, y));
                        int valB = ToBinary(imgB.GetPixel(x, y));
                        resultBmp.SetPixel(x, y, ToBinaryColor(operation(valA, valB)));
                    }
                    else if (inA)
                    {
                        // KASUS 2: Piksel hanya ada di gambar A
                        resultBmp.SetPixel(x, y, imgA.GetPixel(x, y));
                    }
                    else if (inB)
                    {
                        // KASUS 3: Piksel hanya ada di gambar B
                        resultBmp.SetPixel(x, y, imgB.GetPixel(x, y));
                    }
                    else
                    {
                        // KASUS 4: Area kosong (di luar A dan B)
                        resultBmp.SetPixel(x, y, Color.Black);
                    }
                }
            }

            return resultBmp;
        }

        // -------------------------------
        // 3. Metode Publik (API)
        // -------------------------------

        /// <summary>
        /// Operasi logika AND antara dua gambar.
        /// </summary>
        public static Bitmap And(Bitmap imgA, Bitmap imgB)
        {
            return PerformOperation(imgA, imgB, (valA, valB) =>
            {
                return (valA == 255 && valB == 255) ? 255 : 0;
            });
        }

        /// <summary>
        /// Operasi logika OR antara dua gambar.
        /// </summary>
        public static Bitmap Or(Bitmap imgA, Bitmap imgB)
        {
            return PerformOperation(imgA, imgB, (valA, valB) =>
            {
                return (valA == 255 || valB == 255) ? 255 : 0;
            });
        }

        /// <summary>
        /// Operasi logika XOR antara dua gambar.
        /// </summary>
        public static Bitmap Xor(Bitmap imgA, Bitmap imgB)
        {
            return PerformOperation(imgA, imgB, (valA, valB) =>
            {
                return (valA != valB) ? 255 : 0;
            });
        }

        /// <summary>
        /// Operasi logika NOT (negasi) pada satu gambar.
        /// </summary>
        public static Bitmap Not(Bitmap imgA)
        {
            Bitmap resultBmp = new Bitmap(imgA.Width, imgA.Height);

            for (int y = 0; y < imgA.Height; y++)
            {
                for (int x = 0; x < imgA.Width; x++)
                {
                    int valA = ToBinary(imgA.GetPixel(x, y));
                    int valRes = (valA == 255) ? 0 : 255; // Balikkan nilai biner
                    resultBmp.SetPixel(x, y, ToBinaryColor(valRes));
                }
            }

            return resultBmp;
        }
    }
}
//test