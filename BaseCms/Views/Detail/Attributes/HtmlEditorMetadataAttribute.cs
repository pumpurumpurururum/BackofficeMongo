using System;

namespace BaseCms.Views.Detail.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class HtmlEditorMetadataAttribute : Attribute
    {
        public bool CssFromConfig { get; set; }
        public string Css { get; set; }

        public string CssSelectorsForRemoveFormat { get; set; }

        public bool ImagesUrlFromConfig { get; set; }
        public string ImagesUrl { get; set; }
    }
}
