using System.Drawing;
using System.Windows.Forms;
using MiniPhotoshop.Logic.Helpers;
using MiniPhotoshop.Logic.ImageProcessing;
using MiniPhotoshop.Logic.Histogram;

namespace MiniPhotoshop.Logic
{
    public class ImageEditorService
    {
        public Bitmap OriginalImage { get; private set; }
        private int[,,] _imageData3D;
        private Bitmap _backupImage;
        public bool IsImageLoaded => OriginalImage != null;

        public void InitializeImage(Bitmap image)
        {
            // 1. Simpan sebagai Backup (Arsip)
            _backupImage = new Bitmap(image);

            // 2. Simpan sebagai Gambar Kerja (Working Image)
            OriginalImage = new Bitmap(image);
            _imageData3D = ImageDataProcessor.LoadTo3DArray(OriginalImage);
        }

        public Bitmap GetRestoredImage()
        {
            if (_backupImage == null) return null;

            // 1. Reset OriginalImage kembali ke Backup
            OriginalImage = new Bitmap(_backupImage);

            // 2. Reset juga data array 3D-nya
            _imageData3D = ImageDataProcessor.LoadTo3DArray(OriginalImage);

            // 3. Kembalikan clone-nya ke layar
            return (Bitmap)OriginalImage.Clone();
        }

        public Bitmap GetChannel(int channel)
        {
            if (!IsImageLoaded) return null;
            return ImageDataProcessor.CreateBitmapFrom3DArray(_imageData3D, channel);
        }

        public Bitmap ApplyGrayscale()
        {
            return GetChannel(3);
        }

        public Bitmap ApplyNegation(Bitmap currentImage)
        {
            if (!IsImageLoaded) return null;
            return ImageNegation.Apply(currentImage);
        }

        public Bitmap ApplyBinarization(int step)
        {
            if (!IsImageLoaded) return null;
            return BlackWhite.ApplyBinarization(OriginalImage, step);
        }

        public Bitmap ApplyBrightness(int adjustment)
        {
            if (!IsImageLoaded) return null;
            return Brightness.AdjustBrightness(OriginalImage, adjustment);
        }

        public Bitmap ApplyColorSelection(Point imagePoint)
        {
            if (!IsImageLoaded) return null;
            Color clickedColor = OriginalImage.GetPixel(imagePoint.X, imagePoint.Y);
            return SelectionColor.ApplySelection(OriginalImage, clickedColor);
        }

        public Bitmap GetHistogram(int channel, int width, int height)
        {
            if (!IsImageLoaded) return null;
            int[] hist = HistogramCalculator.Calculate(OriginalImage, channel);
            return HistogramDrawer.Draw(hist, channel, width, height);
        }

        public void ClearImage()
        {
            OriginalImage = null;
            _imageData3D = null;
            _backupImage = null; // Hapus backup
        }

        // ---------------------------------------------------------
        // TAMBAHAN BARU: Untuk mengupdate gambar hasil editan (Equalization, dll)
        // ---------------------------------------------------------
        public void UpdateCurrentImage(Bitmap newImage)
        {
            if (newImage != null)
            {
                // 1. Update Gambar Utama
                OriginalImage = new Bitmap(newImage);

                // 2. PENTING: Update juga data pixel mentahnya (_imageData3D)
                // Kalau ini tidak di-update, nanti saat Anda klik fitur lain,
                // dia akan kembali ke gambar lama!
                _imageData3D = ImageDataProcessor.LoadTo3DArray(OriginalImage);
            }
        }
    }
}