using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using BackofficeDemo.Model;
using BackofficeDemo.Repository.Custom;
using BackofficeDemo.Repository.Generic;
using Helper;
using MongoDB.Driver;

namespace BackofficeDemo.Common.ImageProcessing.Controller
{
    public abstract class ImageApiBaseController : ApiController
    {
        protected ImageRepository ImageRepository { get; }
        protected IRepository<Product> ItemRepository { get; }
        protected IRepository<ImageSize> ImageSizeRepository { get; }
        protected IRepository<Partner> PartnerRepository { get; }
        protected ImageApiBaseController(ImageRepository imgRepository, IRepository<Product> itemRepository, IRepository<ImageSize> imgSizeRepository, IRepository<Partner> partnerRepository)
        {
            ImageRepository = imgRepository;
            ItemRepository =itemRepository;
            ImageSizeRepository = imgSizeRepository;
            PartnerRepository = partnerRepository;
        }


        private HttpResponseMessage GetEmptyImage(string imgpath= "")
        {
            if (string.IsNullOrEmpty(imgpath))
                imgpath = "~/Content/images/file-not-found.png";
            var path = HostingEnvironment.MapPath(imgpath);
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            
            result.Content = new StreamContent(stream);
            
            result.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");
            return result;

        }


        [HttpGet]
        [ActionName("productimage")]
        public HttpResponseMessage GetProductImage(string fileName)
        {

            var image = Task.Run(()=>ImageRepository.GetFileByName(fileName)).Result;
            Product product = null;
            ImageSize size = null;
            if (image == null)
            {
                var tmp = fileName.Split('/');
                var tmp2 = tmp[1].Split('.');
                var indexOfSize = tmp2[0].LastIndexOf('-');

                if (indexOfSize >= 0)
                {
                    var sizeKey = tmp2[0].Substring(indexOfSize+1);
                    var productAlias = tmp2[0].Substring(0, indexOfSize);
                    product =
                        ItemRepository.GetFirstByFilter(
                            new ExpressionFilterDefinition<Product>(p => p.Alias == productAlias));
                    size = ImageSizeRepository.GetFirstByFilter(
                            new ExpressionFilterDefinition<ImageSize>(p => p.Key == sizeKey));
                }

            }
           
            var imageId = string.Empty;
            if (!string.IsNullOrEmpty(product?.ImageId) && size!=null)
            {
                var file = Task.Run(() => ImageRepository.GetFile(product.ImageId)).Result;
                if (file != null)
                {
                    var bytes = Task.Run(() => ImageRepository.Read(file)).Result;

                    var img = Helper.ImageProcessing.ByteArrayToImage(bytes);
                    var contentType = Helper.ImageProcessing.GetContentType(bytes);
                    var imgIngo = Helper.ImageProcessing.GetImageCodecInfo(contentType);
                    var simg = Helper.ImageProcessing.Resize(img, size.Width, size.Height, ImageQuality.Good,
                        ResizeMode.BestFit, contentType);
                    var oldFileName = file.Filename;
                    var format = oldFileName.Split('.').Last().ToLower();
                    var partnerAlias = PartnerRepository.GetById(product.PartnerGuid).Alias;
                    var imageName = $"{partnerAlias}/{product.Alias}-{size.Key}.{format}";
                    if (imageName == fileName.ToLower())
                    {
                        imageId =
                            Task.Run(
                                () =>
                                    ImageRepository.SaveFileAsync(Helper.ImageProcessing.ImageToByteArray(simg),
                                        imageName,
                                        imgIngo.MimeType)).Result;

                        image = Task.Run(() => ImageRepository.GetFile(imageId)).Result;
                    }
                }
            }
            if (image!=null)
            {
                var file = Task.Run(() => ImageRepository.GetFile(image.Id.ToString())).Result;
                var bytes = Task.Run(() => ImageRepository.Read(file)).Result;

                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(bytes)
                };

                result.Content.Headers.ContentType = new MediaTypeHeaderValue(file.Metadata["ContentType"].ToString());
                return result;
            }
            return GetEmptyImage();
        }

        [HttpGet]
        [ActionName("partnerlogo")]
        public HttpResponseMessage GetPartnerLogo(string fileName)
        {

            var image = Task.Run(() => ImageRepository.GetFileByName(fileName)).Result;
            Partner partner = null;
            ImageSize size = null;
            if (image == null)
            {
                
                var tmp2 = fileName.Split('.');
                var indexOfSize = tmp2[0].LastIndexOf('-');

                if (indexOfSize >= 0)
                {
                    var sizeKey = tmp2[0].Substring(indexOfSize + 1);
                    var productAlias = tmp2[0].Substring(0, indexOfSize);
                    partner =
                        PartnerRepository.GetFirstByFilter(
                            new ExpressionFilterDefinition<Partner>(p => p.Alias == productAlias));
                    size = ImageSizeRepository.GetFirstByFilter(
                            new ExpressionFilterDefinition<ImageSize>(p => p.Key == sizeKey));
                }

            }

            var imageId = string.Empty;
            if (!string.IsNullOrEmpty(partner?.ImageId) && size != null)
            {
                var file = Task.Run(() => ImageRepository.GetFile(partner.ImageId)).Result;
                var bytes = Task.Run(() => ImageRepository.Read(file)).Result;

                var img = Helper.ImageProcessing.ByteArrayToImage(bytes);
                var contentType = Helper.ImageProcessing.GetContentType(bytes);
                var imgIngo = Helper.ImageProcessing.GetImageCodecInfo(contentType);
                var simg = Helper.ImageProcessing.Resize(img, size.Width, size.Height, ImageQuality.Good,
                                                    ResizeMode.BestFit, contentType);
                var oldFileName = file.Filename;
                var format = oldFileName.Split('.').Last().ToLower();
                
                var imageName = $"{partner.Alias}-{size.Key}.{format}";
                if (imageName == fileName.ToLower())
                {
                    imageId =
                        Task.Run(
                            () =>
                                ImageRepository.SaveFileAsync(Helper.ImageProcessing.ImageToByteArray(simg), imageName,
                                    imgIngo.MimeType)).Result;

                    image = Task.Run(() => ImageRepository.GetFile(imageId)).Result;
                }
            }
            if (image != null)
            {
                var file = Task.Run(() => ImageRepository.GetFile(image.Id.ToString())).Result;
                var bytes = Task.Run(() => ImageRepository.Read(file)).Result;

                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(bytes)
                };

                result.Content.Headers.ContentType = new MediaTypeHeaderValue(file.Metadata["ContentType"].ToString());
                return result;
            }
            return GetEmptyImage();
        }

    }
}
