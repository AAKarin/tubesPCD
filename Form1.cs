using System.Drawing.Drawing2D;

namespace MiniPhotoshop
{
    public partial class Form1 : Form
    {
        private Bitmap originalImage = null;
        private int[,,] imageData3D = null;

        public Form1()
        {
            InitializeComponent();

            // event handler untuk tombol Show Red, Green, Blue
            button5.Click += (s, e) => ShowOnlyColorChannel(0); // Red
            button6.Click += (s, e) => ShowOnlyColorChannel(1); // Green
            button7.Click += (s, e) => ShowOnlyColorChannel(2); // Blue

            // event handler menu histogram
            openToolStripMenuItem.Click += (s, e) => TampilkanHistogramChannel(0); // Histogram R
            saveToolStripMenuItem.Click += (s, e) => TampilkanHistogramChannel(1); // Histogram G
            histogramBToolStripMenuItem.Click += (s, e) => TampilkanHistogramChannel(2); // Histogram B
            histogramGrToolStripMenuItem.Click += (s, e) => TampilkanHistogramChannel(3); // Histogram Grayscale
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                originalImage = new Bitmap(openFileDialog.FileName);

                // Pindahkan data gambar ke array 3D
                LoadImageDataTo3DArray(originalImage);

                // Buat Bitmap baru dari array 3D dan tampilkan di pictureBox1
                Bitmap bmpFromArray = CreateBitmapFrom3DArray();
                if (bmpFromArray != null)
                {
                    pictureBox1.Image = bmpFromArray;
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Tidak ada gambar yang dimuat untuk disimpan", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Bitmap bmp = new Bitmap(pictureBox1.Image);

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "File teks (*.txt) | *.txt";
                sfd.Title = "Simpan nilai gambar";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var sb = new System.Text.StringBuilder();

                        for (int y = 0; y < bmp.Height; y++)
                        {
                            for (int x = 0; x < bmp.Width; x++)
                            {
                                Color pixelColor = bmp.GetPixel(x, y);
                                sb.Append($"{x},{y}: R={pixelColor.R}, G={pixelColor.G}, B={pixelColor.B}\n");
                            }
                        }

                        System.IO.File.WriteAllText(sfd.FileName, sb.ToString());
                        MessageBox.Show("Nilai gambar berhasil disimpan.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Gagal menyimpan file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void buttonRestore_Click(Object sender, EventArgs e)
        {
            if(originalImage != null)
            {
                pictureBox1.Image = (Bitmap)originalImage.Clone();
            }
        }

        private void buttonGrayscale_Click(Object sender, EventArgs e)
        {
            if (imageData3D == null)
            {
                return;
            }

            ShowOnlyColorChannel(3);
        }

        private void LoadImageDataTo3DArray(Bitmap bmp)
        {
            int width = bmp.Width;
            int height = bmp.Height;
            imageData3D = new int[width, height, 4]; // 4 channel: R, G, B, Grayscale

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
                    imageData3D[x, y, 3] = gray; // grayscale disimpan di channel ke-3
                }
            }
        }

        private Bitmap CreateBitmapFrom3DArray(int channel = -1)
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

                    if (channel == 0)
                    { // hanya red
                        bmp.SetPixel(x, y, Color.FromArgb(r, 0, 0));
                    }
                    else if (channel == 1)
                    { // hanya green
                        bmp.SetPixel(x, y, Color.FromArgb(0, g, 0));
                    }
                    else if (channel == 2)
                    { // hanya blue
                        bmp.SetPixel(x, y, Color.FromArgb(0, 0, b));
                    }
                    else if (channel == 3)
                    { // grayscale
                        bmp.SetPixel(x, y, Color.FromArgb(gray, gray, gray));
                    }
                    else
                    { // tampilkan full color
                        bmp.SetPixel(x, y, Color.FromArgb(r, g, b));
                    }
                }
            }

            return bmp;
        }

        private void ShowOnlyColorChannel(int channel)
        {
            if (imageData3D == null)
            {
                return;
            }

            int width = imageData3D.GetLength(0);
            int height = imageData3D.GetLength(1);

            Bitmap bmp = new Bitmap(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int r = 0, g = 0, b = 0;

                    if (channel == 0)
                    {
                        r = imageData3D[x, y, 0];
                    }
                    else if (channel == 1)
                    {
                        g = imageData3D[x, y, 1];
                    }
                    else if (channel == 2)
                    {
                        b = imageData3D[x, y, 2];
                    }
                    else if (channel == 3)
                    {
                        r = g = b = imageData3D[x, y, 3]; // grayscale
                    }

                    bmp.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }

            pictureBox1.Image = bmp;
        }

        private void TampilkanHistogramChannel(int channel)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Tidak ada gambar yang dimuat di PictureBox.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Bitmap bmp = new Bitmap(pictureBox1.Image);

            int width = pictureBoxHistogram.Width;
            int height = pictureBoxHistogram.Height;
            int[] hist = new int[256];

            // Hitung histogram dari bitmap di pictureBox1
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
                        case 3: // Grayscale
                            val = (int)(0.3 * pixelColor.R + 0.59 * pixelColor.G + 0.11 * pixelColor.B);
                            break;
                        default:
                            val = pixelColor.R; // default Red jika channel tidak valid
                            break;
                    }

                    hist[val]++;
                }
            }

            Bitmap histBmp = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(histBmp))
            {
                g.Clear(Color.Black);
                g.SmoothingMode = SmoothingMode.AntiAlias;

                Pen axisPen = new Pen(Color.White, 1);
                Brush textBrush = Brushes.White;
                Font font = new Font("Arial", 8);

                int marginLeft = 60; // Perbesar margin kiri untuk sumbu Y
                int marginBottom = 30;

                // Gambar sumbu X dan Y dengan margin baru
                g.DrawLine(axisPen, marginLeft, height - marginBottom, width - 10, height - marginBottom); // Sumbu X
                g.DrawLine(axisPen, marginLeft, 10, marginLeft, height - marginBottom); // Sumbu Y

                // Label sumbu X (0 sampai 255)
                int xAxisLength = width - marginLeft - 10;
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

                // Label sumbu Y (jumlah pixel)
                int max = hist.Max();
                if (max == 0) max = 1; // agar tidak ada pembagian nol
                int labelCountY = 5;
                int labelMarginRight = 10; // jarak label dari garis sumbu Y
                for (int i = 0; i <= labelCountY; i++)
                {
                    int val = i * max / labelCountY;
                    float yPos = height - marginBottom - (val / (float)max) * (height - 40);

                    g.DrawLine(Pens.White, marginLeft, yPos, marginLeft - 5, yPos);

                    string label = val.ToString();
                    SizeF size = g.MeasureString(label, font);
                    // Gambar label lebih ke kiri agar tidak terpotong
                    g.DrawString(label, font, textBrush, marginLeft - labelMarginRight - size.Width, yPos - size.Height / 2);
                }

                // Pilih warna garis histogram sesuai channel
                Pen pen;
                switch (channel)
                {
                    case 0:
                        pen = Pens.Red;
                        break;
                    case 1:
                        pen = Pens.Lime;
                        break;
                    case 2:
                        pen = Pens.Blue;
                        break;
                    case 3:
                        pen = Pens.White;
                        break;
                    default:
                        pen = Pens.Gray;
                        break;
                }

                // Gambar histogram dengan offset karena sumbu sudah digeser
                PointF[] points = new PointF[256];
                for (int i = 0; i < 256; i++)
                {
                    float xPos = marginLeft + (i / 255f) * xAxisLength;
                    float yPos = height - marginBottom - ((hist[i] / (float)max) * (height - 40));
                    points[i] = new PointF(xPos, yPos);
                }

                g.DrawLines(pen, points);
            }

            pictureBoxHistogram.Image = histBmp;
        }
    }
}
