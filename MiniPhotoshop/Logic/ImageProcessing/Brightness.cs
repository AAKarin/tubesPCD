using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace MiniPhotoshop.Logic.ImageProcessing
{
    public static class Brightness
    {
        public static Bitmap AdjustBrightness(Bitmap originalImage, int adjustment)
        {
            Bitmap resultImage = new Bitmap(originalImage);

            Rectangle rect = new Rectangle(0, 0, resultImage.Width, resultImage.Height);
            BitmapData bmpData = resultImage.LockBits(rect, ImageLockMode.ReadWrite,resultImage.PixelFormat);

            IntPtr ptr = bmpData.Scan0;

            int bytes = Math.Abs(bmpData.Stride) * resultImage.Height;
            byte[] rgbValues = new byte[bytes];

            Marshal.Copy(ptr, rgbValues, 0, bytes);

            int bytesPerPixel = Image.GetPixelFormatSize(resultImage.PixelFormat) / 8;
            int stride = bmpData.Stride;

            for (int y = 0; y < resultImage.Height; y++)
            {
                int rowOffset = y * stride;
                for (int x = 0; x < resultImage.Width; x++)
                {
                    int i = rowOffset + (x * bytesPerPixel);

                    for (int c = 0; c < 3; c++)
                    {

                        int newValue = rgbValues[i + c] + adjustment;

                        if (newValue > 255) newValue = 255;
                        if (newValue < 0) newValue = 0;

                        rgbValues[i + c] = (byte)newValue;
                    }
                }
            }

            Marshal.Copy(rgbValues, 0, ptr, bytes);
            resultImage.UnlockBits(bmpData);

            return resultImage;
        }
    }
}