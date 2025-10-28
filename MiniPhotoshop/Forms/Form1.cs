using System;
using System.Drawing;
using System.Windows.Forms;
using MiniPhotoshop.Logic.Helpers;
using MiniPhotoshop.Logic.IO;
using MiniPhotoshop.Logic.Histogram;
using MiniPhotoshop.Logic.ImageProcessing;

namespace MiniPhotoshop
{
    public partial class Form1 : Form
    {
        private Bitmap originalImage = null;
        private int[,,] imageData3D = null;
        private bool isColorSelectionMode = false;

        public Form1()
        {
            InitializeComponent();

            // Event handler tombol Show Red, Green, Blue
            button5.Click += (s, e) => pictureBox1.Image = ImageDataProcessor.CreateBitmapFrom3DArray(imageData3D, 0);
            button6.Click += (s, e) => pictureBox1.Image = ImageDataProcessor.CreateBitmapFrom3DArray(imageData3D, 1);
            button7.Click += (s, e) => pictureBox1.Image = ImageDataProcessor.CreateBitmapFrom3DArray(imageData3D, 2);
            button4.Click += (s, e) => pictureBox1.Image = ImageDataProcessor.CreateBitmapFrom3DArray(imageData3D, 3);

            // Event handler menu histogram
            histogramRToolStripMenuItem.Click += (s, e) => TampilkanHistogramChannel(0);
            histogramGToolStripMenuItem.Click += (s, e) => TampilkanHistogramChannel(1);
            histogramBToolStripMenuItem.Click += (s, e) => TampilkanHistogramChannel(2);
            histogramGrToolStripMenuItem.Click += (s, e) => TampilkanHistogramChannel(3);

            // Event handler mode seleksi warna
            this.button8.Click += new System.EventHandler(this.button8_Click);
            this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseClick);

            // --- TAMBAHAN: Event handler untuk TrackBar Brightness ---
            this.trackBarBrightness.Scroll += new System.EventHandler(this.trackBarBrightness_Scroll);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                originalImage = new Bitmap(openFileDialog.FileName);
                imageData3D = ImageDataProcessor.LoadTo3DArray(originalImage);

                Bitmap bmpFromArray = ImageDataProcessor.CreateBitmapFrom3DArray(imageData3D);
                if (bmpFromArray != null)
                {
                    pictureBox1.Image = bmpFromArray;
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

                    isColorSelectionMode = false;
                    pictureBox1.Cursor = Cursors.Default;

                    // --- Reset nilai Brightness saat load gambar baru ---
                    trackBarBrightness.Value = 0;
                    lblBrightnessValue.Text = "Brightness: 0";
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

            Bitmap bmpToSave = new Bitmap(pictureBox1.Image);

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "File teks (*.txt) | *.txt";
                sfd.Title = "Simpan nilai gambar";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        FileExporter.ExportPixelDataToTxt(bmpToSave, sfd.FileName);
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
            if (originalImage != null)
            {
                pictureBox1.Image = (Bitmap)originalImage.Clone();

                isColorSelectionMode = false;
                pictureBox1.Cursor = Cursors.Default;

                // --- Reset nilai Brightness saat restore gambar ---
                trackBarBrightness.Value = 0;
                lblBrightnessValue.Text = "Brightness: 0";
            }
        }

        private void buttonGrayscale_Click(Object sender, EventArgs e)
        {
            if (imageData3D == null)
                return;
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

            int[] hist = HistogramCalculator.Calculate(bmp, channel);
            Bitmap histBmp = HistogramDrawer.Draw(hist, channel, width, height);

            pictureBoxHistogram.Image = histBmp;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (originalImage == null)
            {
                MessageBox.Show("Silakan muat gambar terlebih dahulu.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            isColorSelectionMode = true;
            pictureBox1.Cursor = Cursors.Cross;

            MessageBox.Show("Mode Seleksi Warna Aktif.\nKlik pada warna di gambar untuk menyeleksinya.",
                            "Mode Aktif", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (!isColorSelectionMode)
                return;

            if (originalImage != null)
            {
                Point imagePoint = CoordinateHelper.TranslateMouseClickToImagePoint(pictureBox1, e.Location);
                Color clickedColor = originalImage.GetPixel(imagePoint.X, imagePoint.Y);

                Bitmap processedImage = SelectionColor.ApplySelection(originalImage, clickedColor);
                pictureBox1.Image = processedImage;
            }

            isColorSelectionMode = false;
            pictureBox1.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Event handler yang dijalankan setiap kali slider brightness digeser.
        /// </summary>
        private void trackBarBrightness_Scroll(object sender, EventArgs e)
        {
            if (originalImage == null)
                return;

            int adjustment = trackBarBrightness.Value;
            lblBrightnessValue.Text = "Brightness: " + adjustment.ToString();

            Bitmap resultImage = Brightness.AdjustBrightness(originalImage, adjustment);
            pictureBox1.Image = resultImage;
        }
    }
}