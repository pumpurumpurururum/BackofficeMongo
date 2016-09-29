using System.Collections.Generic;
using System.Linq;
using BaseCms.Views.Detail.Interfaces;

namespace BaseCms.Views.Detail
{
    public class DetailMetadata : IDetailMetadata
    {
        public IEnumerable<DetailMetadataItem> Items { get; set; }
        public string CustomEditView { get; set; }
        public string CustomReadView { get; set; }
        public bool ForbidReadMode { get; set; }

        public string ListViewHeaderPlural { get; set; }
        public string TabsWithBlocks { get; set; }

        public object GetValue(object data, string propertyName)
        {
            var property = Items.First(p => p.PropertyName.Equals(propertyName));
            return property.GetValue(data);
        }

        public DetailMetadataItem GetItem(string propertyName)
        {
            return Items.First(p => p.PropertyName.Equals(propertyName));
        }
    }
}
