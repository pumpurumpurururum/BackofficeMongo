using System;

namespace BaseCms.Views.List.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ListViewGroupingMetadataAttribute : Attribute
    {
        public ListViewGroupingMetadataAttribute()
        {
            HideGroupingColumn = false;
            ExpandableGrouping = true;
        }

        public bool HideGroupingColumn { get; set; }
        public bool ExpandableGrouping { get; set; }
    }
}
