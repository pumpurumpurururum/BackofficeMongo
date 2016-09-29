using System;

namespace BaseCms.Views.Xml
{
    public class XmlEditorMetadata
    {
        public Type XmlMetadataCollectionType { get; set; }
        public string XmlMetadataPropertyName { get; set; }
        public Type XmlMetadataValueProviderType { get; set; }
        public string ViewName { get; set; }
    }
}
