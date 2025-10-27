using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;

namespace MiniPhotoshop.Logic.Histogram
{
    public static class HistogramDrawer
    {
        // Menggambar histogram ke Bitmap baru
        public static Bitmap Draw(int[] hist, int channel, int width, int height)
        {
            if (hist == null || hist.Length != 256) return new Bitmap(width, height);

            Bitmap histBmp = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(histBmp))
            {
                g.Clear(Color.Black);
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // Salin semua kode penggambaran sumbu, label, dan garis
                // ... (Salin sisa kode dari TampilkanHistogramChannel mulai dari "Bitmap histBmp = new Bitmap(width, height);")

                Pen axisPen = new Pen(Color.White, 1);
                Brush textBrush = Brushes.White;
                Font font = new Font("Arial", 8);

                int marginLeft = 60;
                int marginBottom = 30;

                // Gambar sumbu X dan Y
                g.DrawLine(axisPen, marginLeft, height - marginBottom, width - 10, height - marginBottom); // Sumbu X
                g.DrawLine(axisPen, marginLeft, 10, marginLeft, height - marginBottom); // Sumbu Y

                // Label sumbu X
                int xAxisLength = width - marginLeft - 10;
                // ... (Logika Label sumbu X)
                int labelCountX = 5;
                for (int i = 0; i <= labelCountX; i++)
                {
                    int val = i * 255 / labelCountX;
                    float xPos = marginLeft + (val / 255f) * xAxisLength;
                    float yPos = height - marginBottom;

                    g.DrawLine(Pens.White, xPos, yPos, xPos, yPos + 5);

                    string label = val.ToString();
                    SizeF size = g.MeasureString(label, font);
                    g.DrawString(label, font, textBrush, xPos - size.Width / 2, yPos + 5);
                }

                // Label sumbu Y
                int max = hist.Max();
                if (max == 0) max = 1;
                int labelCountY = 5;
                int labelMarginRight = 10;
                for (int i = 0; i <= labelCountY; i++)
                {
                    int val = i * max / labelCountY;
                    float yPos = height - marginBottom - (val / (float)max) * (height - 40);

                    g.DrawLine(Pens.White, marginLeft, yPos, marginLeft - 5, yPos);

                    string label = val.ToString();
                    SizeF size = g.MeasureString(label, font);
                    g.DrawString(label, font, textBrush, marginLeft - labelMarginRight - size.Width, yPos - size.Height / 2);
                }

                // Pilih warna garis histogram
                Pen pen;
                switch (channel)
                {
                    case 0: pen = Pens.Red; break;
                    case 1: pen = Pens.Lime; break;
                    case 2: pen = Pens.Blue; break;
                    case 3: pen = Pens.White; break;
                    default: pen = Pens.Gray; break;
                }

                // Gambar histogram
                PointF[] points = new PointF[256];
                for (int i = 0; i < 256; i++)
                {
                    float xPos = marginLeft + (i / 255f) * xAxisLength;
                    float yPos = height - marginBottom - ((hist[i] / (float)max) * (height - 40));
                    points[i] = new PointF(xPos, yPos);
                }

                g.DrawLines(pen, points);
            }

            return histBmp;
        }
    }
}
