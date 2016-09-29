using System;
using System.Collections.Generic;
using System.Linq;
using BaseCms.Views.Tree.Interfaces;

namespace BaseCms.Views.Tree
{
    public class TreeMetadata : ITreeMetadata
    {
        public IEnumerable<TreeMetadataItem> Items { get; private set; }
        public TreeMetadata(IEnumerable<TreeMetadataItem> items)
        {
            Items = items;
        }

        public Dictionary<string, object> Serialize(IEnumerable<object> objects)
        {
            var collection = objects.GetType().GetGenericArguments()[0];

            var keyItem = Items.FirstOrDefault(x => x.IsKey);
            if (keyItem == null) throw new Exception(String.Format("{0} has no key item for Tree", collection.Name));

            var nameItem = Items.FirstOrDefault(x => x.IsName);
            if (nameItem == null) throw new Exception(String.Format("{0} has no name item for Tree", collection.Name));

            var typeItem = Items.FirstOrDefault(x => x.HasChildrenProperty);
            if (typeItem == null) throw new Exception(String.Format("{0} has no children link item for Tree", collection.Name));

            var result = new Dictionary<string, object>();
            foreach (var o in objects)
            {
                var key = keyItem.GetValue(o).ToString();

                result.Add(key, new
                {
                    id = key,
                    name = nameItem.GetValue(o).ToString(),
                    type = Convert.ToBoolean(typeItem.GetValue(o)) ? "folder" : "item"
                });
            }

            return result;
        }
    }
}
