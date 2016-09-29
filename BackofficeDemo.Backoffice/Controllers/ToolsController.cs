using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using BaseCms.DependencyResolution;
using BackofficeDemo.Model;
using BackofficeDemo.Repository.Custom;
using BackofficeDemo.Repository.Generic;

namespace BackofficeDemo.Backoffice.Controllers
{
    public class ToolsController : Controller
    {

        [HttpPost]
        public ActionResult RebuildIndexes()
        {
            return Json(new {Success = true}, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult RemoveImages()
        {
            ImageRepository ImageRepository = IoC.Container.GetInstance<ImageRepository>();
            IRepository<Product> ItemRepository = IoC.Container.GetInstance<IRepository<Product>>();
            IRepository<ImageSize> ImageSizeRepository = IoC.Container.GetInstance<IRepository<ImageSize>>();
            IRepository<Partner> PartnerRepository = IoC.Container.GetInstance<IRepository<Partner>>();
            var sizes = ImageSizeRepository.GetAll().ToList();
            var partners = PartnerRepository.GetAll().ToList();
            var count = 0;

            foreach (var partner in partners)
            {

                foreach (var size in sizes)
                {
                    if (partner.ImageId != null)
                    {
                        var image = Task.Run(() => ImageRepository.GetFile(partner.ImageId)).Result;
                        if (image != null)
                        {
                            var oldFileName = image.Filename;
                            var format = oldFileName.Split('.').Last().ToLower();
                            var imageName = $"{partner.Alias}-{size.Key}.{format}";
                            var curimg = Task.Run(() => ImageRepository.GetFileByName(imageName)).Result;
                            if (curimg != null)
                            {
                                Task.Run(() => ImageRepository.Delete(curimg.Id));
                                count++;
                            }
                        }
                    }
                }
                

                var items = ItemRepository.Where(p => p.PartnerGuid == partner.Id).ToList();
                foreach (var product in items)
                {
                    if (product.ImageId != null)
                    {
                        var image = Task.Run(() => ImageRepository.GetFile(product.ImageId)).Result;
                        if (image != null)
                        {
                            var oldFileName = image.Filename;
                            var format = oldFileName.Split('.').Last().ToLower();
                            foreach (var size in sizes)
                            {
                                var filename = $"{partner.Alias}/{product.Alias}-{size.Key}.{format}";
                                var curimg = Task.Run(() => ImageRepository.GetFileByName(filename)).Result;
                                if (curimg != null)
                                {
                                    Task.Run(() => ImageRepository.Delete(curimg.Id));
                                    count++;
                                }
                            }
                        }
                    }
                }
            }


            return Json(new { Success = true, count }, JsonRequestBehavior.AllowGet);
        }
    }
}