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

        public static Bitmap Add(Bitmap imgA, Bitmap imgB)
        {
            Bitmap bmpB = new Bitmap(imgB, imgA.Size);
            Bitmap resultBmp = new Bitmap(imgA.Width, imgA.Height);
            for (int y = 0; y < imgA.Height; y++)
            {
                for (int x = 0; x < imgA.Width; x++)
                {
                    Color cA = imgA.GetPixel(x, y);
                    Color cB = bmpB.GetPixel(x, y);
                    int newR = Clamp(cA.R + cB.R);
                    int newG = Clamp(cA.G + cB.G);
                    int newB = Clamp(cA.B + cB.B);
                    resultBmp.SetPixel(x, y, Color.FromArgb(newR, newG, newB));
                }
            }
            bmpB.Dispose();
            return resultBmp;
        }

        public static Bitmap Subtract(Bitmap imgA, Bitmap imgB)
        {
            Bitmap bmpB = new Bitmap(imgB, imgA.Size);
            Bitmap resultBmp = new Bitmap(imgA.Width, imgA.Height);
            for (int y = 0; y < imgA.Height; y++)
            {
                for (int x = 0; x < imgA.Width; x++)
                {
                    Color cA = imgA.GetPixel(x, y);
                    Color cB = bmpB.GetPixel(x, y);
                    int newR = Clamp(cA.R - cB.R);
                    int newG = Clamp(cA.G - cB.G);
                    int newB = Clamp(cA.B - cB.B);
                    resultBmp.SetPixel(x, y, Color.FromArgb(newR, newG, newB));
                }
            }
            bmpB.Dispose();
            return resultBmp;
        }

        public static Bitmap Multiply(Bitmap imgA, Bitmap imgB)
        {
            Bitmap bmpB = new Bitmap(imgB, imgA.Size);
            Bitmap resultBmp = new Bitmap(imgA.Width, imgA.Height);
            for (int y = 0; y < imgA.Height; y++)
            {
                for (int x = 0; x < imgA.Width; x++)
                {
                    Color cA = imgA.GetPixel(x, y);
                    Color cB = bmpB.GetPixel(x, y);
                    int newR = Clamp((int)((cA.R / 255.0) * (cB.R / 255.0) * 255.0));
                    int newG = Clamp((int)((cA.G / 255.0) * (cB.G / 255.0) * 255.0));
                    int newB = Clamp((int)((cA.B / 255.0) * (cB.B / 255.0) * 255.0));
                    resultBmp.SetPixel(x, y, Color.FromArgb(newR, newG, newB));
                }
            }
            bmpB.Dispose();
            return resultBmp;
        }

        public static Bitmap Divide(Bitmap imgA, Bitmap imgB)
        {
            Bitmap bmpB = new Bitmap(imgB, imgA.Size);
            Bitmap resultBmp = new Bitmap(imgA.Width, imgA.Height);
            for (int y = 0; y < imgA.Height; y++)
            {
                for (int x = 0; x < imgA.Width; x++)
                {
                    Color cA = imgA.GetPixel(x, y);
                    Color cB = bmpB.GetPixel(x, y);
                    double divR = (cB.R == 0) ? 1 : (cB.R / 255.0);
                    double divG = (cB.G == 0) ? 1 : (cB.G / 255.0);
                    double divB = (cB.B == 0) ? 1 : (cB.B / 255.0);
                    int newR = Clamp((int)((cA.R / 255.0) / divR * 255.0));
                    int newG = Clamp((int)((cA.G / 255.0) / divG * 255.0));
                    int newB = Clamp((int)((cA.B / 255.0) / divB * 255.0));
                    resultBmp.SetPixel(x, y, Color.FromArgb(newR, newG, newB));
                }
            }
            bmpB.Dispose();
            return resultBmp;
        }
    }
}