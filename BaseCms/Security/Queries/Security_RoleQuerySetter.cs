using System;
using System.Linq;
using System.Linq.Dynamic;
using BaseCms.CRUDRepository;
using BaseCms.CRUDRepository.Core;
using BaseCms.DependencyResolution;

namespace BaseCms.Security.Queries
{
    public class Security_RoleQuerySetter : QueryInitializerBase
    {
        public override string CollectionName { get { return "Security_Role"; } }
        protected override void Do()
        {
            Register<GetTypeQueryBase>(c => typeof(Role));

            Register<GetIdentifierQueryBase>(inp => (inp.Object as Role).Id.ToString());

            Register<GetByIdQueryBase>((inp =>  IoC.SecurityProvider.Model.Roles.FirstOrDefault(p => p.Id == Guid.Parse(inp.Id))));

            Register<GetByIdsQueryBase>((inp =>
            {
                var ids = inp.Ids.Select(Guid.Parse).ToArray();
                inp.Output = IoC.SecurityProvider.Model.Roles.Where(e => ids.Contains(e.Id));
            }));

            Register<InsertObject>(inp => 
                {
                    ((Role) inp.Object).Id = Guid.NewGuid();
                    IoC.SecurityProvider.Model.Roles.Add((Role)inp.Object);
                    IoC.SecurityProvider.Save();
                });

            Register<UpdateObject>(inp =>
                {
                    var index = IoC.SecurityProvider.Model.Roles.FindIndex(p => p.Id == ((Role)inp.Object).Id);
                    IoC.SecurityProvider.Model.Roles.RemoveAt(index);
                    IoC.SecurityProvider.Model.Roles.Insert(index, (Role)inp.Object);
                    IoC.SecurityProvider.Save();
                });

            Register<DeleteObject>(inp =>
                {
                    var id = Guid.Parse(inp.Id);
                    var userToRoles = IoC.SecurityProvider.Model.UserToRoles.Where(p => p.RoleId == id).ToList();
                    foreach (var utr in userToRoles)
                    {
                        IoC.SecurityProvider.Model.UserToRoles.Remove(utr);
                    }
                    IoC.SecurityProvider.Model.Roles.RemoveAll(r => r.Id == id);
                    IoC.SecurityProvider.Save();
                });

            Register<GetListQueryBase>((inp => 
            {
                inp.Output = IoC.SecurityProvider.Model.Roles
                                           .AsQueryable()
                                           .OrderBy(inp.SortBy)
                                           .Skip(inp.Index * inp.PageSize)
                                           .Take(inp.PageSize)
                                           .ToList();
            }));

            Register<GetListCountQueryBase>(inp => IoC.SecurityProvider.Model.Roles.Count);
            
        }
    }
}