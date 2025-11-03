using MiniPhotoshop.Logic;
using MiniPhotoshop.Logic.ImageProcessing;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MiniPhotoshop.Helpers
{
    public class OperationManager
    {
        private readonly ImageEditorService _editorService;

        // --- PERUBAHAN ---
        // Kita tidak perlu DragDropManager lagi, kita hanya perlu "peta"
        private readonly Dictionary<RadioButton, PictureBox> _thumbnailMap;

        // --- PERUBAHAN: Constructor Baru ---
        public OperationManager(ImageEditorService editorService, Dictionary<RadioButton, PictureBox> thumbnailMap)
        {
            _editorService = editorService;
            _thumbnailMap = thumbnailMap; // Simpan peta
        }

        // --- FUNGSI HELPER BARU ---
        // Ini adalah inti logika RadioButton Anda
        private Bitmap GetImageB_FromRadioButtons()
        {
            // Loop melalui peta yang kita simpan
            foreach (var pair in _thumbnailMap)
            {
                RadioButton rb = pair.Key;
                PictureBox pb = pair.Value;

                // Jika RadioButton ini dipilih (Checked) DAN PictureBox-nya ada gambar
                if (rb.Checked && pb.Image != null)
                {
                    return new Bitmap(pb.Image); // Kita temukan "Gambar B"
                }
            }

            // Jika tidak ada RadioButton yang dipilih (atau yang dipilih kosong)
            return null;
        }

        // --- FUNGSI HELPER YANG DIPERBARUI ---
        private bool GetImageOperands(out Bitmap imgA, out Bitmap imgB)
        {
            imgA = null;
            imgB = null;

            // 1. Cek Gambar A (via Service)
            if (!_editorService.IsImageLoaded)
            {
                MessageBox.Show("Gambar A (di kanvas utama) masih kosong.\n\n" +
                                "Silakan DRAG gambar dari thumbnail ke kanvas utama dulu.",
                                "Gambar A Kosong", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // 2. Cek Gambar B (via Radio Buttons BARU)
            imgB = GetImageB_FromRadioButtons();
            if (imgB == null)
            {
                MessageBox.Show("Gambar B belum dipilih.\n\n" +
                                "Silakan PILIH salah satu gambar yang ada di bawah. (Jangan pilih yang gambarnya tidak ada!!!)",
                                "Gambar B Kosong", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // 3. Jika lolos, ambil Gambar A dari service
            imgA = _editorService.GetRestoredImage();
            return true;
        }

        private Bitmap GetOriginalImage() => _editorService.GetRestoredImage();


        // --- FUNGSI PUBLIK (Tidak perlu diubah) ---

        public Bitmap PerformAdd()
        {
            if (GetImageOperands(out Bitmap imgA, out Bitmap imgB))
            {
                using (imgA) using (imgB)
                    return ArithmeticOperations.Add(imgA, imgB);
            }
            return GetOriginalImage();
        }

        public Bitmap PerformSubtract()
        {
            if (GetImageOperands(out Bitmap imgA, out Bitmap imgB))
            {
                using (imgA) using (imgB)
                    return ArithmeticOperations.Subtract(imgA, imgB);
            }
            return GetOriginalImage();
        }

        public Bitmap PerformMultiply()
        {
            if (GetImageOperands(out Bitmap imgA, out Bitmap imgB))
            {
                using (imgA) using (imgB)
                    return ArithmeticOperations.Multiply(imgA, imgB);
            }
            return GetOriginalImage();
        }

        public Bitmap PerformDivide()
        {
            if (GetImageOperands(out Bitmap imgA, out Bitmap imgB))
            {
                using (imgA) using (imgB)
                    return ArithmeticOperations.Divide(imgA, imgB);
            }
            return GetOriginalImage();
        }

        public Bitmap PerformAnd()
        {
            if (GetImageOperands(out Bitmap imgA, out Bitmap imgB))
            {
                using (imgA) using (imgB)
                    return LogicalOperations.And(imgA, imgB);
            }
            return GetOriginalImage();
        }

        public Bitmap PerformOr()
        {
            if (GetImageOperands(out Bitmap imgA, out Bitmap imgB))
            {
                using (imgA) using (imgB)
                    return LogicalOperations.Or(imgA, imgB);
            }
            return GetOriginalImage();
        }

        public Bitmap PerformXor()
        {
            if (GetImageOperands(out Bitmap imgA, out Bitmap imgB))
            {
                using (imgA) using (imgB)
                    return LogicalOperations.Xor(imgA, imgB);
            }
            return GetOriginalImage();
        }

        public Bitmap PerformNot()
        {
            if (!_editorService.IsImageLoaded)
            {
                MessageBox.Show("Gambar A (di kanvas utama) masih kosong.", "Error");
                return GetOriginalImage();
            }

            using (Bitmap imgA = GetOriginalImage())
            {
                return LogicalOperations.Not(imgA);
            }
        }
    }
}