using System;

namespace BaseCms.Menu
{
    public class SubmenuItem
    {
        public Type CollectionType { get; set; }
        public string CollectionName { get; set; }

        public string Title { get; set; }
        public string ClassName { get; set; }
        public string[] Arguments { get; set; }

        public FilterDescription[] Filters { get; set; }
        public string InitFilters { get; set; }

        public string EditPermissionsFormat { get; set; }
        public string CommandString { get; set; }
    }
}
