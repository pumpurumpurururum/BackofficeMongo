using System.Collections.Generic;
using BaseCms.Manager.IconsForMenu;

namespace BaseCms.Menu
{
    public class MenuItem
    {
        public string Title { get; set; }
        public MenuIcon Icon { get; set; }

        //private List<SubmenuItem> _submenuItems;
        public List<SubmenuItem> SubmenuItems { get; set; }
        //{
        //    get { return _submenuItems ?? new List<SubmenuItem>(); }
        //    set { _submenuItems = value; }
        //}

        public MenuItem() { SubmenuItems = new List<SubmenuItem>(); }
    }
}
