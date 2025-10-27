using System.Drawing.Drawing2D;

namespace MiniPhotoshop.Logic.ImageProcessing
{
    public static class ImageNegation
    {
        public static Bitmap Apply(Bitmap currentImage)
        {
            Bitmap resultImage = new Bitmap(currentImage.Width, currentImage.Height);

            for (int y = 0; y < currentImage.Height; y++)
            {
                for (int x = 0; x < currentImage.Width; x++)
                {
                    Color originalColor = currentImage.GetPixel(x, y);

                    int newR = 255 - originalColor.R;
                    int newG = 255 - originalColor.G;
                    int newB = 255 - originalColor.B;

                    Color newColor = Color.FromArgb(originalColor.A, newR, newG, newB);
                    resultImage.SetPixel(x, y, newColor);
                }
            }
            return resultImage;
        }
    }
}