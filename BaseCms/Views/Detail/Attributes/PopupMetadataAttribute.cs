using System;

namespace BaseCms.Views.Detail.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PopupMetadataAttribute : Attribute
    {
        public PopupMetadataAttribute()
        {
            ViewName = "PopupEdit";
            Title = String.Empty;

            Top = String.Empty;
            Left = String.Empty;

            Height = String.Empty;
            Width = String.Empty;

            NoScroll = false;
        }

        public string ViewName { get; set; }
        public string Title { get; set; }

        public string Top { get; set; }
        public string Left { get; set; }

        public string Height { get; set; }
        public string Width { get; set; }

        public bool NoScroll { get; set; }
    }
}
