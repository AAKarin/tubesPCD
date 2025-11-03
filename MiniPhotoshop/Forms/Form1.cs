using MiniPhotoshop.Forms;
using MiniPhotoshop.Helpers;
using MiniPhotoshop.Logic;
using MiniPhotoshop.Logic.Helpers;
using MiniPhotoshop.Logic.Helpers;
using MiniPhotoshop.Logic.Histogram;
using MiniPhotoshop.Logic.ImageProcessing;
using MiniPhotoshop.Logic.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace MiniPhotoshop
{
    public partial class Form1 : Form
    {
        private readonly ImageEditorService _editorService;
        private readonly UIManager _toolManager;
        private readonly DragDropManager _dragDropManager;
        private bool isColorSelectionMode = false;

        public Form1()
        {
            InitializeComponent();

            // Inisialisasi Service dan Manager
            _editorService = new ImageEditorService();

            // daftar semua kontrol yang akan dikelola
            var toolsToManage = new List<Control>
            {
                button2, // Save Gambar
                button3, // Restore Color
                button4, // Grayscale
                button5, // Show Red
                button6, // Show Green
                button7, // Show Blue
                button8, // Selection Tool
                button10, // Negation
                trackBarBlackWhite,
                trackBarBrightness,
            };
            _toolManager = new UIManager(
                toolsToManage,
                trackBarBrightness,
                lblBrightnessValue,
                trackBarBlackWhite
            );

            // Matikan semua alat saat start
            _toolManager.DisableTools();
            fileToolStripMenuItem.Enabled = false;

            // 2. Buat daftar semua thumbnail DENGAN NAMA YANG BENAR
            var thumbnails = new List<PictureBox>
            {
                thumbPictureBox1,
                thumbPictureBox2, // <-- Nama dari file Designer Anda
                thumbPictureBox3, // <-- Nama dari file Designer Anda
                thumbPictureBox4  // <-- Nama dari file Designer Anda
            };

            // 3. Buat DragDropManager BARU dengan DAFTAR thumbnail
            _dragDropManager = new DragDropManager(
                pictureBox1,
                thumbnails, // <-- Berikan daftarnya
                pictureBoxHistogram,
                _editorService,
                _toolManager,
                fileToolStripMenuItem
            );

            // 3. Daftarkan event
            _dragDropManager.RegisterDragDropEvents();

            // event handler untuk tombol Show Red, Green, Blue
            button5.Click += (s, e) => { if (_editorService.IsImageLoaded) pictureBox1.Image = _editorService.GetChannel(0); }; // Red
            button6.Click += (s, e) => { if (_editorService.IsImageLoaded) pictureBox1.Image = _editorService.GetChannel(1); }; // Green
            button7.Click += (s, e) => { if (_editorService.IsImageLoaded) pictureBox1.Image = _editorService.GetChannel(2); }; // Blue
            button4.Click += (s, e) => { if (_editorService.IsImageLoaded) pictureBox1.Image = _editorService.ApplyGrayscale(); }; // Grayscale

            // event handler menu histogram
            histogramRToolStripMenuItem.Click += (s, e) => TampilkanHistogramChannel(0); // Histogram R
            histogramGToolStripMenuItem.Click += (s, e) => TampilkanHistogramChannel(1); // Histogram G
            histogramBToolStripMenuItem.Click += (s, e) => TampilkanHistogramChannel(2); // Histogram B
            histogramGrToolStripMenuItem.Click += (s, e) => TampilkanHistogramChannel(3); // Histogram Grayscale

            // event handler tombol color selection
            this.button8.Click += new System.EventHandler(this.button8_Click);
            this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseClick);

            // event handler tombol negasi citra
            this.button10.Click += new System.EventHandler(this.btnNegation_Click);

            // event handler trackbar black & white
            this.trackBarBlackWhite.Scroll += new System.EventHandler(this.trackBarBlackWhite_Scroll);

            // event handler trackbar brightness
            this.trackBarBrightness.Scroll += new
            System.EventHandler(this.trackBarBrightness_Scroll);
        }

        // Fungsi helper untuk mereset UI
        private void ResetUIState()
        {
            // Form1 HANYA mengelola state-nya sendiri
            isColorSelectionMode = false;
            pictureBox1.Cursor = Cursors.Default;

            // Delegasikan reset kontrol ke UIManager
            _toolManager.ResetControls();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var loadedImage = new Bitmap(openFileDialog.FileName);
                _dragDropManager.LoadImageToThumbnail(loadedImage);
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
            if (!_editorService.IsImageLoaded) return;
            pictureBox1.Image = _editorService.GetRestoredImage();
            ResetUIState();
        }

        private void TampilkanHistogramChannel(int channel)
        {
            if (!_editorService.IsImageLoaded)
            {
                MessageBox.Show("Silakan muat gambar terlebih dahulu.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Dapatkan gambar histogram dari service
            Bitmap histBmp = _editorService.GetHistogram(channel,
                pictureBoxHistogram.Width,
                pictureBoxHistogram.Height);

            pictureBoxHistogram.Image = histBmp;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (!_editorService.IsImageLoaded)
            {
                MessageBox.Show("Silakan muat gambar terlebih dahulu.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            isColorSelectionMode = true;
            pictureBox1.Cursor = Cursors.Cross;

            MessageBox.Show("Mode Seleksi Warna Aktif. \nKlik pada warna di gambar untuk menyeleksinya.", "Mode Aktif", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (!isColorSelectionMode || !_editorService.IsImageLoaded) return;

            Point imagePoint = CoordinateHelper.TranslateMouseClickToImagePoint(pictureBox1, e.Location);
            pictureBox1.Image = _editorService.ApplyColorSelection(imagePoint);

            isColorSelectionMode = false;
            pictureBox1.Cursor = Cursors.Default;
        }

        private void btnNegation_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null) return; // Cek gambar di picturebox

            Bitmap currentImage = new Bitmap(pictureBox1.Image);
            pictureBox1.Image = _editorService.ApplyNegation(currentImage);
        }

        private void trackBarBlackWhite_Scroll(object sender, EventArgs e)
        {
            if (!_editorService.IsImageLoaded) return;

            int step = trackBarBlackWhite.Value;
            // 2. Perbarui label
            string labelText = "Threshold: ";
            switch (step)
            {
                case 0: labelText += "Darkest (50)"; break;
                case 1: labelText += "Dark (100)"; break;
                case 2: labelText += "Normal (127)"; break;
                case 3: labelText += "Light (150)"; break;
                case 4: labelText += "Lightest (200)"; break;
            }
            trackBarBlackWhite.Text = labelText;
            pictureBox1.Image = _editorService.ApplyBinarization(step);
        }

        private void trackBarBrightness_Scroll(object sender, EventArgs e)
        {
            if (!_editorService.IsImageLoaded) return;

            int adjustment = trackBarBrightness.Value;
            lblBrightnessValue.Text = "Brightness: " + adjustment.ToString();
            pictureBox1.Image = _editorService.ApplyBrightness(adjustment);
        }

        #region Operasi Aritmatika & Logika

        // --- FUNGSI HELPER UTAMA ---
        // Fungsi ini mengambil Gambar A (dari kanvas) dan Gambar B (dari thumbnail yang DIKLIK)
        private bool GetImageOperands(out Bitmap imgA, out Bitmap imgB)
        {
            imgA = null;
            imgB = null;

            // 1. Cek Gambar A (Kanvas Utama)
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Gambar A (di kanvas utama) masih kosong.\n\n" +
                                "Silakan DRAG gambar dari thumbnail ke kanvas utama dulu.",
                                "Gambar A Kosong", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // 2. Cek Gambar B (Thumbnail yang Dipilih)
            //    Kita panggil fungsi 'GetSelectedImage' dari DragDropManager
            imgB = _dragDropManager.GetSelectedImage();
            if (imgB == null)
            {
                MessageBox.Show("Gambar B belum dipilih.\n\n" +
                                "Silakan KLIK salah satu gambar di thumbnail.",
                                "Gambar B Kosong", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // 3. Jika keduanya ada, buat salinannya
            imgA = new Bitmap(pictureBox1.Image);
            // imgB sudah didapat dari manager
            return true;
        }

        // --- ISI SEMUA FUNGSI DARI LANGKAH 2 DENGAN KODE INI ---
        // (Salin-tempel kode ini ke fungsi-fungsi yang baru Anda buat)

        private void tambahToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GetImageOperands(out Bitmap imgA, out Bitmap imgB))
            {
                using (imgA) // 'using' untuk auto-dispose memori
                using (imgB)
                {
                    pictureBox1.Image = ArithmeticOperations.Add(imgA, imgB);
                }
            }
        }

        private void kurangToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GetImageOperands(out Bitmap imgA, out Bitmap imgB))
            {
                using (imgA)
                using (imgB)
                {
                    pictureBox1.Image = ArithmeticOperations.Subtract(imgA, imgB);
                }
            }
        }

        private void kaliToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GetImageOperands(out Bitmap imgA, out Bitmap imgB))
            {
                using (imgA)
                using (imgB)
                {
                    pictureBox1.Image = ArithmeticOperations.Multiply(imgA, imgB);
                }
            }
        }

        private void bagiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GetImageOperands(out Bitmap imgA, out Bitmap imgB))
            {
                using (imgA)
                using (imgB)
                {
                    pictureBox1.Image = ArithmeticOperations.Divide(imgA, imgB);
                }
            }
        }


        private void aNDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GetImageOperands(out Bitmap imgA, out Bitmap imgB))
            {
                using (imgA)
                using (imgB)
                {
                    pictureBox1.Image = LogicalOperations.And(imgA, imgB);
                }
            }
        }

        private void oRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GetImageOperands(out Bitmap imgA, out Bitmap imgB))
            {
                using (imgA)
                using (imgB)
                {
                    pictureBox1.Image = LogicalOperations.Or(imgA, imgB);
                }
            }
        }

        private void xORToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GetImageOperands(out Bitmap imgA, out Bitmap imgB))
            {
                using (imgA)
                using (imgB)
                {
                    pictureBox1.Image = LogicalOperations.Xor(imgA, imgB);
                }
            }
        }

        private void nEGATIONToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Operasi NOT hanya butuh Gambar A
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Gambar A (di kanvas utama) masih kosong.", "Error");
                return;
            }

            using (Bitmap imgA = new Bitmap(pictureBox1.Image))
            {
                pictureBox1.Image = LogicalOperations.Not(imgA);
            }
        }

        #endregion
    }
}
