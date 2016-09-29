using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace BackofficeDemo.Repository.Custom
{
    public class ImageRepository 
    {

        private readonly GridFSBucket _gridFs;

        public ImageRepository() 
        {
            _gridFs = new GridFSBucket(Util<Guid>.GetDatabase());
        }

        public async Task<string> SaveFileAsync(byte[] data, string fileName, string contentType)
        {
            var createOptions = new GridFSUploadOptions
            {
                ChunkSizeBytes = _gridFs.Options.ChunkSizeBytes,
                Metadata = new BsonDocument
                {
                    //{ "FileName", fileName },
                    { "UploadDate", DateTime.Now },
                    { "ContentType", contentType }
                },
            };
            using (var stream = await _gridFs.OpenUploadStreamAsync(fileName, createOptions))
            {
                var fid = stream.Id;
                await stream.WriteAsync(data, 0, data.Length);
                await stream.CloseAsync();

                return fid.ToString();
            }
        }

        public async Task<string> SaveFileAsync(HttpPostedFileBase file)
        {
            var data = new byte[file.ContentLength];
            await file.InputStream.ReadAsync(data, 0, file.ContentLength);

            return await SaveFileAsync(data, file.FileName, file.ContentType);
        }

        public async Task<GridFSFileInfo> GetFile(string id)
        {
            var filter = Builders<GridFSFileInfo>.Filter.Eq("_id", ObjectId.Parse(id));
            var found = await (await _gridFs.FindAsync(filter)).ToListAsync();

            return !found.Any() ? null : found.FirstOrDefault();
        }


        public async Task<GridFSFileInfo> GetFileByName(string name)
        {
            var filter = Builders<GridFSFileInfo>.Filter.Eq("Filename", name);
            var found = await (await _gridFs.FindAsync(filter)).ToListAsync();

            return !found.Any() ? null : found.FirstOrDefault();
        }

        public async Task<byte[]> Read(GridFSFileInfo fileInfo)
        {
            return await _gridFs.DownloadAsBytesAsync(fileInfo.Id);
        }

        public async void Delete(ObjectId id)
        {
            await _gridFs.DeleteAsync(id);
        }
    }
}
