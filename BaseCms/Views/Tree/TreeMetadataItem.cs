using System;

namespace BaseCms.Views.Tree
{
    public class TreeMetadataItem
    {
        public string PropertyName { get; set; }
        public bool IsHidden { get; set; }
        public string Format { get; set; }

        public bool IsKey { get; set; }
        public bool IsName { get; set; }
        public bool HasChildrenProperty { get; set; }

        public Func<object, object> GetValue { get; set; }
    }
}
