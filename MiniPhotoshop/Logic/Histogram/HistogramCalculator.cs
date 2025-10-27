using System.Drawing.Drawing2D;

namespace MiniPhotoshop.Logic.Histogram
{
    public static class HistogramCalculator
    {
        // Menghitung array histogram dari Bitmap untuk channel tertentu
        public static int[] Calculate(Bitmap bmp, int channel)
        {
            int[] hist = new int[256];

            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    Color pixelColor = bmp.GetPixel(x, y);
                    int val = 0;

                    switch (channel)
                    {
                        case 0: // Red
                            val = pixelColor.R;
                            break;
                        case 1: // Green
                            val = pixelColor.G;
                            break;
                        case 2: // Blue
                            val = pixelColor.B;
                            break;
                        case 3: // Grayscale (Penting: gunakan rumus Grayscale)
                            val = (int)(0.3 * pixelColor.R + 0.59 * pixelColor.G + 0.11 * pixelColor.B);
                            break;
                        default:
                            val = pixelColor.R;
                            break;
                    }

                    // Pastikan val berada dalam batas [0, 255]
                    if (val >= 0 && val <= 255)
                    {
                        hist[val]++;
                    }
                }
            }
            return hist;
        }
    }
}
