namespace MiniPhotoshop
{
    public partial class Form1 : Form
    {
        private Bitmap originalImage = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    originalImage = new Bitmap(openFileDialog.FileName);
                    pictureBox1.Image = (Bitmap)originalImage.Clone();
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

        //tambahkan 2 method untuk grayscale dan restore color
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
    }
}
