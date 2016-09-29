using System.IO;
using BaseCms.Model;

namespace BaseCms.Common.FileUploader.Interfaces
{
    public interface IFileManagerWithTemporaryStorage : IFileManager
    {
        FileOutput GetTemporaryFile(string path);
        FileOutput GetTemporaryFilePrew(string path);
        string SendToStorage(string path, string newPath, string poolName);
        bool ExistsTemporary(string path);
        void CopyToTemporary(string sourcePath, string destinationPath, string sourcePoolName = null);
    }
}
