using System;
using System.Linq;
using System.Linq.Dynamic;
using BaseCms.CRUDRepository;
using BaseCms.CRUDRepository.Core;
using BaseCms.DependencyResolution;

namespace BaseCms.Security.Queries
{
    public class Security_UserQuerySetter : QueryInitializerBase
    {
        public override string CollectionName { get { return "Security_User"; } }
        protected override void Do()
        {
            Register<GetTypeQueryBase>(c => typeof(User));

            Register<GetIdentifierQueryBase>(inp => (inp.Object as User).Id.ToString());

            Register<GetByIdQueryBase>((inp =>  IoC.SecurityProvider.Model.Users.FirstOrDefault(p => p.Id == Guid.Parse(inp.Id))));

            Register<GetByIdsQueryBase>((inp =>
            {
                var ids = inp.Ids.Select(Guid.Parse).ToArray();
                inp.Output = IoC.SecurityProvider.Model.Users.Where(e => ids.Contains(e.Id));
            }));

            Register<InsertObject>(inp => 
                {
                    ((User) inp.Object).Id = Guid.NewGuid();
                    IoC.SecurityProvider.Model.Users.Add((User)inp.Object);
                    IoC.SecurityProvider.Save();
                });

            Register<UpdateObject>(inp => 
                {
                    var index = IoC.SecurityProvider.Model.Users.FindIndex(p => p.Id == ((User)inp.Object).Id);
                    IoC.SecurityProvider.Model.Users.RemoveAt(index);
                    IoC.SecurityProvider.Model.Users.Insert(index, (User)inp.Object);
                    IoC.SecurityProvider.Save();
                });

            Register<DeleteObject>(inp =>
                {
                    var id = Guid.Parse(inp.Id);
                    var userToRoles = IoC.SecurityProvider.Model.UserToRoles.Where(p => p.UserId == id).ToList();
                    foreach (var utr in userToRoles)
                    {
                        IoC.SecurityProvider.Model.UserToRoles.Remove(utr);
                    }
                    IoC.SecurityProvider.Model.Users.RemoveAll(u => u.Id == id);
                    IoC.SecurityProvider.Save();
                });

            Register<GetListQueryBase>((inp => 
            {
                inp.Output = IoC.SecurityProvider.Model.Users
                                           .AsQueryable()
                                           .OrderBy(inp.SortBy)
                                           .Skip(inp.Index * inp.PageSize)
                                           .Take(inp.PageSize)
                                           .ToList();
            }));

            Register<GetListCountQueryBase>(inp => IoC.SecurityProvider.Model.Users.Count);

            Register<CheckUniqueQueryBase>((inp =>
                {
                    var id = !String.IsNullOrEmpty(inp.Identifier)
                                  ? Guid.Parse(inp.Identifier)
                                  : Guid.Empty;
                    switch (inp.PropertyName)
                    {
                        case "Email":
                            return
                                !IoC.SecurityProvider.Model.Users.Any(
                                    p => p.Email == inp.PropertyValue && p.Id != id);
                        case "Login":
                            return
                                !IoC.SecurityProvider.Model.Users.Any(
                                    p => p.Login == inp.PropertyValue && p.Id != id);
                    }
                    return false;
                }));
        }
    }
}