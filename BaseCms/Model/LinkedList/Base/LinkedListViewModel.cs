using System;
using System.Collections.Generic;
using System.Linq;
using BaseCms.CRUDRepository;

namespace BaseCms.Model.LinkedList.Base
{
    public abstract class LinkedListViewModel
    {
        public string CollectionName { get; set; }
        public Type Type { get; set; }
        public string PropertyName { get; set; }
        public IEnumerable<LookupItem> Preloaded { get; set; }


        protected LinkedListViewModel()
        {
            Preloaded = Enumerable.Empty<LookupItem>();
        }
        protected LinkedListViewModel(string propertyName, string collectionName, Type type)
            : this()
        {
            CollectionName = collectionName;
            Type = type;
            PropertyName = propertyName;
        }
    }
}
