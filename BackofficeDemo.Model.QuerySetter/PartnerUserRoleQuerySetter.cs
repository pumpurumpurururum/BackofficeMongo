using System;
using System.Collections.Generic;
using System.Linq;
using BaseCms.CRUDRepository;
using BaseCms.CRUDRepository.DetailView;
using BaseCms.DependencyResolution;
using BackofficeDemo.Model.QuerySetter.Base;
using BackofficeDemo.Repository.Generic;

namespace BackofficeDemo.Model.QuerySetter
{
    public class PartnerUserRoleQuerySetter : MongoQueryInitializerBase<PartnerUserRole>
    {
        protected readonly IRepository<PartnerUser> UserContext = IoC.Container.GetInstance<IRepository<PartnerUser>>();

        protected override void Do()
        {
            base.Do();
            Register<GetLinkedListQueryBase>((inp =>
            {
                var dataContext = GetDataContext(inp.Input.DetailViewGuid);
                

                if (dataContext == null)
                {
                    Guid linkId;
                    if (Guid.TryParse(inp.Input.UpperIdentifier, out linkId) && linkId != Guid.Empty)
                    {
                        
                        var linkedroles = GetResultByFilters(new List<string>() {inp.Input.UpperIdentifier});
                        
                        dataContext = linkedroles;
                    }
                    else
                    {
                        dataContext = new List<PartnerUserRole>().AsQueryable();
                    }
                }

                var linked = dataContext.Select(p=>p.Id).ToList();

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

            Register<CreateToManyLink>((inp) =>
            {

                var userId = Guid.Parse(inp.Input.UpperIdentifier);
                var user = UserContext.GetById(userId);
                if (user != null)
                {
                    var roleId = Guid.Parse(inp.Input.LinkedIdentifier);
                    user.AddRole(roleId);
                    UserContext.Update(user);
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
                    var user = UserContext.GetById(upperId);
                    user.RemoveRole(id);
                    UserContext.Update(user);
                }
                else
                {
                    var obj = BaseContext.GetById(id);
                    var users = UserContext.ToList();
                    users.ForEach(p=>p.RemoveRole(id));
                    UserContext.Update(users);

                    BaseContext.Delete(obj);
                }
                
                
            });

            Register<ClearDetailViewObjects>(inp => SessionDataContainer.ClearData(inp.DetailViewGuid + CollectionName));

        }

        protected override IQueryable<PartnerUserRole> GetDataContext(string detailViewGuid, int? upperIdentifier = null)
        {
            if (String.IsNullOrEmpty(detailViewGuid)) return null;

            var sessionData = SessionDataContainer.GetData(detailViewGuid + CollectionName);
            if (sessionData == null)
            {
                if (!upperIdentifier.HasValue) return null;
                var l = QueryResolver.Execute(
                    new GetListQueryBase(0, int.MaxValue, "Id asc", new List<string> { upperIdentifier.ToString() }),
                    CollectionName).ToList();

                SessionDataContainer.SetData(detailViewGuid + CollectionName, l);
                sessionData = l;
            }
            return sessionData.OfType<PartnerUserRole>().AsQueryable();
        }
        public override IQueryable<PartnerUserRole> GetResultByFilters(List<string> filters)
        {
            var ret = BaseContext.AsQueryable();
            if (filters.Count > 0)
            {

                var userid = Guid.Empty;
                if (Guid.TryParse(filters[0], out userid) && userid != Guid.Empty)
                {
                       var user = UserContext.GetById(userid);
                        var roles = user.Roles;
                        ret = ret.Where(p => roles.Contains(p.Id)).AsQueryable();
                    
                }

               
               
            }
            return ret;
        }
    }
}
