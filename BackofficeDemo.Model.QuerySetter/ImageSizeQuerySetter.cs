using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseCms.CRUDRepository;
using BackofficeDemo.Model.QuerySetter.Base;

namespace BackofficeDemo.Model.QuerySetter
{
    public class ImageSizeQuerySetter : MongoQueryInitializerBase<ImageSize>
    {
        protected override void Do()
        {
            base.Do();
            Register<LookupListQueryBase>(inp =>
            {
                var query = BaseContext;
                var str = inp.Input.Search ?? "";
                inp.Input.Output = query
                    .Where(p => string.IsNullOrEmpty(str) || p.Key.StartsWith(str))
                    .Take(inp.Input.Max)
                    .OrderBy(p => p.Key).AsEnumerable()
                    .Select(
                        p =>
                        new LookupItem
                        {
                            Id = p.Id.ToString(),
                            Name = $"{p.Key} ({p.Width}x{p.Height})"
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
                           new LookupItem { Id = p.Id.ToString(), Name = $"{p.Key} ({p.Width}x{p.Height})" })
                       .FirstOrDefault();
            });
        }
    }
}
