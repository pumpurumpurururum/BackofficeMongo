using System.IO;
using System.Web;
using BaseCms.Model;

namespace BaseCms.Common.FileUploader.Interfaces
{
    public interface IFileManager
    {
        string Upload(HttpPostedFileBase file);
        FileOutput GetFile(string path, string poolName = null);
        FileOutput GetFilePrew(string path, string poolName = null);

        void SaveFile(string path, Stream stream);
        void RemoveFile(string path, string poolName = null);
        bool Exists(string path, string poolName = null);
        void CopyTo(string sourcePath, string destinationPath, string poolName = null);
    }
}
