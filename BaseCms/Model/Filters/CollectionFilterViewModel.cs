using System.Collections.Generic;
using BaseCms.CRUDRepository;

namespace BaseCms.Model.Filters
{
    public class CollectionFilterViewModel
    {
        public string Id { get; set; }
        public string CollectionName { get; set; }
        public string Caption { get; set; }
        public bool IsPreloaded { get; set; }
        public List<LookupItem> PreloadedItems { get; set; }

        public string EmptyValueText { get; set; }
        public string EmptyValue { get; set; }

        public string RelatedId { get; set; }
    }
}
