using System;
using System.Linq;
using BaseCms.CRUDRepository;
using BaseCms.CRUDRepository.Core;
using BaseCms.DependencyResolution;
using BaseCms.Manager.Interfaces;

namespace BaseCms.Security.Queries
{
    public class Security_ExternalSystemAccessPermissionQuerySetter : QueryInitializerBase
    {
        private readonly IBackofficeManager _backOfficeManager = IoC.Container.GetInstance<IBackofficeManager>();

        public override string CollectionName { get { return "Security_ExternalSystemAccessPermission"; } }
        protected override void Do()
        {
            Register<GetTypeQueryBase>(c => typeof(ExternalSystemAccessPermission));

            Register<GetIdentifierQueryBase>(inp => (inp.Object as Role).Id.ToString());

            Register<LookupListQueryBase>(inp =>
                {
                    // Получить подключённые внешние системы
                    var extSystems = _backOfficeManager.GetExternalSystems();

                    var perms = extSystems.Select(sys => new ExternalSystemAccessPermission
                    {
                        ExternalSystem = sys.Name
                    });

                    inp.Output =
                        perms.Take(inp.Max)
                             .OrderBy(p => p.ExternalSystem)
                             .Select(p => new LookupItem() {Id = p.ExternalSystem, Name = p.FullDescription})
                             .ToList();
                });

            Register<ToManyGetQueryBase>(inp =>
                {
                    var roleId = Guid.Parse(inp.Identifier);
                    return
                        IoC.SecurityProvider.Model.Roles.First(p => p.Id == roleId).Permissions
                           .Where(p => p.GetType() == typeof (ExternalSystemAccessPermission))
                           .OfType<ExternalSystemAccessPermission>()
                           .AsEnumerable()
                           .Select(p => new LookupItem {Id = p.ExternalSystem, Name = p.FullDescription})
                           .ToList();
                });

            Register<ToManySave>(inp =>
            {
                var roleId = Guid.Parse(inp.Identifier);
                var role = IoC.SecurityProvider.Model.Roles.First(r => r.Id == roleId);

                role.Permissions.RemoveAll(p => p.GetType() == typeof(ExternalSystemAccessPermission));

                foreach (var extSys in inp.Identifiers)
                {
                    role.Permissions.Add(new ExternalSystemAccessPermission
                        {
                            ExternalSystem = extSys,
                            Grant = true
                        });
                }
                
                IoC.SecurityProvider.Save();
            });

            Register<GetListCountQueryBase>(inp => _backOfficeManager.GetExternalSystems().Count);
            
        }
    }
}