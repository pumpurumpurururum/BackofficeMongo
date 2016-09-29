using System;

namespace BaseCms.Common.Image.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ImageMetadataAttribute : Attribute
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public bool FreeSize { get; set; }
        public string PoolName { get; set; }
        public string FileSuffix { get; set; }

    }
}
