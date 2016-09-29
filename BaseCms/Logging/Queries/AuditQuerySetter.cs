using System;
using System.Linq;
using System.Linq.Dynamic;
using BaseCms.CRUDRepository;
using BaseCms.CRUDRepository.Core;
using BaseCms.CRUDRepository.Serialization.Interfaces;
using BaseCms.DependencyResolution;
using BaseCms.Logging.Interfaces;

namespace BaseCms.Logging.Queries
{
    public class AuditQuerySetter : QueryInitializerBase
    {
        private static readonly ILogger Logger = IoC.Container.GetInstance<ILogger>();

        public override string CollectionName { get { return "Audit"; } }
        protected override void Do()
        {
            Register<GetTypeQueryBase>(c => typeof(Audit));

            Register<GetListQueryBase>(inp =>
            {
                var audit = Logger.GetHistory(inp.Filters[0], inp.Filters[1]).ToArray();

                foreach (var a in audit)
                {
                    if (a.CommandData.Contains("UpdateObject"))
                    {
                        a.ChangeType = "обновлено";
                        continue;
                    }
                    if (a.CommandData.Contains("InsertObject"))
                    {
                        a.ChangeType = "создано";
                        continue;
                    }
                    if (a.CommandData.Contains("DeleteObject"))
                    {
                        a.ChangeType = "удалено";
                    }
                    if (a.CommandData.Contains("Unite"))
                    {
                        a.ChangeType = "произведено объединение";
                    }
                    if (!a.CommandData.Contains("ChangeObjectStateId")) continue;
                    var command = IoC.Container.GetInstance<ISerializer>().Deserialize<ChangeObjectStateId>(a.CommandData);

                    a.ChangeType = "изменено состояние";

                    if (QueryResolver.ContainsQuery(typeof(GetStateMachineQueryBase), a.CollectionName))
                    {
                        a.ChangeType += String.Format(" на \"{0}\"",
                                                      QueryResolver.Execute(new GetStateMachineQueryBase(),
                                                                            a.CollectionName)
                                                                   .GetState(int.Parse(command.StateId))
                                                                   .Name);
                    }
                }

                return audit.AsQueryable().OrderBy(inp.SortBy)
                            .Skip(inp.Index * inp.PageSize)
                            .Take(inp.PageSize)
                            .ToList();
            });

            Register<GetListCountQueryBase>(inp => Logger.GetHistory(inp.Filters[0], inp.Filters[1]).Count());
        }
    }
}
