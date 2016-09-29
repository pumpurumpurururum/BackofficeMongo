using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseCms.CRUDRepository;
using BaseCms.CRUDRepository.DetailView;
using BaseCms.DependencyResolution;
using BackofficeDemo.Model.QuerySetter.Base;
using BackofficeDemo.Repository.Custom;
using BackofficeDemo.Repository.Generic;
using MongoDB.Bson;

namespace BackofficeDemo.Model.QuerySetter
{
    public class PartnerCategoryQuerySetter : MongoQueryInitializerBase<PartnerCategory>
    {
        protected readonly IRepository<Partner> PartnerContext = IoC.Container.GetInstance<IRepository<Partner>>();
        protected readonly ImageRepository ImageContext = IoC.Container.GetInstance<ImageRepository>();
        private void SetTrueImage(ref PartnerCategory product)
        {
            var imgId = product.ImageId;
            
            var image = Task.Run(() => ImageContext.GetFile(imgId)).Result;
            var imageContentType = image.Metadata["ContentType"].ToString();
            var oldFileName = image.Filename;
            var format = oldFileName.Split('.').Last().ToLower();
            //var format = ImageProcessing.GetImageCodecInfo(imageContentType).FilenameExtension.Split(';').First().Split('.')[1].ToLower();
            var imageName = $"{product.Alias}.{format}";
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
                var obj = (PartnerCategory)inp.Input.Object;

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
                var obj = (PartnerCategory)inp.Input.Object;


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

            Register<GetLinkedListQueryBase>((inp =>
            {
                var dataContext = GetDataContext(inp.Input.DetailViewGuid);


                if (dataContext == null)
                {
                    Guid linkId;
                    if (Guid.TryParse(inp.Input.UpperIdentifier, out linkId) && linkId != Guid.Empty)
                    {

                        var linkedroles = GetResultByFilters(new List<string>() { inp.Input.UpperIdentifier });

                        dataContext = linkedroles;
                    }
                    else
                    {
                        dataContext = new List<PartnerCategory>().AsQueryable();
                    }
                }

                var linked = dataContext.Select(p => p.Id).ToList();

                var query =
                    BaseContext.Where(p => !linked.Contains(p.Id))
                       .AsQueryable();

                if (!String.IsNullOrEmpty(inp.Input.Search))
                {
                    query =
                        query.Where(p => p.Name.ToLower().Contains(inp.Input.Search.ToLower()));
                }

                inp.Input.Output = query.Take(inp.Input.Max)
                                        .OrderBy(p => p.Name).AsEnumerable()
                                        .Select(p => new LookupItem() { Id = p.Id.ToString(), Name = p.Name })
                                        .ToList();
            }));

            Register<CreateToManyLink>(inp =>
            {

                var partnerId = Guid.Parse(inp.Input.UpperIdentifier);
                var partner = PartnerContext.GetById(partnerId);
                if (partner != null)
                {
                    var pcategoryId = Guid.Parse(inp.Input.LinkedIdentifier);
                    if (partner.PartnerCategoryGuids == null)
                        partner.PartnerCategoryGuids = new List<Guid>();
                    partner.PartnerCategoryGuids.Add(pcategoryId);
                    PartnerContext.Update(partner);
                }

            });

            Register<DeleteObject>(inp =>
            {
                var id = Guid.Parse(inp.Input.Id);
                var upperId = String.IsNullOrEmpty(inp.Input.UpperIdentifier)
                                  ? Guid.Empty
                                  : Guid.Parse(inp.Input.UpperIdentifier);




                if (upperId != Guid.Empty)
                {
                    var partner = PartnerContext.GetById(upperId);
                    partner.PartnerCategoryGuids?.Remove(id);
                    PartnerContext.Update(partner);
                }
                else
                {
                    var obj = BaseContext.GetById(id);
                    var users = PartnerContext.ToList();
                    users.ForEach(p => p.PartnerCategoryGuids?.Remove(id));
                    PartnerContext.Update(users);

                    BaseContext.Delete(obj);
                }


            });

            Register<ClearDetailViewObjects>(inp => SessionDataContainer.ClearData(inp.DetailViewGuid + CollectionName));



        }


        public override IQueryable<PartnerCategory> GetResultByFilters(List<string> filters)
        {
            var ret = BaseContext.AsQueryable();
            if (filters.Count > 0)
            {

                Guid partnerid;
                if (Guid.TryParse(filters[0], out partnerid) && partnerid != Guid.Empty)
                {
                    var user = PartnerContext.GetById(partnerid);
                    var roles = user.PartnerCategoryGuids ?? new List<Guid>();
                    ret = ret.Where(p => roles.Contains(p.Id)).AsQueryable();

                }



            }
            return ret;
        }
    }
}
