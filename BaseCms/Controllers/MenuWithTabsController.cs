using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using BaseCms.Common.Attributes;
using BaseCms.DependencyResolution;
using BaseCms.Manager.IconsForMenu;
using BaseCms.Manager.Interfaces;
using BaseCms.Menu;
using BaseCms.Model;
using BaseCms.Security;

namespace BaseCms.Controllers
{
    public class MenuWithTabsController : Controller
    {
        private readonly Permission[] _currentUserPermissions;

        private static readonly List<MenuItem> MenuItems = IoC.Container.GetInstance<IBackofficeManager>().GetMenuItems();
        private static readonly MenuWithTabsViewModel[] MenuItemModels;

        static MenuWithTabsController()
        {
            AddSecurity();
            MenuItemModels = ConvertMenuItemsToMenuWithTabsViewModel(MenuItems).ToArray();
        }

        public MenuWithTabsController(SecurityProvider securityProvider)
        {
            _currentUserPermissions = securityProvider.GetPermissions(securityProvider.CurrentUser.Id).ToArray();
        }

        public ActionResult Read()
        {
            var model = new List<MenuWithTabsViewModel>();

            foreach (var item in MenuItemModels.SelectMany(i => i.Items))
            {
                item.Properties[MenuWithTabsViewModel.ItemCMSViewKey] = item.Properties[MenuWithTabsViewModel.ItemCMSViewKeyFormat].ToString()
                                                                                     .Replace("{PERMISSIONS}",
                                                                                              GetEditPermissons(
                                                                                                  item.Properties[MenuWithTabsViewModel.ItemCMSViewName].ToString(),
                                                                                                  item.Properties[MenuWithTabsViewModel.EditPermissionFormat].ToString()));
            }

            model.AddRange(MenuItemModels);

            ApplyPermissions(model);

            return View(model);
        }

        public void ApplyPermissions(List<MenuWithTabsViewModel> model)
        {
            var visibleCollections = _currentUserPermissions.OfType<GetListPermission>().Where(p => p.Grant).Select(p => p.Collection).ToArray();

            foreach (var tab in model)
            {
                foreach (var item in tab.Items)
                {
                    item.Properties[MenuWithTabsViewModel.IsVisible] = (bool)item.Properties[MenuWithTabsViewModel.IsCommand] || visibleCollections.Contains(item.Properties[MenuWithTabsViewModel.ItemCMSViewName]);
                }
                tab.Visible = tab.Items.Any(p => (bool)p.Properties[MenuWithTabsViewModel.IsVisible]);
            }
        }

        public string GetEditPermissons(string collectionName, string format)
        {
            var insertPermission = _currentUserPermissions.OfType<InsertObjectPermission>()
                                                           .Where(p => p.Grant)
                                                           .Any(p => p.Collection == collectionName);
            var updatePermission = _currentUserPermissions.OfType<UpdateObjectPermission>().Where(p => p.Grant)
                                                           .Any(p => p.Collection == collectionName);
            var deletePermission = _currentUserPermissions.OfType<DeleteObjectPermission>().Where(p => p.Grant)
                                                           .Any(p => p.Collection == collectionName);

            return String.Format(format, insertPermission, updatePermission, deletePermission).ToLower();
        }

        #region Построение меню

        private static IEnumerable<MenuWithTabsViewModel> ConvertMenuItemsToMenuWithTabsViewModel(IList<MenuItem> menuItems)
        {
            var result = new List<MenuWithTabsViewModel>(menuItems.Count);
            for (var i = 0; i < menuItems.Count; i++)
            {
                result.Add(new MenuWithTabsViewModel
                {
                    Name = menuItems[i].Title,
                    Icon = menuItems[i].Icon.ToDescriptionString()
                });

                var subitimsCount = menuItems[i].SubmenuItems != null ? menuItems[i].SubmenuItems.Count : 0;
                var items = new List<Item>(subitimsCount);
                var id = 0;
                var menuSubItems = menuItems[i].SubmenuItems;
                if (menuSubItems != null)
                    foreach (var menuSubItem in menuSubItems)
                    {
                        var collectionName = String.IsNullOrEmpty(menuSubItem.CollectionName) && menuSubItem.CollectionType !=null
                                                 ? menuSubItem.CollectionType.Name
                                                 : menuSubItem.CollectionName;
                        if (!string.IsNullOrEmpty(collectionName))
                        {
                            var cmsMetadataTypeAttribute =
                                menuSubItem.CollectionType.GetCustomAttributes(typeof (CmsMetadataTypeAttribute), true)
                                    .OfType<CmsMetadataTypeAttribute>()
                                    .FirstOrDefault();

                            var nameKey = String.IsNullOrEmpty(menuSubItem.Title)
                                ? cmsMetadataTypeAttribute == null
                                    ? collectionName
                                    : cmsMetadataTypeAttribute.CollectionTitlePlural
                                : menuSubItem.Title;

                            id++;
                            items.Add(new Item
                            {
                                Id = id.ToString(CultureInfo.InvariantCulture),
                                Properties = new Dictionary<string, object>
                                {
                                    {MenuWithTabsViewModel.IsActiveKey, false},
                                    {MenuWithTabsViewModel.IsVisible, true},
                                    {MenuWithTabsViewModel.ItemCMSViewName, collectionName},
                                    {MenuWithTabsViewModel.ItemCMSViewKey, String.Empty},
                                    {
                                        MenuWithTabsViewModel.ItemCMSViewKeyFormat,
                                        ConstructItemCMSViewKey(menuSubItem, collectionName, cmsMetadataTypeAttribute)
                                    },
                                    {
                                        MenuWithTabsViewModel.EditPermissionFormat,
                                        string.IsNullOrEmpty(menuSubItem.EditPermissionsFormat)
                                            ? "{0}, {1}, {2}"
                                            : menuSubItem.EditPermissionsFormat
                                    },
                                    {MenuWithTabsViewModel.NameKey, nameKey},
                                    {MenuWithTabsViewModel.IsCommand, false}
                                }
                            });
                        }
                        else if (!string.IsNullOrEmpty(menuSubItem.CommandString))
                        {
                            id++;
                            items.Add(new Item
                            {
                                Id = id.ToString(CultureInfo.InvariantCulture),
                                Properties = new Dictionary<string, object>
                                {
                                    {MenuWithTabsViewModel.IsActiveKey, false},
                                    {MenuWithTabsViewModel.IsVisible, true},
                                    {MenuWithTabsViewModel.NameKey, menuSubItem.Title},
                                    {
                                        MenuWithTabsViewModel.EditPermissionFormat,
                                        string.IsNullOrEmpty(menuSubItem.EditPermissionsFormat)
                                            ? "{0}, {1}, {2}"
                                            : menuSubItem.EditPermissionsFormat
                                    },
                                    {MenuWithTabsViewModel.ItemCMSViewName, string.Empty},
                                    {MenuWithTabsViewModel.ItemCMSViewKey, String.Empty},
                                    {MenuWithTabsViewModel.ItemCMSViewKeyFormat,string.Empty},
                                    {MenuWithTabsViewModel.IsCommand, true},
                                    {MenuWithTabsViewModel.CommandStr, menuSubItem.CommandString }
                                }
                            });
                        }
                    }
                result[i].Items = items;
            }

            return result;
        }

        private static string ConstructItemCMSViewKey(SubmenuItem subItem, string collectionName, CmsMetadataTypeAttribute attribute)
        {
            var itemCmsViewKeyBuilder = new StringBuilder();
            itemCmsViewKeyBuilder.Append("new ");

            itemCmsViewKeyBuilder.AppendFormat("{0}(", String.IsNullOrEmpty(subItem.ClassName)
                                                           ? "CMSViews.ListView"
                                                           : subItem.ClassName);

            if (subItem.Arguments != null)
            {
                foreach (var arg in subItem.Arguments)
                {
                    itemCmsViewKeyBuilder.Append(arg);
                }
            }
            else
            {
                itemCmsViewKeyBuilder.AppendFormat("'{0}', ", collectionName);
                itemCmsViewKeyBuilder.AppendFormat("function(id, view) {{ _CMSScope_.createOrActivateTab('{0} #' + id, view); }}, ", attribute == null ? collectionName : attribute.CollectionTitleSingular);
                itemCmsViewKeyBuilder.Append("null, ");

                if ((subItem.Filters == null) || (subItem.Filters.Length == 0))
                {
                    itemCmsViewKeyBuilder.Append("null, ");
                }
                else
                {

                    var filters = (
                        from t in subItem.Filters 
                        let args = string.Join(",", t.Arguments) 
                        select $"new {t.ClassName}({args})").ToList();
                    itemCmsViewKeyBuilder.AppendFormat("[{0}],", string.Join(", ", filters));
                   
                }

                itemCmsViewKeyBuilder.Append("[{PERMISSIONS}]");

                if (!String.IsNullOrEmpty(subItem.InitFilters))
                    itemCmsViewKeyBuilder.AppendFormat(", '{0}'", subItem.InitFilters);
            }

            itemCmsViewKeyBuilder.Append(")");
            return itemCmsViewKeyBuilder.ToString();
        }

        private static void AddSecurity()
        {
            MenuItems.Add(new MenuItem
            {
                Icon = MenuIcon.shield,
                Title = "Security",
                SubmenuItems = new List<SubmenuItem>
                        {
                            new SubmenuItem
                                {
                                    CollectionName = "Security_User",
                                    CollectionType = typeof (User),
                                },
                            new SubmenuItem
                                {
                                    CollectionName = "Security_Role",
                                    CollectionType = typeof (Role),
                                }
                        }
            });
        }

        #endregion
    }
}
