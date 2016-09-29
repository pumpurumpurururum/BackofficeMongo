using System;
using BaseCms.Model.Interfaces;

namespace BaseCms.Views.LinkDDL
{
    public class DefaultLinkDDLMetadataProvider : IMetadataProvider<DDLMetadata>
    {
        public DDLMetadata GetMetadata(Type type)
        {
            return new DDLMetadata()
            {
                GetKey = (o) => type.GetProperty("Id").GetValue(o).ToString(),
                GetValue = (o) => type.GetProperty("Value").GetValue(o).ToString()
            };
        }
    }
}
