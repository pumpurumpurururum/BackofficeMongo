using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BaseCms.Common.FileUploader.Interfaces;
using BaseCms.Common.Image.Interfaces;

namespace BaseCms.Controllers
{
    public class ImageUploadController : Controller
    {
        private readonly IFileManagerWithTemporaryStorage _fileManager;
        private readonly IImageCropper _imageCropper;

        public ImageUploadController(IFileManagerWithTemporaryStorage fileManager, IImageCropper cropper)
        {
            _fileManager = fileManager;
            _imageCropper = cropper;
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            string location = _fileManager.Upload(file);
            return Json(new
            {
                location,
                success = !String.IsNullOrEmpty(location)
            });
        }

        [HttpPost]
        public ActionResult UploadMultiple(HttpPostedFileBase[] files)
        {
            var locations = new string[files.Length];
            for (var i = 0; i < files.Length; i++)
            {
                locations[i] = _fileManager.Upload(files[i]);
            }

            return Json(new
            {
                locations,
                success = !locations.Any(String.IsNullOrEmpty)
            });
        }

        [HttpPost]
        public ActionResult Crop(string path, int x1, int x2, int y1, int y2, int width, int height, bool resize, string fileSuffix = "")
        {
            try
            {
                var ret = _fileManager.GetTemporaryFile(path);
                using (var stream =  new MemoryStream(ret.Bytes))
                {
                    using (var resultStream = _imageCropper.Crop(stream, x1, x2, y1, y2, width, height, resize))
                    {
                        _fileManager.SaveFile(path, resultStream);
                    }
                    return Json(new { success = true });
                }
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null) ex = ex.InnerException;
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult Remove(string path)
        {
            _fileManager.RemoveFile(path);
            return null;
        }
    }
}
