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

        public bool IsImageLoaded => OriginalImage != null;

        public void InitializeImage(Bitmap image)
        {
            OriginalImage = new Bitmap(image);
            _imageData3D = ImageDataProcessor.LoadTo3DArray(OriginalImage);
        }

        public Bitmap GetRestoredImage()
        {
            if (!IsImageLoaded) return null;
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
        }
    }
}