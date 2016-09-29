using System;
using System.Collections.Generic;
using System.Linq;
using BaseCms.CRUDRepository;
using BaseCms.CRUDRepository.Core;
using BaseCms.DependencyResolution;

namespace BaseCms.Security.Queries
{
    public class Security_ChangeObjectStateIdPermissionQuerySetter : QueryInitializerBase
    {
        public override string CollectionName { get { return "Security_ChangeObjectStateIdPermission"; } }
        protected override void Do()
        {
            Register<GetTypeQueryBase>(c => typeof(ChangeObjectStateIdPermission));

            Register<GetIdentifierQueryBase>(inp => (inp.Object as Role).Id.ToString());

            Register<LookupListQueryBase>(inp =>
                {
                    // Получить сущности, для которых определён запрос GetStateMachineQueryBase
                    var allEntitiesWithChangeObjectStateQuery =
                        QueryResolver.GetAllKeys()
                                     .Where(key => key.ObjectType == typeof (GetStateMachineQueryBase))
                                     .Select(k => k.CollectionName);

                    var query = new List<ChangeObjectStateIdPermission>();

                    foreach (var entity in allEntitiesWithChangeObjectStateQuery)
                    {
                        var possibleStates = QueryResolver.Execute(new GetStateMachineQueryBase(), entity).GetStates();
                        query.AddRange(possibleStates.Select(possibleState => new ChangeObjectStateIdPermission
                            {
                                Collection = entity,
                                StateId = possibleState.Value,
                                StateName = possibleState.Name
                            }));
                    }

                    if (!String.IsNullOrEmpty(inp.Search))
                    {
                        query = query.Where(p => p.FullDescription.Contains(inp.Search)).ToList();
                    }
                    return query.Take(inp.Max)
                                .OrderBy(p => p.Collection)
                                .Select(
                                    p =>
                                    new LookupItem() {Id = p.Collection + "|" + p.StateId, Name = p.FullDescription})
                                .ToList();
                });

            Register<ToManyGetQueryBase>(inp =>
                {
                    var roleId = Guid.Parse(inp.Identifier);
                    return
                        IoC.SecurityProvider.Model.Roles.First(p => p.Id == roleId).Permissions
                           .Where(p => p.GetType() == typeof (ChangeObjectStateIdPermission))
                           .OfType<ChangeObjectStateIdPermission>()
                           .AsEnumerable()
                           .Select(p => new LookupItem {Id = p.Collection + "|" + p.StateId, Name = p.FullDescription})
                           .ToList();
                });

            Register<ToManySave>(inp =>
            {
                var roleId = Guid.Parse(inp.Identifier);
                var role = IoC.SecurityProvider.Model.Roles.First(r => r.Id == roleId);

                role.Permissions.RemoveAll(p => p.GetType() == typeof(ChangeObjectStateIdPermission));

                foreach (var entity in inp.Identifiers)
                {
                    var temp = entity.Split(new[] {'|'});
                    role.Permissions.Add(new ChangeObjectStateIdPermission
                        {
                            Collection = temp[0],
                            StateId = int.Parse(temp[1]),
                            Grant = true
                        });
                }
                
                IoC.SecurityProvider.Save();
            });

            Register<GetListCountQueryBase>(inp => QueryResolver.GetAllKeys().Where(key => key.ObjectType == typeof(GetStateMachineQueryBase)).Sum(entity => QueryResolver.Execute(new GetStateMachineQueryBase(), entity.CollectionName).GetStates().Count()));
            
        }
    }
}