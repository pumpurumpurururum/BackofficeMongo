using System;

namespace BaseCms.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CmsMetadataTypeAttribute : Attribute
    {
        public Type Type { get; set; }
        public bool ForbidReadMode { get; set; }
        public string CustomEditView { get; set; }

        public string CollectionTitleSingular { get; set; }
        public string CollectionTitlePlural { get; set; }
        public string Sizes { get; set; }
        public string TabsWithBlocks { get; set; }

        public string CustomListView { get; set; }

        public CmsMetadataTypeAttribute(Type type, string customEditView = null, bool forbidReadMode = false, string listViewHeaderPlural = "", string sizes = "")
        {
            Type = type;
            CustomEditView = customEditView;
            ForbidReadMode = forbidReadMode;

            CollectionTitlePlural = listViewHeaderPlural;
            Sizes = sizes;
        }
    }
}
