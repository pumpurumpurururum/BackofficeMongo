using System;
using System.Collections.Generic;
using System.Linq;
using BaseCms.CRUDRepository;
using BaseCms.Model.LinkedList.Base;

namespace BaseCms.Model.LinkedList
{
    public class LinkedListMultipleViewModel : LinkedListViewModel
    {
        public IEnumerable<LookupItem> Selected { get; set; }
        public bool IsEditable { get; set; }
        public LinkedListMultipleViewModel()
        {
            Selected = Enumerable.Empty<LookupItem>();
        }
        public LinkedListMultipleViewModel(string propertyName, string collectionName, Type type)
            : base(propertyName, collectionName, type)
        {
            Selected = Enumerable.Empty<LookupItem>();
        }
    }
}
