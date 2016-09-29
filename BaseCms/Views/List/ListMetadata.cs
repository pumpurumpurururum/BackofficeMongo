using System;
using System.Collections.Generic;
using BaseCms.Views.List.Interfaces;

namespace BaseCms.Views.List
{
    public class ListMetadata : IListMetadata
    {
        public IEnumerable<ListMetadataItem> Items { get; private set; }
        private readonly Func<IEnumerable<object>, object[]> _serialize;
        public ListMetadata(Func<IEnumerable<object>, object[]> serialize, IEnumerable<ListMetadataItem> items)
        {
            _serialize = serialize;
            Items = items;
        }

        public object[] Serialize(IEnumerable<object> objects)
        {
            return _serialize(objects);
        }
    }
}
