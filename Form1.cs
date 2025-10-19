namespace MiniPhotoshop
{
    public partial class Form1 : Form
    {
        private Bitmap originalImage = null;
        private int[,,] imageData3D = null;

        public Form1()
        {
            InitializeComponent();

            // Tambahkan event handler untuk tombol Show Red, Green, Blue
            button5.Click += (s, e) => ShowOnlyColorChannel(0); // Red
            button6.Click += (s, e) => ShowOnlyColorChannel(1); // Green
            button7.Click += (s, e) => ShowOnlyColorChannel(2); // Blue
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
            if (originalImage == null)
            {
                return;
            }

            Bitmap grayBmp = new Bitmap(originalImage.Width, originalImage.Height);

            for (int y = 0; y < originalImage.Height; y++)
            {
                for (int x = 0; x < originalImage.Width; x++)
                {
                    Color originalColor = originalImage.GetPixel(x, y);
                    int grayValue = (int)(0.3 * originalColor.R + 0.59 * originalColor.G + 0.11 * originalColor.B);
                    Color grayColor = Color.FromArgb(grayValue, grayValue, grayValue);
                    grayBmp.SetPixel(x, y, grayColor);
                }
            }
            pictureBox1.Image = grayBmp;
        }

        private void LoadImageDataTo3DArray(Bitmap bmp)
        {
            int width = bmp.Width;
            int height = bmp.Height;
            imageData3D = new int[width, height, 3];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color pixelColor = bmp.GetPixel(x, y);
                    imageData3D[x, y, 0] = pixelColor.R;
                    imageData3D[x, y, 1] = pixelColor.G;
                    imageData3D[x, y, 2] = pixelColor.B;
                }
            }
        }

        private Bitmap CreateBitmapFrom3DArray()
        {
            if (imageData3D == null)
                return null;

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

                    r = Math.Min(255, Math.Max(0, r));
                    g = Math.Min(255, Math.Max(0, g));
                    b = Math.Min(255, Math.Max(0, b));

                    bmp.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }

            return bmp;
        }

        private void ShowOnlyColorChannel(int channel)
        {
            if (imageData3D == null)
                return;

            int width = imageData3D.GetLength(0);
            int height = imageData3D.GetLength(1);

            Bitmap bmp = new Bitmap(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int r = (channel == 0) ? imageData3D[x, y, 0] : 0;
                    int g = (channel == 1) ? imageData3D[x, y, 1] : 0;
                    int b = (channel == 2) ? imageData3D[x, y, 2] : 0;

                    bmp.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }

            pictureBox1.Image = bmp;
        }

    }
}
