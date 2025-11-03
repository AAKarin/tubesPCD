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

        // Helper untuk mengubah 1 nilai biner (0/255) ke Warna (Hitam/Putih)
        private static Color ToBinaryColor(int val)
        {
            return Color.FromArgb(val, val, val);
        }

        // --- FUNGSI HELPER UNIVERSAL (INI YANG DIPERBAIKI) ---
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
                        // KASUS 1: Piksel saling bertemu. Lakukan operasi BINER.
                        int valA = ToBinary(imgA.GetPixel(x, y));
                        int valB = ToBinary(imgB.GetPixel(x, y));
                        resultBmp.SetPixel(x, y, ToBinaryColor(operation(valA, valB)));
                    }
                    else if (inA)
                    {
                        // KASUS 2: Piksel hanya ada di A. Salin WARNA ASLI A.
                        resultBmp.SetPixel(x, y, imgA.GetPixel(x, y));
                    }
                    else if (inB)
                    {
                        // KASUS 3: Piksel hanya ada di B. Salin WARNA ASLI B.
                        resultBmp.SetPixel(x, y, imgB.GetPixel(x, y));
                    }
                    else
                    {
                        // KASUS 4: Area kosong (di luar A dan B)
                        resultBmp.SetPixel(x, y, Color.Black); // Diisi hitam
                    }
                }
            }
            return resultBmp;
        }


        // --- FUNGSI PUBLIK (dipanggil oleh OperationManager) ---

        public static Bitmap And(Bitmap imgA, Bitmap imgB)
        {
            return PerformOperation(imgA, imgB, (valA, valB) => {
                return (valA == 255 && valB == 255) ? 255 : 0;
            });
        }

        public static Bitmap Or(Bitmap imgA, Bitmap imgB)
        {
            return PerformOperation(imgA, imgB, (valA, valB) => {
                return (valA == 255 || valB == 255) ? 255 : 0;
            });
        }

        public static Bitmap Xor(Bitmap imgA, Bitmap imgB)
        {
            return PerformOperation(imgA, imgB, (valA, valB) => {
                return (valA != valB) ? 255 : 0;
            });
        }

        // FUNGSI NOT (TIDAK BERUBAH)
        // NOT adalah operasi 1 gambar. Sesuai screenshot Anda sebelumnya 
        // (image_da5a28.png), operasi ini mengubah SELURUH Gambar A
        // menjadi biner dan meng-inversinya.
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