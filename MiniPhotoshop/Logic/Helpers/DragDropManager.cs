using MiniPhotoshop.Logic;
using System.Drawing;
using System.Windows.Forms;

namespace MiniPhotoshop.Helpers
{
    public class DragDropManager
    {
        // Kontrol UI yang dikelola
        private readonly PictureBox _mainCanvas;
        private readonly PictureBox _thumbnail;
        private readonly PictureBox _histogram;
        private readonly ToolStripMenuItem _viewMenu;

        // Service
        private readonly ImageEditorService _editorService;
        private readonly UIManager _toolManager;

        // State (dipindahkan dari Form1)
        private Bitmap _thumbnailImage;

        public DragDropManager(
            PictureBox mainCanvas, PictureBox thumbnail, PictureBox histogram,
            ImageEditorService editorService, UIManager toolManager, ToolStripMenuItem viewMenu)
        {
            _mainCanvas = mainCanvas;
            _thumbnail = thumbnail;
            _histogram = histogram;
            _editorService = editorService;
            _toolManager = toolManager;
            _viewMenu = viewMenu;
        }

        // 1. Metode untuk mendaftarkan semua event
        public void RegisterDragDropEvents()
        {
            // Event dari Thumbnail ke Kanvas Utama
            _thumbnail.MouseDown += OnThumbnail_MouseDown;
            _mainCanvas.DragEnter += OnCanvas_DragEnter;
            _mainCanvas.DragDrop += OnCanvas_DragDrop;

            // Event dari Kanvas Utama ke Thumbnail
            _mainCanvas.MouseDown += OnCanvas_MouseDown;
            _thumbnail.DragEnter += OnThumbnail_DragEnter;
            _thumbnail.DragDrop += OnThumbnail_DragDrop;
        }

        // 2. Metode untuk memuat gambar ke thumbnail (dipanggil oleh button1)
        public void LoadImageToThumbnail(Bitmap image)
        {
            _thumbnailImage = image;
            _thumbnail.Image = image;
        }

        // --- Logika Event (Dipindahkan dari Form1.cs) ---

        // KASUS 1: Thumbnail -> Kanvas Utama
        private void OnThumbnail_MouseDown(object sender, MouseEventArgs e)
        {
            if (_thumbnailImage != null)
            {
                _thumbnail.DoDragDrop(_thumbnailImage, DragDropEffects.Copy);
            }
        }

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

            _toolManager.ResetControls(); // <-- Panggil UIManager
            _toolManager.EnableTools();
            _viewMenu.Enabled = true;

            _thumbnailImage = null;
            _thumbnail.Image = null;
        }

        // KASUS 2: Kanvas Utama -> Thumbnail
        private void OnCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            // Cek kursor, BUKAN boolean. Ini lebih bersih.
            if (_editorService.IsImageLoaded && _mainCanvas.Cursor != Cursors.Cross && e.Button == MouseButtons.Left && _mainCanvas.Image != null)
            {
                Bitmap imageToDrag = new Bitmap(_mainCanvas.Image);
                _mainCanvas.DoDragDrop(imageToDrag, DragDropEffects.Copy);
            }
        }

        private void OnThumbnail_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Bitmap))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void OnThumbnail_DragDrop(object sender, DragEventArgs e)
        {
            Bitmap droppedImage = (Bitmap)e.Data.GetData(DataFormats.Bitmap);
            _thumbnailImage = droppedImage;
            _thumbnail.Image = droppedImage;

            _mainCanvas.Image = null;
            _histogram.Image = null;
            _editorService.ClearImage();
            _toolManager.DisableTools();
            _viewMenu.Enabled = false;
            _toolManager.ResetControls(); // <-- Panggil UIManager
        }
    }
}