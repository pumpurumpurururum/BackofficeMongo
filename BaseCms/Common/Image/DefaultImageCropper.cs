using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using BaseCms.Common.Image.Interfaces;

namespace BaseCms.Common.Image
{
    public class DefaultImageCropper : IImageCropper
    {
        public Stream Crop(Stream imageStream, int x1, int x2, int y1, int y2, int width, int height, bool resize)
        {
            var resultStream = new MemoryStream();

            var image = System.Drawing.Image.FromStream(imageStream);

            // Crop...
            var bmpImage = new Bitmap(image);

            // ширина нового изображения
            var newWidth = Math.Min(x2 - x1, image.Width);
            if (x1 + newWidth > image.Width) newWidth = image.Width - x1;
            // высота нового изображения
            var newHeight = Math.Min(y2 - y1, image.Height);
            if (y1 + newHeight > image.Height) newHeight = image.Height - y1;

            var bmpResult =
                bmpImage.Clone(
                    new Rectangle(x1, y1, newWidth, newHeight),
                    bmpImage.PixelFormat);

            // Resize...
            if ((resize) && ((width > 0) || (height > 0)))
            {
                if (height == 0) height = width * newHeight / newWidth;
                if (width == 0) width = height * newWidth / newHeight;

                var bmpResized = new Bitmap(width, height);
                Graphics g = Graphics.FromImage(bmpResized);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmpResult, 0, 0, width, height);
                g.Dispose();
                bmpResult = bmpResized;
            }

            bmpResult.Save(resultStream, ImageFormat.Jpeg);

            return resultStream;
        }
    }
}
