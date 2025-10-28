using System.Drawing.Drawing2D;
using System.Drawing.Imaging; 
using System.Runtime.InteropServices; 

namespace MiniPhotoshop.Logic.ImageProcessing
{
    public static class ImageNegation
    {
        public static Bitmap Apply(Bitmap currentImage)
        {
            Bitmap resultImage = new Bitmap(currentImage); // Buat salinan
            Rectangle rect = new Rectangle(0, 0, resultImage.Width, resultImage.Height);

            BitmapData bmpData = resultImage.LockBits(rect, ImageLockMode.ReadWrite,
                                                    resultImage.PixelFormat);

            IntPtr ptr = bmpData.Scan0;
            int bytes = Math.Abs(bmpData.Stride) * resultImage.Height;
            byte[] rgbValues = new byte[bytes];

            // Salin data ke array
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            int bytesPerPixel = Image.GetPixelFormatSize(resultImage.PixelFormat) / 8;
            int stride = bmpData.Stride;

            for (int y = 0; y < resultImage.Height; y++)
            {
                int rowOffset = y * stride;
                for (int x = 0; x < resultImage.Width; x++)
                {
                    int i = rowOffset + (x * bytesPerPixel);

                    // Logika Negasi (jangan sentuh Alpha/kanal ke-4)
                    rgbValues[i] = (byte)(255 - rgbValues[i]);     // Blue
                    rgbValues[i + 1] = (byte)(255 - rgbValues[i + 1]); // Green
                    rgbValues[i + 2] = (byte)(255 - rgbValues[i + 2]); // Red
                }
            }

            // Kembalikan data ke gambar
            Marshal.Copy(rgbValues, 0, ptr, bytes);
            resultImage.UnlockBits(bmpData);

            return resultImage;
        }
    }
}