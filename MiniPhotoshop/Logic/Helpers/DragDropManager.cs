using MiniPhotoshop.Logic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq; // <-- PERBAIKAN 1: Menambahkan 'using' yang hilang
using System.Collections.Generic; // <-- Diperlukan untuk List<>

namespace MiniPhotoshop.Helpers
{
    public class DragDropManager
    {
        private readonly PictureBox _mainCanvas;
        private readonly List<PictureBox> _thumbnails;
        private readonly PictureBox _histogram;
        private readonly ToolStripMenuItem _viewMenu;

        private readonly ImageEditorService _editorService;
        private readonly UIManager _toolManager;

        // Constructor
        public DragDropManager(
            PictureBox mainCanvas, List<PictureBox> thumbnails, PictureBox histogram,
            ImageEditorService editorService, UIManager toolManager, ToolStripMenuItem viewMenu)
        {
            _mainCanvas = mainCanvas;
            _thumbnails = thumbnails;
            _histogram = histogram;
            _editorService = editorService;
            _toolManager = toolManager;
            _viewMenu = viewMenu;
        }

        // Mendaftarkan event untuk SEMUA thumbnail
        public void RegisterDragDropEvents()
        {
            _mainCanvas.DragEnter += OnCanvas_DragEnter;
            _mainCanvas.DragDrop += OnCanvas_DragDrop;
            _mainCanvas.MouseDown += OnCanvas_MouseDown;

            foreach (var thumbnail in _thumbnails)
            {
                thumbnail.MouseDown += OnThumbnail_MouseDown;
                thumbnail.DragEnter += OnThumbnail_DragEnter;
                thumbnail.DragDrop += OnThumbnail_DragDrop;
            }
        }

        // FUNGSI Logika Geser (Shift Logic)
        public void LoadImageToThumbnail(Bitmap image)
        {
            if (_thumbnails[3].Image != null)
            {
                MessageBox.Show("Slot thumbnail sudah penuh (Maksimal 4 gambar).",
                                "Gagal Menambah Gambar",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }
            for (int i = _thumbnails.Count - 2; i >= 0; i--)
            {
                _thumbnails[i + 1].Image = _thumbnails[i].Image;
            }
            _thumbnails[0].Image = image;
        }

        #region Logika Drag Drop (Sudah benar)
        private void OnThumbnail_MouseDown(object sender, MouseEventArgs e)
        {
            var clickedThumbnail = (PictureBox)sender;
            if (clickedThumbnail.Image == null || e.Button != MouseButtons.Left) return;
            var dragData = new DataObject();
            dragData.SetData(DataFormats.Bitmap, clickedThumbnail.Image);
            dragData.SetData("SourceThumbnail", clickedThumbnail);
            clickedThumbnail.DoDragDrop(dragData, DragDropEffects.Copy);
        }
        private void OnCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (!_editorService.IsImageLoaded || _mainCanvas.Cursor == Cursors.Cross || e.Button != MouseButtons.Left || _mainCanvas.Image == null) return;
            var dragData = new DataObject();
            dragData.SetData(DataFormats.Bitmap, new Bitmap(_mainCanvas.Image));
            _mainCanvas.DoDragDrop(dragData, DragDropEffects.Copy);
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
            _toolManager.ResetControls();
            _toolManager.EnableTools();
            _viewMenu.Enabled = true;
            if (e.Data.GetDataPresent("SourceThumbnail"))
            {
                var sourceThumbnail = (PictureBox)e.Data.GetData("SourceThumbnail");
                sourceThumbnail.Image = null;
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
            var droppedOnThumbnail = (PictureBox)sender;
            Bitmap droppedImage = (Bitmap)e.Data.GetData(DataFormats.Bitmap);
            if (e.Data.GetDataPresent("SourceThumbnail"))
            {
                var sourceThumbnail = (PictureBox)e.Data.GetData("SourceThumbnail");
                if (sourceThumbnail != droppedOnThumbnail)
                {
                    sourceThumbnail.Image = null;
                    droppedOnThumbnail.Image = droppedImage;
                }
            }
            else
            {
                droppedOnThumbnail.Image = droppedImage;
                _mainCanvas.Image = null;
                _histogram.Image = null;
                _editorService.ClearImage();
                _toolManager.DisableTools();
                _viewMenu.Enabled = false;
                _toolManager.ResetControls();
            }
        }
        #endregion

        #region Fungsi untuk Form Operasi

        // Fungsi ini sekarang sudah benar
        public int GetLoadedThumbnailCount()
        {
            int count = 0;
            foreach (var tb in _thumbnails)
            {
                if (tb.Image != null)
                {
                    count++;
                }
            }
            return count;
        }

        #endregion
    }
}