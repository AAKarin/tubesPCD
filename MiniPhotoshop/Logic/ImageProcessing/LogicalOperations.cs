using System;
using System.Drawing;

namespace MiniPhotoshop.Logic.ImageProcessing
{
    public static class LogicalOperations
    {
        private static Bitmap ToBinary(Bitmap img)
        {
            Bitmap resultBmp = new Bitmap(img.Width, img.Height);
            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                {
                    Color c = img.GetPixel(x, y);
                    int gray = (int)(c.R * 0.299 + c.G * 0.587 + c.B * 0.114);
                    int val = (gray < 128) ? 0 : 255;
                    resultBmp.SetPixel(x, y, Color.FromArgb(val, val, val));
                }
            }
            return resultBmp;
        }

        public static Bitmap And(Bitmap imgA, Bitmap imgB)
        {
            using (Bitmap binA = ToBinary(imgA))
            using (Bitmap binB = new Bitmap(ToBinary(imgB), binA.Size))
            {
                Bitmap resultBmp = new Bitmap(binA.Width, binA.Height);
                for (int y = 0; y < binA.Height; y++)
                {
                    for (int x = 0; x < binA.Width; x++)
                    {
                        int valA = binA.GetPixel(x, y).R;
                        int valB = binB.GetPixel(x, y).R;
                        int valRes = (valA == 255 && valB == 255) ? 255 : 0;
                        resultBmp.SetPixel(x, y, Color.FromArgb(valRes, valRes, valRes));
                    }
                }
                return resultBmp;
            }
        }

        public static Bitmap Or(Bitmap imgA, Bitmap imgB)
        {
            using (Bitmap binA = ToBinary(imgA))
            using (Bitmap binB = new Bitmap(ToBinary(imgB), binA.Size))
            {
                Bitmap resultBmp = new Bitmap(binA.Width, binA.Height);
                for (int y = 0; y < binA.Height; y++)
                {
                    for (int x = 0; x < binA.Width; x++)
                    {
                        int valA = binA.GetPixel(x, y).R;
                        int valB = binB.GetPixel(x, y).R;
                        int valRes = (valA == 255 || valB == 255) ? 255 : 0;
                        resultBmp.SetPixel(x, y, Color.FromArgb(valRes, valRes, valRes));
                    }
                }
                return resultBmp;
            }
        }

        public static Bitmap Xor(Bitmap imgA, Bitmap imgB)
        {
            using (Bitmap binA = ToBinary(imgA))
            using (Bitmap binB = new Bitmap(ToBinary(imgB), binA.Size))
            {
                Bitmap resultBmp = new Bitmap(binA.Width, binA.Height);
                for (int y = 0; y < binA.Height; y++)
                {
                    for (int x = 0; x < binA.Width; x++)
                    {
                        int valA = binA.GetPixel(x, y).R;
                        int valB = binB.GetPixel(x, y).R;
                        int valRes = (valA != valB) ? 255 : 0;
                        resultBmp.SetPixel(x, y, Color.FromArgb(valRes, valRes, valRes));
                    }
                }
                return resultBmp;
            }
        }

        public static Bitmap Not(Bitmap imgA)
        {
            using (Bitmap binA = ToBinary(imgA))
            {
                Bitmap resultBmp = new Bitmap(binA.Width, binA.Height);
                for (int y = 0; y < binA.Height; y++)
                {
                    for (int x = 0; x < binA.Width; x++)
                    {
                        int valA = binA.GetPixel(x, y).R;
                        int valRes = (valA == 255) ? 0 : 255;
                        resultBmp.SetPixel(x, y, Color.FromArgb(valRes, valRes, valRes));
                    }
                }
                return resultBmp;
            }
        }
    }
}