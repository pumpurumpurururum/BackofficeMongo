using System.IO;

namespace BaseCms.Common.Image.Interfaces
{
    public interface IImageCropper
    {
        Stream Crop(Stream imageStream, int x1, int x2, int y1, int y2, int width, int height, bool resize);
    }
}
