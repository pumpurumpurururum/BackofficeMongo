using System;

namespace BaseCms.Views.List.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ListMetadataAttribute : Attribute
    {
        public int? WidthValue;
        public bool? IsHiddenValue;

        public string Display { get; set; }
        public string Template { get; set; }
        public string Format { get; set; }
        public int? OrderValue { get; set; }

        public int Width
        {
            get { throw new NotSupportedException(); }
            set { WidthValue = value; }
        }

        public bool IsHidden
        {
            get { throw new NotSupportedException(); }
            set { IsHiddenValue = value; }
        }

        public int Order
        {
            get { throw new NotSupportedException(); }
            set { OrderValue = value; }
        }

        public bool InitSortValue { get; set; }
        public bool InitSort
        {
            get { throw new NotSupportedException(); }
            set { InitSortValue = value; }
        }

        public string InitSortDirValue { get; set; }
        public string InitSortDir
        {
            get { throw new NotSupportedException(); }
            set { InitSortDirValue = value; }
        }

        public bool NoSort { get; set; }
        public string ColumnClass { get; set; }
        public bool IsGroupColumn { get; set; }
    }
}
