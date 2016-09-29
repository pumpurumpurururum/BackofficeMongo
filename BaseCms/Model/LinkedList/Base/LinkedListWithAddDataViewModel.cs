using System;
using System.Collections.Generic;
using System.Linq;
using BaseCms.CRUDRepository;

namespace BaseCms.Model.LinkedList.Base
{
    public abstract class LinkedListWithAddDataViewModel
    {
        public string CollectionName { get; set; }
        public Type Type { get; set; }
        public string PropertyName { get; set; }
        public IEnumerable<LookupItemWithAddData> Preloaded { get; set; }


        protected LinkedListWithAddDataViewModel()
        {
            Preloaded = Enumerable.Empty<LookupItemWithAddData>();
        }
        protected LinkedListWithAddDataViewModel(string propertyName, string collectionName, Type type)
            : this()
        {
            CollectionName = collectionName;
            Type = type;
            PropertyName = propertyName;
        }
    }
}
