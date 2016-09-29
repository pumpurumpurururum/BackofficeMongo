using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Helper
{
    /// <summary>
    /// Качество изображения
    /// </summary>
    public enum ImageQuality
    {
        /// <summary>
        /// Плохое
        /// </summary>
        Poor = 0,
        /// <summary>
        /// Нормальное
        /// </summary>
        Normal = 1,
        /// <summary>
        /// Хорошее
        /// </summary>
        Good = 2,
        /// <summary>
        /// Наилучшее
        /// </summary>
        Excellent = 3
    };
    /// <summary>
    /// Способ обрезки
    /// </summary>
    public enum ResizeMode
    {
        /// <summary>
        /// Вылезающие края обрезаются
        /// </summary>
        Photo = 0,
        /// <summary>
        /// Изображение вписывается с сохранением пропорций
        /// </summary>
        BestFit = 1,
        /// <summary>
        /// Изображение растягивается по вертикали и по горизотали
        /// </summary>
        Stretch = 2
    };

    /// <summary>
    /// Ориентация изображения
    /// </summary>
    public enum ImageAlignType
    {
        /// <summary>
        /// по центру
        /// </summary>
        Center = 0,
        /// <summary>
        /// левый верхний угол
        /// </summary>
        TopLeft = 1
    }

    public static class ImageProcessing
    {
        /*если для previw задаем только ширину или высоту, то */
        /// <summary>
        /// Изменение размеров изображения
        /// </summary>
        /// <param name="source">Изображение</param>
        /// <param name="newWidth">новая ширина</param>
        /// <param name="newHeight">новая высота</param>
        /// <param name="quality">качество</param>
        /// <param name="mode">способ сжатия</param>
        /// <param name="format">формат итогового изображения</param>
        /// <param name="constrainProportions">сохранение пропорций</param>
        /// <returns>Изображение</returns>
        public static Image Resize(Image source, int newWidth, int newHeight, ImageQuality quality, ResizeMode mode, ImageFormat format, bool constrainProportions)
        {
            if (newWidth > 0 && newHeight == 0 && constrainProportions)
            {
                var res = Convert.ToDouble(source.Height) / Convert.ToDouble(source.Width);
                res = res * newWidth;
                newHeight = Convert.ToInt32(res);
            }

            if (newHeight > 0 && newWidth == 0 && constrainProportions)
            {
                var res = Convert.ToDouble(source.Width) / Convert.ToDouble(source.Height);
                res = res * newHeight;
                newWidth = Convert.ToInt32(res);
            }
            return Resize(source, newWidth, newHeight, quality, mode, format);
        }

        /// <summary>
        /// Изменение размеров изображения
        /// </summary>
        /// <param name="source">Изображение</param>
        /// <param name="newWidth">новая ширина</param>
        /// <param name="newHeight">новая высота</param>
        /// <param name="quality">качество</param>
        /// <param name="mode">способ сжатия</param>
        /// <param name="format">формат итогового изображения</param>
        /// <returns>Изображение</returns>
        public static Image Resize(Image source, int newWidth, int newHeight, ImageQuality quality, ResizeMode mode, ImageFormat format)
        {
            return Resize(source, newWidth, newHeight, quality, mode, format, ImageAlignType.TopLeft);
        }

        /// <summary>
        /// Изменение размеров изображения
        /// </summary>
        /// <param name="source">Изображение</param>
        /// <param name="newWidth">новая ширина</param>
        /// <param name="newHeight">новая высота</param>
        /// <param name="quality">качество</param>
        /// <param name="mode">способ сжатия</param>
        /// <param name="format">формат итогового изображения</param>
        /// <param name="imageAlignType">ориентация изображения</param>
        /// <returns>Изображение</returns>
        public static Image Resize(Image source, int newWidth, int newHeight, ImageQuality quality, ResizeMode mode, ImageFormat format, ImageAlignType imageAlignType)
        {
            return Resize(source, newWidth, newHeight, quality, mode, format, source.PixelFormat, imageAlignType);
        }

        /// <summary>
        /// Изменение размеров изображения
        /// </summary>
        /// <param name="source">Изображение</param>
        /// <param name="newWidth">новая ширина</param>
        /// <param name="newHeight">новая высота</param>
        /// <param name="quality">качество</param>
        /// <param name="mode">способ сжатия</param>
        /// <param name="format">формат итогового изображения</param>
        /// <param name="imageAlignType">ориентация изображения</param>
        /// <param name="pixelFormat">Тип цветопередачи</param>
        /// <exception cref="NotImplementedException"></exception>
        /// <returns>Изображение</returns>
        public static Image Resize(Image source, int newWidth, int newHeight, ImageQuality quality, ResizeMode mode,
                                   ImageFormat format, PixelFormat pixelFormat, ImageAlignType imageAlignType)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            //return null;

            var newSize =
                new Size(newWidth > 0 ? newWidth : source.Width, newHeight > 0 ? newHeight : source.Height);

            if (quality == ImageQuality.Excellent && Equals(source.RawFormat, format)
                && newSize.Width == source.Width && newSize.Height == source.Height)
            {
                return source;
            }

            //TODO: make gifs transparent and fix their palletes
            using (var newBitmap = pixelFormat == PixelFormat.DontCare ? new Bitmap(newSize.Width, newSize.Height) : new Bitmap(newSize.Width, newSize.Height, pixelFormat))
            {
                using (var newGraphics = Graphics.FromImage(newBitmap))
                {
                    if (Equals(format, ImageFormat.Png) || Equals(format, ImageFormat.Gif))
                        newGraphics.Clear(Color.Transparent);
                    else
                        newGraphics.Clear(Color.White);

                    long codecQuality;

                    switch (quality)
                    {
                        case ImageQuality.Poor:
                            newGraphics.CompositingQuality = CompositingQuality.HighSpeed;
                            newGraphics.InterpolationMode = InterpolationMode.Low;
                            newGraphics.SmoothingMode = SmoothingMode.HighSpeed;
                            newGraphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                            codecQuality = 50;
                            break;

                        case ImageQuality.Normal:
                            newGraphics.CompositingQuality = CompositingQuality.HighQuality;
                            newGraphics.InterpolationMode = InterpolationMode.Bicubic;
                            newGraphics.SmoothingMode = SmoothingMode.HighSpeed;
                            newGraphics.PixelOffsetMode = PixelOffsetMode.Half;
                            codecQuality = 70;
                            break;

                        case ImageQuality.Good:
                            newGraphics.CompositingQuality = CompositingQuality.HighQuality;
                            newGraphics.InterpolationMode = InterpolationMode.High;
                            newGraphics.SmoothingMode = SmoothingMode.HighQuality;
                            newGraphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            codecQuality = 90;
                            break;

                        case ImageQuality.Excellent:
                            newGraphics.CompositingQuality = CompositingQuality.HighQuality;
                            newGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            newGraphics.SmoothingMode = SmoothingMode.HighQuality;
                            newGraphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            codecQuality = 100;
                            break;

                        default:
                            //Setting Low Quality Transformation
                            newGraphics.CompositingQuality = CompositingQuality.HighSpeed;
                            newGraphics.InterpolationMode = InterpolationMode.Low;
                            newGraphics.SmoothingMode = SmoothingMode.HighSpeed;
                            newGraphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                            codecQuality = 30;
                            break;
                    }

                    float ratioWidth = (float)newSize.Width / source.Width;
                    float ratioHeight = (float)newSize.Height / source.Height;
                    float ratio;
                    int width;
                    int height;

                    switch (mode)
                    {
                        case ResizeMode.BestFit:
                            ratio = Math.Min(ratioWidth, ratioHeight);
                            width = (int)(ratio * source.Width);
                            height = (int)(ratio * source.Height);
                            break;

                        case ResizeMode.Photo:
                            ratio = Math.Max(ratioWidth, ratioHeight);
                            width = (int)(ratio * source.Width);
                            height = (int)(ratio * source.Height);
                            break;

                        default:
                            width = newSize.Width;
                            height = newSize.Height;
                            break;
                    }

                    switch (imageAlignType)
                    {
                        case ImageAlignType.Center:
                            newGraphics.DrawImage(source, (newSize.Width - width) / 2,
                                                  (newSize.Height - height) / 2, width,
                                                  height);
                            break;
                        case ImageAlignType.TopLeft:
                            newGraphics.DrawImage(source, 0, 0, width, height);
                            break;
                        default:
                            throw new NotImplementedException("Выбранное выравнивание изображения не реализовано в коде");
                    }

                    newGraphics.Flush();

                    var parameters = new EncoderParameters(1)
                    {
                        Param = {[0] = new EncoderParameter(Encoder.Quality, codecQuality)}
                    };
                    using (var memStream = new MemoryStream())
                    {
                        newBitmap.Save(memStream, GetImageCodecInfo(format), parameters);

                        //return new Bitmap(memStream);// Image.FromStream(memStream);

                        using (var original = new Bitmap(memStream))
                        {
                            var returnBmp = new Bitmap(original.Width, original.Height);
                            using (var g = Graphics.FromImage(returnBmp))
                            {
                                g.CompositingQuality = newGraphics.CompositingQuality;
                                g.InterpolationMode = newGraphics.InterpolationMode;
                                g.SmoothingMode = newGraphics.SmoothingMode;
                                g.PixelOffsetMode = newGraphics.PixelOffsetMode;

                                g.DrawImage(original, 0, 0, original.Width, original.Height);
                            }

                            return returnBmp;
                        }
                    }
                }
            }
        }
        ///// <summary>
        ///// Приведение формата изображения из NewtonCMS в формат Drawing
        ///// </summary>
        ///// <param name="format">Формат изображения</param>
        ///// <returns>информация о кодеке</returns>
        //public static ImageCodecInfo GetImageCodecInfo(ImageFormat format) { return GetImageCodecInfo(format); }

        /// <summary>
        /// Приведение формата изображения из NewtonCMS в формат Drawing
        /// </summary>
        /// <param name="mimeType">Тип MIME изображения</param>
        /// <returns>информация о кодеке</returns>
        public static ImageCodecInfo GetImageCodecInfo(string mimeType)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            return codecs.FirstOrDefault(t => t.MimeType == mimeType);
        }


        /// <summary>
        /// Приведение формата изображения из NewtonCMS в формат Drawing
        /// </summary>
        /// <param name="imgFormat">Формат изображения</param>
        /// <returns>информация о кодеке</returns>
        public static ImageCodecInfo GetImageCodecInfo(ImageFormat imgFormat)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            return codecs.FirstOrDefault(t => t.FormatID == imgFormat.Guid);
        }

        public static ImageFormat GetContentType(byte[] imageBytes)
        {
            MemoryStream ms = new MemoryStream(imageBytes);

            using (BinaryReader br = new BinaryReader(ms))
            {
                int maxMagicBytesLength = ImageFormatDecoders.Keys.OrderByDescending(x => x.Length).First().Length;

                byte[] magicBytes = new byte[maxMagicBytesLength];

                for (int i = 0; i < maxMagicBytesLength; i += 1)
                {
                    magicBytes[i] = br.ReadByte();

                    foreach (var kvPair in ImageFormatDecoders)
                    {
                        if (magicBytes.StartsWith(kvPair.Key))
                        {
                            return kvPair.Value;
                        }
                    }
                }

                throw new ArgumentException("Could not recognise image format", nameof(imageBytes));
            }
        }

        private static bool StartsWith(this byte[] thisBytes, byte[] thatBytes)
        {
            for (int i = 0; i < thatBytes.Length; i += 1)
            {
                if (thisBytes[i] != thatBytes[i])
                {
                    return false;
                }
            }
            return true;
        }
        public static string GetFilenameExtension(ImageFormat format)
        {
            var firstOrDefault = ImageCodecInfo.GetImageEncoders().FirstOrDefault(x => x.FormatID == format.Guid);
            if (firstOrDefault != null)
                return firstOrDefault.FilenameExtension;
            return string.Empty;
        }

        private static readonly Dictionary<byte[], ImageFormat> ImageFormatDecoders = new Dictionary<byte[], ImageFormat>
        {
            { new byte[]{ 0x42, 0x4D }, ImageFormat.Bmp},
            { new byte[]{ 0x47, 0x49, 0x46, 0x38, 0x37, 0x61 }, ImageFormat.Gif },
            { new byte[]{ 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 }, ImageFormat.Gif },
            { new byte[]{ 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }, ImageFormat.Png },
            { new byte[]{ 0xff, 0xd8 }, ImageFormat.Jpeg },
        };

        public static ImageFormat GetImageFormat(this Image img)
        {
            if (img.RawFormat.Equals(ImageFormat.Jpeg))
                return ImageFormat.Jpeg;
            if (img.RawFormat.Equals(ImageFormat.Bmp))
                return ImageFormat.Bmp;
            if (img.RawFormat.Equals(ImageFormat.Png))
                return ImageFormat.Png;
            if (img.RawFormat.Equals(ImageFormat.Emf))
                return ImageFormat.Emf;
            if (img.RawFormat.Equals(ImageFormat.Exif))
                return ImageFormat.Exif;
            if (img.RawFormat.Equals(ImageFormat.Gif))
                return ImageFormat.Gif;
            if (img.RawFormat.Equals(ImageFormat.Icon))
                return ImageFormat.Icon;
            if (img.RawFormat.Equals(ImageFormat.MemoryBmp))
                return ImageFormat.MemoryBmp;
            if (img.RawFormat.Equals(ImageFormat.Tiff))
                return ImageFormat.Tiff;
            return ImageFormat.Wmf;
        }
        public static Image ByteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
        public static byte[] ImageToByteArray(Image imageIn)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(imageIn, typeof(byte[]));
        }
    }
}
