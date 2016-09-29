using System;
using BaseCms.CRUDRepository;
using BaseCms.Model.LinkedList.Base;

namespace BaseCms.Model.LinkedList
{
    public class LinkedListWithAddDataSingleViewModel : LinkedListWithAddDataViewModel
    {
        public LookupItemWithAddData Selected { get; set; }
        public string Filter { get; set; }
        public string AdditionalClass { get; set; }
        public string Validation { get; set; }
        public bool IsEditable { get; set; }

        public LinkedListWithAddDataSingleViewModel()
        {

        }
        public LinkedListWithAddDataSingleViewModel(string propertyName, string collectionName, Type type, string filter = null, string additionalClass = null, string validation = null)
            : base(propertyName, collectionName, type)
        {
            Filter = filter;
            AdditionalClass = additionalClass;
            Validation = validation;
        }
    }
}
