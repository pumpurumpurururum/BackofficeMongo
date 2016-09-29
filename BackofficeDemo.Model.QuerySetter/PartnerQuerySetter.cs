using System;
using System.Linq;
using System.Threading.Tasks;
using BaseCms.CRUDRepository;
using BaseCms.DependencyResolution;
using BackofficeDemo.Model.QuerySetter.Base;
using BackofficeDemo.Repository.Custom;
using MongoDB.Bson;

namespace BackofficeDemo.Model.QuerySetter
{
    public class PartnerQuerySetter : MongoQueryInitializerBase<Partner>
    {
        protected readonly ImageRepository ImageContext = IoC.Container.GetInstance<ImageRepository>();
        private void SetTrueImage(ref Partner p)
        {
            var imgId = p.ImageId;

            var image = Task.Run(() => ImageContext.GetFile(imgId)).Result;
            var imageContentType = image.Metadata["ContentType"].ToString();
            var oldFileName = image.Filename;
            var format = oldFileName.Split('.').Last().ToLower();
            //var format = ImageProcessing.GetImageCodecInfo(imageContentType).FilenameExtension.Split(';').First().Split('.')[1].ToLower();
            var imageName = $"{p.Alias}.{format}";
            var bytes = Task.Run(() => ImageContext.Read(image)).Result;
            var id = Task.Run(() => ImageContext.SaveFileAsync(bytes, imageName, imageContentType)).Result;
            p.ImageId = id;
            BaseContext.Update(p);
            Task.Run(() => ImageContext.Delete(ObjectId.Parse(imgId)));
        }
        protected override void Do()
        {
            base.Do();
            Register<GetTypeQueryBase>(t => typeof(Partner));
            

            Register<InsertObject>(inp =>
            {
                var obj = (Partner)inp.Input.Object;
                obj.RegisterDate = DateTime.Now;
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
                var obj = (Partner)inp.Input.Object;


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


            Register<LookupListQueryBase>(inp =>
            {
                var query = BaseContext;
                var str = inp.Input.Search ?? "";
                inp.Input.Output = query
                    .Where(p => string.IsNullOrEmpty(str) || p.Name.StartsWith(str))
                    .Take(inp.Input.Max)
                    .OrderBy(p => p.Name).AsEnumerable()
                    .Select(
                        p =>
                        new LookupItem
                        {
                            Id = p.Id.ToString(),
                            Name = p.Name
                        })
                    .ToList();
            });

            Register<LookupGetItemQueryBase>(inp =>
            {
                var parsedId = Guid.Parse(inp.Input.Id);
                inp.Input.Output =
                    BaseContext.Where(p => p.Id == parsedId).AsEnumerable()
                       .Select(
                           p =>
                           new LookupItem { Id = p.Id.ToString(), Name = p.Name })
                       .FirstOrDefault();
            });
        }
    }
}
