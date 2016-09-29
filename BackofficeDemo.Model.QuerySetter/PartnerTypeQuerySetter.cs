using System;
using System.Linq;
using BaseCms.CRUDRepository;
using BackofficeDemo.Model.QuerySetter.Base;

namespace BackofficeDemo.Model.QuerySetter
{
    public class PartnerTypeQuerySetter : MongoQueryInitializerBase<PartnerType>
    {
        protected override void Do()
        {
            base.Do();
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
