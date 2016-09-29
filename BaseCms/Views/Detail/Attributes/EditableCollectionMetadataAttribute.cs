using System;

namespace BaseCms.Views.Detail.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class EditableCollectionMetadataAttribute : Attribute
    {
        public EditableCollectionMetadataAttribute()
        {
            ViewName = "EditableCollection";
        }

        public string ViewName { get; set; }
    }
}
