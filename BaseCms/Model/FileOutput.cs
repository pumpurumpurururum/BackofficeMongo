using System.IO;

namespace BaseCms.Model
{
    public class FileOutput
    {
        public string ContentType { get; set; }
        //public Stream Stream { get; set; }

        public byte[] Bytes { get; set; }
    }
}
