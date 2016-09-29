using System;

namespace BaseCms.Views.Xml.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class XmlEditorMetadataAttribute : Attribute
    {
        public XmlEditorMetadataAttribute()
        {
            XmlMetadataCollectionType = null;
            XmlMetadataPropertyName = "";
            XmlMetadataValueProviderType = null;
            ViewName = "XmlEditor";
        }

        public Type XmlMetadataCollectionType { get; set; }
        public string XmlMetadataPropertyName { get; set; }
        public Type XmlMetadataValueProviderType { get; set; }
        public string ViewName { get; set; }
    }
}
