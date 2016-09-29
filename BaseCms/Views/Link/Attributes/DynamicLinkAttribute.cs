using System;

namespace BaseCms.Views.Link.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DynamicLinkAttribute : Attribute
    {
        public DynamicLinkAttribute(string targetCollection, bool isPreloaded = true)
        {
            TargetCollection = targetCollection;
            IsPreloaded = isPreloaded;
        }

        public bool IsPreloaded { get; set; }
        public string TargetCollection { get; set; }

        public bool DetailViewSave { get; set; }
    }
}
