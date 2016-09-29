using System;
using BaseCms.Views.Detail.Interfaces;

namespace BaseCms.Views.Detail
{
    public class TypeItem : ITypeItem, ICloneable
    {
        public Type Type { get; set; }
        public Func<string, object> ParseMethod { get; set; }
        public string Template { get; set; }
        public string ListViewName { get; set; }

        public object Clone()
        {
            return new TypeItem()
            {
                Type = this.Type,
                ParseMethod = this.ParseMethod,
                Template = this.Template
            };
        }
    }
}
