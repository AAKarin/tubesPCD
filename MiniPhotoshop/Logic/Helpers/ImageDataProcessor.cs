using System.Drawing.Drawing2D;

namespace MiniPhotoshop.Logic.Helpers
{
    public static class ImageDataProcessor
    {
        public static int[,,] LoadTo3DArray(Bitmap bmp)
        {
            int width = bmp.Width;
            int height = bmp.Height;
            int[,,] imageData3D = new int[width, height, 4];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color pixelColor = bmp.GetPixel(x, y);
                    int r = pixelColor.R;
                    int g = pixelColor.G;
                    int b = pixelColor.B;
                    int gray = (int)(0.3 * r + 0.59 * g + 0.11 * b);

                    imageData3D[x, y, 0] = r;
                    imageData3D[x, y, 1] = g;
                    imageData3D[x, y, 2] = b;
                    imageData3D[x, y, 3] = gray;
                }
            }
            return imageData3D;
        }

        public static Bitmap CreateBitmapFrom3DArray(int[,,] imageData3D, int channel = -1)
        {
            if (imageData3D == null)
            {
                return null;
            }

            int width = imageData3D.GetLength(0);
            int height = imageData3D.GetLength(1);
            Bitmap bmp = new Bitmap(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int r = imageData3D[x, y, 0];
                    int g = imageData3D[x, y, 1];
                    int b = imageData3D[x, y, 2];
                    int gray = imageData3D[x, y, 3];

                    Color color;
                    if (channel == 0)
                    { // hanya red
                        color = Color.FromArgb(r, 0, 0);
                    }
                    else if (channel == 1)
                    { // hanya green
                        color = Color.FromArgb(0, g, 0);
                    }
                    else if (channel == 2)
                    { // hanya blue
                        color = Color.FromArgb(0, 0, b);
                    }
                    else if (channel == 3)
                    { // grayscale
                        color = Color.FromArgb(gray, gray, gray);
                    }
                    else
                    { // tampilkan full color
                        color = Color.FromArgb(r, g, b);
                    }
                    bmp.SetPixel(x, y, color);
                }
            }

            return bmp;
        }
    }
}
