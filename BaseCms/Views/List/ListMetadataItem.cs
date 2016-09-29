using System;

namespace BaseCms.Views.List
{
    public class ListMetadataItem
    {
        public string Display { get; set; }
        public string PropertyName { get; set; }
        public int? Width { get; set; }
        public bool IsHidden { get; set; }
        public string Format { get; set; }
        public string Template { get; set; }
        public int Order { get; set; }
        public bool InitSort { get; set; }
        public string InitSortDir { get; set; }
        public bool NoSort { get; set; }
        public string ColumnClass { get; set; }
        public bool IsGroupColumn { get; set; }
        public Func<object, object> GetValue { get; set; }
    }
}
