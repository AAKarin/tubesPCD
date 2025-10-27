using System.Drawing.Drawing2D;
using System.Text;
using System.IO;
using System;

namespace MiniPhotoshop.Logic.IO
{
    public static class FileExporter
    {
        public static void ExportPixelDataToTxt(Bitmap bmp, string filePath)
        {
            if (bmp == null)
            {
                throw new ArgumentNullException(nameof(bmp), "Bitmap tidak boleh null.");
            }

            var sb = new StringBuilder();

            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    Color pixelColor = bmp.GetPixel(x, y);
                    sb.Append($"{x},{y}: R={pixelColor.R}, G={pixelColor.G}, B={pixelColor.B}\n");
                }
            }

            // Memastikan penggunaan System.IO
            File.WriteAllText(filePath, sb.ToString());
        }
    }
}
