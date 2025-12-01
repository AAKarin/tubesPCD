using System;
using System.Windows.Forms;

namespace MiniPhotoshop.Logic.ImageProcessing
{
    // ---------------------------------------------------------
    // 1. Struktur Data Kernel
    // ---------------------------------------------------------
    public class KernelData
    {
        public double[,] Matrix { get; set; }
        public double Divisor { get; set; }
        public int Offset { get; set; }

        public KernelData(double[,] m, double d, int o)
        {
            Matrix = m;
            Divisor = d;
            Offset = o;
        }
    }

    // ---------------------------------------------------------
    // 2. Database Generator Kernel
    // ---------------------------------------------------------
    public static class KernelDatabase
    {
        /// <summary>
        /// Menghasilkan objek KernelData berdasarkan mode dan ukuran n.
        /// </summary>
        /// <param name="mode">Nama filter (contoh: "Blur (Box)", "Sobel Horizontal")</param>
        /// <param name="n">Ukuran matriks (n x n)</param>
        /// <returns>Objek KernelData berisi matriks dan divisor</returns>
        public static KernelData GetKernel(string mode, int n)
        {
            // Inisialisasi Default: Matriks kosong
            double[,] kernel = new double[n, n];
            double divisor = 1;
            int offset = 0;
            Random rnd = new Random();

            // --- LOGIKA FILTER ---
            // 1. RANDOM
            if (mode == "Random")
            {
                for (int r = 0; r < n; r++)
                    for (int c = 0; c < n; c++)
                        kernel[r, c] = rnd.Next(-5, 6);
                divisor = 1;
            }
            // 2. BLUR BOX (Mean Filter)
            else if (mode.Contains("Blur (Box)"))
            {
                for (int r = 0; r < n; r++)
                    for (int c = 0; c < n; c++)
                        kernel[r, c] = 1;
                divisor = n * n; // Normalisasi
            }
            // 3. GAUSSIAN BLUR (Hardcoded 3x3)
            else if (mode.Contains("Gaussian"))
            {
                if (n == 3)
                {
                    kernel = new double[,] {
                        { 1, 2, 1 },
                        { 2, 4, 2 },
                        { 1, 2, 1 }
                    };
                    divisor = 16;
                }
                else MessageBox.Show("Gaussian preset hanya optimal untuk ukuran 3x3");
            }
            // 4. SHARPEN
            else if (mode.Contains("Sharpen"))
            {
                int mid = n / 2;
                kernel[mid, mid] = 5;
                // Tetangga Cross (Atas, Bawah, Kiri, Kanan) jadi -1
                if (mid - 1 >= 0) kernel[mid - 1, mid] = -1;
                if (mid + 1 < n) kernel[mid + 1, mid] = -1;
                if (mid - 1 >= 0) kernel[mid, mid - 1] = -1;
                if (mid + 1 < n) kernel[mid, mid + 1] = -1;
                divisor = 1;
            }
            // 5. EDGE SIMPLE
            else if (mode.Contains("Edge (Simple)"))
            {
                int mid = n / 2;
                kernel[mid, mid] = 4;
                if (mid - 1 >= 0) kernel[mid - 1, mid] = -1;
                if (mid + 1 < n) kernel[mid + 1, mid] = -1;
                if (mid - 1 >= 0) kernel[mid, mid - 1] = -1;
                if (mid + 1 < n) kernel[mid, mid + 1] = -1;
                divisor = 1;
            }
            // 6. LAPLACIAN
            else if (mode.Contains("Laplacian"))
            {
                for (int r = 0; r < n; r++)
                    for (int c = 0; c < n; c++)
                        kernel[r, c] = -1;
                kernel[n / 2, n / 2] = (n * n) - 1;
                divisor = 1;
            }
            // 7. SOBEL HORIZONTAL (Maksimal 5x5)
            else if (mode.Contains("Sobel Horizontal"))
            {
                if (n == 3)
                    kernel = new double[,] { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } };
                else if (n == 5)
                    kernel = new double[,] {
                        { -1, -2, 0, 2, 1 }, { -4, -8, 0, 8, 4 },
                        { -6, -12, 0, 12, 6 },
                        { -4, -8, 0, 8, 4 }, { -1, -2, 0, 2, 1 }
                    };
                else
                    ShowLimitWarning("Sobel Horizontal");
                divisor = 1;
            }
            // 8. SOBEL VERTICAL (Maksimal 5x5)
            else if (mode.Contains("Sobel Vertical"))
            {
                if (n == 3)
                    kernel = new double[,] { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
                else if (n == 5)
                    kernel = new double[,] {
                        { -1, -4, -6, -4, -1 }, { -2, -8, -12, -8, -2 },
                        { 0, 0, 0, 0, 0 },
                        { 2, 8, 12, 8, 2 }, { 1, 4, 6, 4, 1 }
                    };
                else
                    ShowLimitWarning("Sobel Vertical");
                divisor = 1;
            }
            // 9. EMBOSS (Maksimal 5x5)
            else if (mode.Contains("Emboss"))
            {
                if (n == 3)
                    kernel = new double[,] { { -2, -1, 0 }, { -1, 1, 1 }, { 0, 1, 2 } };
                else if (n == 5)
                    kernel = new double[,] {
                        { -1, -1, -1, -1, 0 }, { -1, -1, -1, 0, 1 },
                        { -1, -1, 1, 1, 1 },
                        { -1, 0, 1, 1, 1 }, { 0, 1, 1, 1, 1 }
                    };
                else
                    ShowLimitWarning("Emboss");
                divisor = 1;
            }

            return new KernelData(kernel, divisor, offset);
        }

        // Helper untuk menampilkan peringatan UI
        private static void ShowLimitWarning(string featureName)
        {
            MessageBox.Show(
                $"Untuk alasan performa memori, fitur '{featureName}' dibatasi maksimal ukuran 5x5.\n\n" +
                "Silakan pilih ukuran 3x3 atau 5x5.",
                "Ukuran Kernel Terlalu Besar",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
        }
    }
}
