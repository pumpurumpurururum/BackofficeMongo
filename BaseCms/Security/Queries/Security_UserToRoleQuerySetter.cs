using System;
using System.Linq;
using System.Linq.Dynamic;
using BaseCms.CRUDRepository;
using BaseCms.CRUDRepository.Core;
using BaseCms.DependencyResolution;

namespace BaseCms.Security.Queries
{
    public class SecurityUserToRoleQuerySetter : QueryInitializerBase
    {
        public override string CollectionName { get { return "Security_UserToRole"; } }
        protected override void Do()
        {
            Register<GetTypeQueryBase>(c => typeof(User));

            Register<GetIdentifierQueryBase>(inp => (inp.Object as User).Id.ToString());

            Register<DeleteObject>(inp =>
                {
                    var id = Guid.Parse(inp.Id);
                    var obj = IoC.SecurityProvider.Model.Roles.FirstOrDefault(r => r.Id == id);
                    IoC.SecurityProvider.Model.Roles.Remove(obj);
                    IoC.SecurityProvider.Save();
                });

            Register<GetListQueryBase>((inp =>  IoC.SecurityProvider.Model.Roles
                                                                              .AsQueryable()
                                                                              .OrderBy(inp.SortBy)
                                                                              .Skip(inp.Index*inp.PageSize)
                                                                              .Take(inp.PageSize)
                                                                              .ToList()));

            Register<GetListCountQueryBase>((inp => IoC.SecurityProvider.Model.Roles.Count()));

            Register<LookupListQueryBase>((inp =>
            {
                var query = IoC.SecurityProvider.Model.Roles.AsQueryable();
                if (!String.IsNullOrEmpty(inp.Search))
                {
                    query = query.Where(p => p.Name.Contains(inp.Search));
                }
                inp.Output = query.Take(inp.Max).Select(p => new LookupItem() { Id = p.Id.ToString(), Name = p.Name }).ToList();
            }));

            Register<ToManySave>((inp =>
                {
                    var userId = Guid.Parse(inp.Identifier);
                    var links = IoC.SecurityProvider.Model.UserToRoles.Where(p => p.UserId == userId).ToList();
                    foreach (var link in links)
                    {
                        IoC.SecurityProvider.Model.UserToRoles.Remove(link);
                    }
                    foreach (var linkid in inp.Identifiers)
                    {
                        var roleId = Guid.Parse(linkid);
                        IoC.SecurityProvider.Model.UserToRoles.Add(new UserToRole
                            {
                                UserId = userId,
                                RoleId = roleId
                            });
                    }
                    IoC.SecurityProvider.Save();
                }));

            Register<ToManyGetQueryBase>((inp =>
                {
                    var userId = Guid.Parse(inp.Identifier);
                    inp.Output = 
                        IoC.SecurityProvider.Model.UserToRoles.Where(p => p.UserId == userId)
                           .Select(
                               p =>
                               new
                                   {
                                       Id = p.RoleId,
                                       Value = IoC.SecurityProvider.Model.Roles.First(r => r.Id == p.RoleId).Name
                                   })
                           .AsEnumerable()
                           .Select(p => new LookupItem {Id = p.Id.ToString(), Name = p.Value})
                           .ToList();
                }));
        }
    }
}