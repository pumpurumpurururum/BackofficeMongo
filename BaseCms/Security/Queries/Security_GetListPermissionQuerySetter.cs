using System;
using System.Linq;
using BaseCms.CRUDRepository;
using BaseCms.CRUDRepository.Core;
using BaseCms.DependencyResolution;

namespace BaseCms.Security.Queries
{
    public class Security_GetListPermissionQuerySetter : QueryInitializerBase
    {
        public override string CollectionName { get { return "Security_GetListPermission"; } }
        protected override void Do()
        {
            Register<GetTypeQueryBase>(c => typeof(GetListPermission));

            Register<GetIdentifierQueryBase>(inp => (inp.Object as Role).Id.ToString());

            Register<LookupListQueryBase>((inp =>
            {
                var allEntitiesWithGetListQuery = QueryResolver.GetAllKeys().Where(key => key.ObjectType == typeof(GetListQueryBase)).Select(k => k.CollectionName);
                
                var query = allEntitiesWithGetListQuery.Select(ent => new GetListPermission
                    {
                        Collection = ent
                    });

                if (!String.IsNullOrEmpty(inp.Search))
                {
                    query = query.Where(p => p.FullDescription.Contains(inp.Search));
                }
                inp.Output = query.Take(inp.Max).OrderBy(p => p.Collection).Select(p => new LookupItem() { Id = p.Collection, Name = p.FullDescription })
                         .ToList();
            }));

            Register<ToManyGetQueryBase>((inp =>
            {
                var roleId = Guid.Parse(inp.Identifier);
                inp.Output = 
                    IoC.SecurityProvider.Model.Roles.First(p => p.Id == roleId).Permissions
                       .Where(p => p.GetType() == typeof(GetListPermission))
                       .OfType<GetListPermission>()
                       .AsEnumerable()
                       .Select(p => new LookupItem { Id = p.Collection, Name = p.FullDescription })
                       .ToList();
            }));

            Register<ToManySave>((inp =>
            {
                var roleId = Guid.Parse(inp.Identifier);
                var role = IoC.SecurityProvider.Model.Roles.First(r => r.Id == roleId);

                role.Permissions.RemoveAll(p => p.GetType() == typeof(GetListPermission));

                foreach (var entity in inp.Identifiers)
                {
                    role.Permissions.Add(new GetListPermission
                        {
                            Collection = entity,
                            Grant = true
                        });
                }
                
                IoC.SecurityProvider.Save();
            }));

            Register<GetListCountQueryBase>((inp => QueryResolver.GetAllKeys().Count(key => key.ObjectType == typeof(GetListQueryBase))));

        }
    }
}