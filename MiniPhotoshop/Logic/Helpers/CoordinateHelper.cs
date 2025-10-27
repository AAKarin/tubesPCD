using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MiniPhotoshop.Logic.Helpers
{
    public static class CoordinateHelper
    {
        public static Point TranslateMouseClickToImagePoint(PictureBox pb, Point mouseLocation)
        {
            if (pb.Image == null)
            {
                return mouseLocation;
            }

            float pWidth = pb.ClientSize.Width;
            float pHeight = pb.ClientSize.Height;

            float iWidth = pb.Image.Width;
            float iHeight = pb.Image.Height;

            float pRatio = pWidth / pHeight;
            float iRatio = iWidth / iHeight;

            float newWidth = iWidth;
            float newHeight = iHeight;
            float offsetX = 0;
            float offsetY = 0;

            if (pRatio > iRatio) 
            {
                newWidth = pHeight * iRatio;
                newHeight = pHeight;
                offsetX = (pWidth - newWidth) / 2;
            }
            else 
            {
                newWidth = pWidth;
                newHeight = pWidth / iRatio;
                offsetY = (pHeight - newHeight) / 2;
            }

            int imgX = (int)((mouseLocation.X - offsetX) * (iWidth / newWidth));
            int imgY = (int)((mouseLocation.Y - offsetY) * (iHeight / newHeight));

            if (imgX < 0) imgX = 0;
            if (imgY < 0) imgY = 0;
            if (imgX >= iWidth) imgX = (int)iWidth - 1;
            if (imgY >= iHeight) imgY = (int)iHeight - 1;

            return new Point(imgX, imgY);
        }
    }
}