using System;

namespace BaseCms.Views.Tree.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class TreeMetadataAttribute : Attribute
    {
        public bool? IsHiddenValue;
        public bool? IsKeyValue;
        public bool? IsNameValue;
        public bool? HasChildrenPropertyValue;

        public string Format { get; set; }

        public bool IsHidden
        {
            get { throw new NotSupportedException(); }
            set { IsHiddenValue = value; }
        }

        public bool IsKey
        {
            set { IsKeyValue = value; }
            get { throw new NotSupportedException(); }
        }

        public bool IsName
        {
            set { IsNameValue = value; }
            get { throw new NotSupportedException(); }
        }

        public bool HasChildrenProperty
        {
            set { HasChildrenPropertyValue = value; }
            get { throw new NotSupportedException(); }
        }
    }
}
