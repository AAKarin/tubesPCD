using System;
using System.Drawing;
using System.Drawing.Imaging;
// PENTING: Panggil namespace folder EdgeDetection agar file Robert.cs dikenali
using MiniPhotoshop.Logic.EdgeDetection;

namespace MiniPhotoshop.Logic.Helpers
{
    public class EdgeDetectionManager
    {
        // -------------------------------------------------------------
        // 1. ROBERT (TUGAS ANDA - SUDAH JADI)
        // -------------------------------------------------------------
        public Bitmap ProcessRobert(Bitmap source)
        {
            // Langkah 1: Ubah ke Grayscale dulu (Wajib untuk akurasi)
            Bitmap gray = MakeGrayscale(source);

            // Langkah 2: Panggil Worker Robert
            Robert worker = new Robert();
            return worker.Apply(gray);
        }

        // -------------------------------------------------------------
        // 2. CANNY (TUGAS ANDA - AKAN DATANG)
        // -------------------------------------------------------------

        public Bitmap ProcessCanny(Bitmap source)
        {
            if (source == null) return null;

            Bitmap gray = MakeGrayscale(source);

            // SEBELUMNYA: 20f, 100f (Terlalu Tinggi!)
            // SEKARANG: Turunkan drastis agar garis halus pun terdeteksi
            // Low: 5f (Sangat sensitif)
            // High: 20f (Cukup longgar)
            Canny worker = new Canny(5f, 20f);

            return worker.Apply(gray);
        }

        // -------------------------------------------------------------
        // HELPER: GRAYSCALE CONVERTER (Standard Rec. 601)
        // -------------------------------------------------------------
        private Bitmap MakeGrayscale(Bitmap original)
        {
            Bitmap newBmp = new Bitmap(original.Width, original.Height);
            using (Graphics g = Graphics.FromImage(newBmp))
            {
                ColorMatrix colorMatrix = new ColorMatrix(
                   new float[][]
                   {
                      new float[] {.299f, .299f, .299f, 0, 0},
                      new float[] {.587f, .587f, .587f, 0, 0},
                      new float[] {.114f, .114f, .114f, 0, 0},
                      new float[] {0, 0, 0, 1, 0},
                      new float[] {0, 0, 0, 0, 1}
                   });
                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(colorMatrix);
                g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
                   0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
            }
            return newBmp;
        }
    }
}