using System.Linq;
using System.Threading.Tasks;
using BaseCms.CRUDRepository;
using BaseCms.DependencyResolution;
using BackofficeDemo.Model.QuerySetter.Base;
using BackofficeDemo.Repository.Custom;
using BackofficeDemo.Repository.Generic;
using MongoDB.Bson;

namespace BackofficeDemo.Model.QuerySetter
{

    public class ProductQuerySetter : MongoQueryInitializerBase<Product>
    {
        protected readonly IRepository<Partner> PartnerContext = IoC.Container.GetInstance<IRepository<Partner>>();
        protected readonly ImageRepository ImageContext = IoC.Container.GetInstance<ImageRepository>();
        private void SetTrueImage(ref Product product)
        {
            var imgId = product.ImageId;
            var partnerAlias = PartnerContext.GetById(product.PartnerGuid).Alias;
            var image = Task.Run(() => ImageContext.GetFile(imgId)).Result;
            var imageContentType = image.Metadata["ContentType"].ToString();
            var oldFileName = image.Filename;
            var format = oldFileName.Split('.').Last().ToLower();
            //var format = ImageProcessing.GetImageCodecInfo(imageContentType).FilenameExtension.Split(';').First().Split('.')[1].ToLower();
            var imageName = $"{partnerAlias}/{product.Alias}.{format}";
            var bytes = Task.Run(() => ImageContext.Read(image)).Result;
            var id = Task.Run(() => ImageContext.SaveFileAsync(bytes, imageName, imageContentType)).Result;
            product.ImageId = id;
            BaseContext.Update(product);
            Task.Run(() => ImageContext.Delete(ObjectId.Parse(imgId)));
        }

        protected override void Do()
        {
            base.Do();
            Register<InsertObject>(inp =>
            {
                var obj = (Product)inp.Input.Object;

                var ret = BaseContext.Add(obj);

                ValidateAlias(ret);
                if (!string.IsNullOrEmpty(obj.ImageId))
                {
                    SetTrueImage(ref obj);

                }
                inp.Input.Output = ret.Id.ToString();
            });

            Register<UpdateObject>(inp =>
            {
                var obj = (Product)inp.Input.Object;


                BaseContext.Update(obj);

                ValidateAlias(obj);
                if (!string.IsNullOrEmpty(obj.ImageId))
                {
                    var obj1 = obj;
                    var img = Task.Run(() => ImageContext.GetFile(obj1.ImageId)).Result;
                    if (!img.Filename.Contains(obj.Alias))
                    {
                        SetTrueImage(ref obj);
                    }
                }

            });
        }
    }
}
