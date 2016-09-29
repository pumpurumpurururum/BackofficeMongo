using System.Collections.Generic;

namespace BaseCms.Model
{
    public class MenuWithTabsViewModel
    {
        public static string IsActiveKey = "IsActive";
        public static string NameKey = "Display";
        public static string ItemCMSViewKey = "ItemCMSView";
        public static string ItemCMSViewName = "ItemCMSViewName";
        public static string IsVisible = "IsVisible";
        public string Name { get; set; }
        public bool Visible { get; set; }
        public IEnumerable<Item> Items { get; set; }
        public string Icon { get; set; }
        public static string CommandStr = "CommandStr";

        public static string IsCommand = "IsCommand";

        public static string ItemCMSViewKeyFormat = "ItemCMSViewFormat";
        public static string EditPermissionFormat = "EditPermissionFormat";

        public MenuWithTabsViewModel()
        {
            Visible = true;
            Icon = "icon-file";
        }
    }
}
