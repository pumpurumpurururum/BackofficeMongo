using System;

namespace BaseCms.Views.LinkDDL
{
    public class DDLMetadata
    {
        public Func<object, string> GetKey { get; set; }
        public Func<object, string> GetValue { get; set; }
    }
}
