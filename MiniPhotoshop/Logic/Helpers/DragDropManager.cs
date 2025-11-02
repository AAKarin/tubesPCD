using MiniPhotoshop.Logic;
using System.Drawing;
using System.Windows.Forms;

namespace MiniPhotoshop.Helpers
{
    public class DragDropManager
    {
        private readonly PictureBox _mainCanvas;
        private readonly List<PictureBox> _thumbnails; // Mengelola DAFTAR thumbnail
        private readonly PictureBox _histogram;
        private readonly ToolStripMenuItem _viewMenu;

        private readonly ImageEditorService _editorService;
        private readonly UIManager _toolManager;

        // Constructor BARU: Menerima List<PictureBox>
        public DragDropManager(
            PictureBox mainCanvas, List<PictureBox> thumbnails, PictureBox histogram,
            ImageEditorService editorService, UIManager toolManager, ToolStripMenuItem viewMenu)
        {
            _mainCanvas = mainCanvas;
            _thumbnails = thumbnails; // Disimpan sebagai daftar
            _histogram = histogram;
            _editorService = editorService;
            _toolManager = toolManager;
            _viewMenu = viewMenu;
        }

        // Mendaftarkan event untuk SEMUA thumbnail
        public void RegisterDragDropEvents()
        {
            // Daftarkan event untuk KANVAS UTAMA
            _mainCanvas.DragEnter += OnCanvas_DragEnter;
            _mainCanvas.DragDrop += OnCanvas_DragDrop;
            _mainCanvas.MouseDown += OnCanvas_MouseDown;

            // Loop untuk mendaftarkan semua thumbnail
            foreach (var thumbnail in _thumbnails)
            {
                thumbnail.MouseDown += OnThumbnail_MouseDown;
                thumbnail.DragEnter += OnThumbnail_DragEnter;
                thumbnail.DragDrop += OnThumbnail_DragDrop;
            }
        }

        // FUNGSI BARU (Logika Geser / Shift Logic)
        public void LoadImageToThumbnail(Bitmap image)
        {
            // Asumsi dari Form1.cs:
            // _thumbnails[0] = thumbPictureBox1
            // _thumbnails[1] = thumbPictureBox2 (atau 3)
            // _thumbnails[2] = thumbPictureBox3 (atau 4)
            // _thumbnails[3] = thumbPictureBox4 (atau 5)
            // (Nama dari Form1.Designer.cs Anda adalah thumbPictureBox1, 2, 3, 4)

            // 1. Cek apakah slot TERAKHIR (paling kanan) sudah terisi.
            if (_thumbnails[3].Image != null)
            {
                MessageBox.Show("Slot thumbnail sudah penuh (Maksimal 4 gambar). " +
                                "Silakan hapus gambar atau gunakan gambar yang sudah ada.",
                                "Gagal Menambah Gambar",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            // 2. Geser semua gambar yang ada ke kanan (mulai dari belakang)
            for (int i = _thumbnails.Count - 2; i >= 0; i--)
            {
                // i=2: _thumbnails[3].Image = _thumbnails[2].Image
                // i=1: _thumbnails[2].Image = _thumbnails[1].Image
                // i=0: _thumbnails[1].Image = _thumbnails[0].Image
                _thumbnails[i + 1].Image = _thumbnails[i].Image;
            }

            // 3. Taruh gambar baru di slot PERTAMA (paling kiri)
            _thumbnails[0].Image = image;
        }

        // --- KASUS 1: Drag dimulai dari THUMBNAIL ---
        private void OnThumbnail_MouseDown(object sender, MouseEventArgs e)
        {
            var clickedThumbnail = (PictureBox)sender; // Tahu thumbnail mana yang diklik
            if (clickedThumbnail.Image == null || e.Button != MouseButtons.Left) return;

            var dragData = new DataObject();
            dragData.SetData(DataFormats.Bitmap, clickedThumbnail.Image);
            dragData.SetData("SourceThumbnail", clickedThumbnail); // Penting: tandai sumbernya

            clickedThumbnail.DoDragDrop(dragData, DragDropEffects.Copy);
        }

        // --- KASUS 2: Drag dimulai dari KANVAS UTAMA ---
        private void OnCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (!_editorService.IsImageLoaded || _mainCanvas.Cursor == Cursors.Cross || e.Button != MouseButtons.Left || _mainCanvas.Image == null) return;

            var dragData = new DataObject();
            dragData.SetData(DataFormats.Bitmap, new Bitmap(_mainCanvas.Image));
            // Tidak menandai "SourceThumbnail", jadi kita tahu ini dari kanvas

            _mainCanvas.DoDragDrop(dragData, DragDropEffects.Copy);
        }


        // --- MENANGANI DROP DI KANVAS UTAMA ---
        private void OnCanvas_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Bitmap))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void OnCanvas_DragDrop(object sender, DragEventArgs e)
        {
            Bitmap droppedImage = (Bitmap)e.Data.GetData(DataFormats.Bitmap);
            _editorService.InitializeImage(droppedImage);
            _mainCanvas.Image = _editorService.GetRestoredImage();

            _toolManager.ResetControls();
            _toolManager.EnableTools();
            _viewMenu.Enabled = true;

            // Jika sumbernya dari thumbnail, kosongkan thumbnail tsb
            if (e.Data.GetDataPresent("SourceThumbnail"))
            {
                var sourceThumbnail = (PictureBox)e.Data.GetData("SourceThumbnail");
                sourceThumbnail.Image = null;
            }
        }

        // --- MENANGANI DROP DI THUMBNAIL (salah satu dari 4) ---
        private void OnThumbnail_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Bitmap))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void OnThumbnail_DragDrop(object sender, DragEventArgs e)
        {
            var droppedOnThumbnail = (PictureBox)sender; // Tahu di-drop di thumbnail mana
            Bitmap droppedImage = (Bitmap)e.Data.GetData(DataFormats.Bitmap);

            // KASUS A: Drag dari Thumbnail LAIN
            if (e.Data.GetDataPresent("SourceThumbnail"))
            {
                var sourceThumbnail = (PictureBox)e.Data.GetData("SourceThumbnail");
                if (sourceThumbnail != droppedOnThumbnail) // Pastikan bukan di-drop ke diri sendiri
                {
                    // Ini adalah "pindah" (move) antar thumbnail
                    sourceThumbnail.Image = null; // Kosongkan sumber
                    droppedOnThumbnail.Image = droppedImage; // Isi target
                }
            }
            // KASUS B: Drag dari KANVAS UTAMA
            else
            {
                // Taruh gambar di thumbnail
                droppedOnThumbnail.Image = droppedImage;

                // Kosongkan kanvas utama dan matikan semua alat
                _mainCanvas.Image = null;
                _histogram.Image = null;
                _editorService.ClearImage();
                _toolManager.DisableTools();
                _viewMenu.Enabled = false;
                _toolManager.ResetControls();
            }
        }
    }
}