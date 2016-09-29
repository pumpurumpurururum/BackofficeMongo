using System;

namespace BaseCms.Views.Detail
{
    public class ImageMetadata
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public bool FreeSize { get; set; }
        public string Ratio
        {
            get
            {
                return Width != 0 && Height != 0 && !FreeSize
                           ? String.Format("{0}:{1}", Width, Height)
                           : String.Empty;
            }
        }

        public string PoolName { get; set; }

        public string FileSuffix { get; set; }
    }
}
