using System.Collections.Generic;

namespace BaseCms.Views.List.Interfaces
{
    public interface IListMetadata
    {
        IEnumerable<ListMetadataItem> Items { get; }
        object[] Serialize(IEnumerable<object> objects);
    }
}
