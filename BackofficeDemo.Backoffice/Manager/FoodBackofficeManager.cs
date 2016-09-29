using System.Collections.Generic;
using BaseCms.Common.FileUploader.Interfaces;
using BaseCms.Events;
using BaseCms.Logging.Interfaces;
using BaseCms.Manager.IconsForMenu;
using BaseCms.Manager.Interfaces;
using BaseCms.Menu;
using BaseCms.MetadataValueProviders.Interfaces;
using BaseCms.ThirdPartySystems.ExternalSystems;
using BackofficeDemo.Backoffice.Helpers;
using BackofficeDemo.Common.ImageProcessing;
using BackofficeDemo.Model;
using BackofficeDemo.Repository;
using StructureMap;
using MenuItem = BaseCms.Menu.MenuItem;

namespace BackofficeDemo.Backoffice.Manager
{
    public class FoodBackofficeManager : IBackofficeManager
    {
        public void InitializeIoCContainer(ConfigurationExpression x)
        {
            var dalManager = new DalManager();
            dalManager.InitializeIoCContainer(x);
            x.For<IFileManagerWithTemporaryStorage>().Singleton().Use(new ImageUploader());
            x.For<IPageSettingsValueProvider>().Singleton().Use(new MyPageSettingsValueProvider());
            x.For<ILogger>().Singleton().Use(new Logger());
            
        }

        public void SubscribeToEvents(EventSource eventSource)
        {
            //throw new NotImplementedException();
        }



        public List<MenuItem> GetMenuItems()
        {

            return new List<MenuItem>
            {
                new MenuItem
                {
                    Icon = MenuIcon.sitemap,
                    Title = "Partners",
                    SubmenuItems = new List<SubmenuItem>
                    {
                        new SubmenuItem  { CollectionType = typeof(Partner)},
                        new SubmenuItem  { CollectionType = typeof(PartnerUser) },
                        new SubmenuItem  { CollectionType = typeof(Product) }
                    }
                },
                new MenuItem
                {
                    Icon = MenuIcon.exclamationCircle,
                    Title = "Settings",
                    SubmenuItems = new List<SubmenuItem>
                    {
                        new SubmenuItem  { CollectionType = typeof(PartnerType) },
                        new SubmenuItem  { CollectionType = typeof(PartnerCategory) },
                        new SubmenuItem  { CollectionType = typeof(PartnerUserRole) },
                        
                        new SubmenuItem { CollectionType = typeof(ImageSize)}
                    }
                },
                new MenuItem
                {
                    Icon = MenuIcon.globe,
                    Title = "Locations",
                    SubmenuItems = new List<SubmenuItem>
                    {
                        
                        new SubmenuItem { CollectionType = typeof(City)}
                    }
                },
                new MenuItem
                {
                    Icon = MenuIcon.bolt,
                    Title = "Tools",
                    SubmenuItems = new List<SubmenuItem>
                    {
                        new SubmenuItem { Title = "Remove images", CommandString = "my.removeImagesBySize()" },
                        new SubmenuItem { Title = "Rebuild indexes", CommandString = "my.rebuildIndexes()" }
                    }
                }
            };
        }

        public List<ExternalSystem> GetExternalSystems()
        {
            return new List<ExternalSystem>();
        }

        public Dictionary<string, string[]> GetStyles()
        {
            return new Dictionary<string, string[]>();
        }

        public Dictionary<string, string[]> GetScripts()
        {
            return new Dictionary<string, string[]>
                {
                    {
                        "Root",
                        new[]
                            {
                                "~/Scripts/BillingTransaction.js",
                                "~/Scripts/scripts.js"
                            }
                    }
                };
        }

        public Dictionary<string, string> GetPartials()
        {
            return new Dictionary<string, string>
            {
                //{"SidebarShortcuts", "SidebarShortcuts"},
                //{"LoginHeader", "LoginHeader"}
            };
        }
    }
}
