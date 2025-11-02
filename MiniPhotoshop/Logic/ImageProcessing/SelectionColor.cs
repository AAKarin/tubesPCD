using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices; 

namespace MiniPhotoshop.Logic.ImageProcessing
{
    public static class SelectionColor
    {
        public static Bitmap ApplySelection(Bitmap originalImage, Color selectedColor)
        {
            Bitmap resultImage = new Bitmap(originalImage.Width, originalImage.Height,originalImage.PixelFormat);
            int targetArgb = selectedColor.ToArgb();
            Rectangle rect = new Rectangle(0, 0, originalImage.Width, originalImage.Height);

            BitmapData originalData = originalImage.LockBits(rect, ImageLockMode.ReadOnly,originalImage.PixelFormat);
            BitmapData resultData = resultImage.LockBits(rect, ImageLockMode.WriteOnly,resultImage.PixelFormat);

            IntPtr ptrAsli = originalData.Scan0;
            IntPtr ptrHasil = resultData.Scan0;

            int bytes = Math.Abs(originalData.Stride) * originalImage.Height;
            byte[] rgbAsli = new byte[bytes];
            byte[] rgbHasil = new byte[bytes];

            Marshal.Copy(ptrAsli, rgbAsli, 0, bytes);

            int bytesPerPixel = Image.GetPixelFormatSize(originalImage.PixelFormat) / 8;
            int stride = originalData.Stride;

            for (int y = 0; y < originalImage.Height; y++)
            {
                int rowOffset = y * stride;
                for (int x = 0; x < originalImage.Width; x++)
                {
                    int i = rowOffset + (x * bytesPerPixel);

                    byte b = rgbAsli[i];
                    byte g = rgbAsli[i + 1];
                    byte r = rgbAsli[i + 2];
                    byte a = (bytesPerPixel == 4) ? rgbAsli[i + 3] : (byte)255;

                    int currentArgb = (a << 24) | (r << 16) | (g << 8) | b;

                    if (currentArgb == targetArgb)
                    {
                        rgbHasil[i] = b;
                        rgbHasil[i + 1] = g;
                        rgbHasil[i + 2] = r;
                        if (bytesPerPixel == 4) rgbHasil[i + 3] = a;
                    }
                    else
                    {
                        rgbHasil[i] = 255;
                        rgbHasil[i + 1] = 255;
                        rgbHasil[i + 2] = 255;
                        if (bytesPerPixel == 4) rgbHasil[i + 3] = a;
                    }
                }
            }

            Marshal.Copy(rgbHasil, 0, ptrHasil, bytes);

            originalImage.UnlockBits(originalData);
            resultImage.UnlockBits(resultData);

            return resultImage;
        }
    }
}