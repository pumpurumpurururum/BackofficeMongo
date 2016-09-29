using System;

namespace BaseCms.Views.Detail.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AutoTypographMetadataAttribute : Attribute
    {
        public Type AutoTypographHelperType { get; set; }
    }
}
