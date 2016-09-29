using System;
using System.Web.Mvc;
using BaseCms.Common.FileUploader.Interfaces;

namespace BaseCms.Controllers
{
    public class ImageController : Controller
    {
        private readonly IFileManagerWithTemporaryStorage _uploader;

        public ImageController(IFileManagerWithTemporaryStorage uploader)
        {
            _uploader = uploader;
        }

        /// <summary>
        /// Получить картинку из пула
        /// </summary>
        /// <param name="path">Путь к картинке</param>
        /// <param name="edit">Признак редактируемости картинки. Если пусто или равно "0", то картинка не редактируется</param>
        /// <param name="poolName">Имя пула</param>
        /// <param name="fileSuffix">Строка, добавляемая к имени картинки. 
        ///     Если задана, то при получении файла fileName.jpeg сначала будет попытка получить файл fileName{filesuffix}.jpeg,
        ///     если она не удастся, то будет возвращён fileName.jpeg
        /// </param>
        /// <returns></returns>
        public ActionResult Index(string path, string edit, string poolName, string fileSuffix)
        {
            if (!String.IsNullOrEmpty(path))
            {
               
                if (_uploader.Exists(path, poolName))
                {
                    

                    //var extension = path.Substring(path.LastIndexOf('.') + 1);
                    //return new FileStreamResult(_uploader.GetFile(path, poolName), "image/" + extension);
                    var file = _uploader.GetFilePrew(path, poolName);
                    //return new FileStreamResult(file.Stream, file.ContentType);
                    return new FileContentResult(file.Bytes, file.ContentType);
                }
            }

            return (!String.IsNullOrEmpty(edit) && (edit.Equals("0")))
                       ? new FilePathResult("/BaseCms/Content/images/file-not-found.png", "image/png")
                       : new FilePathResult("/BaseCms/Content/images/input-file.png", "image/png");
        }

        public ActionResult Temp(string path, string edit)
        {
            if ((!String.IsNullOrEmpty(path)) && (_uploader.ExistsTemporary(path)))
            {
                //string extension = path.Substring(path.LastIndexOf('.'));
                //return new FileStreamResult(_uploader.GetTemporaryFile(path), "image/" + extension);
                var ret = _uploader.GetTemporaryFilePrew(path);
                return new FileContentResult(ret.Bytes, ret.ContentType);
                //return new FileStreamResult(ret.Stream, ret.ContentType);

            }

            return (!String.IsNullOrEmpty(edit) && (edit.Equals("0")))
                       ? new FilePathResult("/BaseCms/Content/images/file-not-found.png", "image/png")
                       : new FilePathResult("/BaseCms/Content/images/input-file.png", "image/png");
        }
    }
}
