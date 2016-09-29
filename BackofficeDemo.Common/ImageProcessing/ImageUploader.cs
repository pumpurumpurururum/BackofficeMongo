using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using BaseCms.Common.FileUploader.Interfaces;
using BaseCms.Model;
using BackofficeDemo.Repository.Custom;
using MongoDB.Bson;

namespace BackofficeDemo.Common.ImageProcessing
{
    public class ImageUploader : IFileManagerWithTemporaryStorage
    {
        protected ImageRepository ImageRepository = new ImageRepository();
        
   
        public string Upload(HttpPostedFileBase file)
        {
            var id = Task.Run(()=>ImageRepository.SaveFileAsync(file)).Result;

            return id;
        }

        public string GetOriginalFileName(string fileName)
        {
            

            return string.Empty;
        }

        public FileOutput GetFile(string path, string poolName)
        {

            return GetTemporaryFile(path);
        }

        public FileOutput GetFilePrew(string path, string poolName)
        {

            return GetTemporaryFile(path);
        }

        public void SaveFile(string path, Stream stream)
        {
            byte[] myByteArray = new byte[10];
            
            stream.Write(myByteArray, 0, myByteArray.Length);

            Task.Run(()=>ImageRepository.SaveFileAsync(myByteArray, path, ""));
        }

        public void RemoveFile(string path, string poolName = null)
        {
            
        }

        public bool Exists(string path, string poolName = null)
        {
            ObjectId id;
            if (!ObjectId.TryParse(path, out id))
                return false;
            var file = Task.Run(()=>ImageRepository.GetFile(path)).Result;

            return file!=null;
        }

        public void CopyTo(string sourcePath, string destinationPath, string poolName = null)
        {
            throw new NotImplementedException();
        }

        public FileOutput GetTemporaryFile(string path)
        {
            var ret = new FileOutput();
            //var ms = new MemoryStream();
            var file = Task.Run(()=>ImageRepository.GetFile(path)).Result;
            var bytes = Task.Run(()=>ImageRepository.Read(file)).Result;
            //ms.Write(bytes, 0, bytes.Length);
            //ret.Stream = ms;
            ret.ContentType = file.Metadata["ContentType"].ToString();
            ret.Bytes = bytes;
            return ret;
        }

        public FileOutput GetTemporaryFilePrew(string path)
        {
            var ret = new FileOutput();
            var emptypath = "";
            if (!string.IsNullOrEmpty(path))
            {
                var file = Task.Run(() => ImageRepository.GetFile(path)).Result;
                if (file == null)
                {
                    emptypath = HttpContext.Current.Server.MapPath("/BaseCms/Content/images/file-not-found.png");
                }
                else
                {
                    var bytes = Task.Run(() => ImageRepository.Read(file)).Result;
                    ret.ContentType = file.Metadata["ContentType"].ToString();
                    ret.Bytes = bytes;
                    return ret;
                }
            }
            
            using (var file = new FileStream(emptypath, FileMode.Open, FileAccess.Read))
            {
                var bytes = new byte[file.Length];
                file.Read(bytes, 0, (int)file.Length);
                ret.Bytes = bytes;
                ret.ContentType = "image/png";
            }
            return ret;
        }

        public string SendToStorage(string path, string newPath = null, string poolName = null)
        {
            return path;
        }

        public bool ExistsTemporary(string path)
        {
            ObjectId id;
            if (!ObjectId.TryParse(path, out id))
                return false;
            var file = Task.Run(() => ImageRepository.GetFile(path)).Result;

            return file != null;
        }

        public void CopyToTemporary(string sourcePath, string destinationPath, string sourcePoolName)
        {
            
        }
    }
}