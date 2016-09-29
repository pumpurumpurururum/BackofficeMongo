using System;
using BaseCms.CRUDRepository;
using BaseCms.Model.LinkedList.Base;

namespace BaseCms.Model.LinkedList
{
    public class LinkedListSingleViewModel : LinkedListViewModel
    {
        public LookupItem Selected { get; set; }
        public string Filter { get; set; }
        public string AdditionalClass { get; set; }
        public string Validate { get; set; }
        public bool IsEditable { get; set; }
        public string ParentProperty { get; set; }
        public LinkedListSingleViewModel()
        {

        }
        public LinkedListSingleViewModel(string propertyName, string collectionName, Type type, string filter = null, string additionalClass = null, string parentProperty = null)
            : base(propertyName, collectionName, type)
        {
            Filter = filter;
            AdditionalClass = additionalClass;
            ParentProperty = parentProperty;
        }
    }
}
