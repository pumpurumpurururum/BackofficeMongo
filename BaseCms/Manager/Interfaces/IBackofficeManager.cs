using System.Collections.Generic;
using StructureMap;
using BaseCms.Events;
using BaseCms.ThirdPartySystems.ExternalSystems;

namespace BaseCms.Manager.Interfaces
{
    public interface IBackofficeManager
    {
        void InitializeIoCContainer(ConfigurationExpression x);
        void SubscribeToEvents(EventSource eventSource);

        List<Menu.MenuItem> GetMenuItems();

        List<ExternalSystem> GetExternalSystems();

        Dictionary<string, string[]> GetStyles();

        Dictionary<string, string[]> GetScripts();

        Dictionary<string, string> GetPartials();
    }
}
