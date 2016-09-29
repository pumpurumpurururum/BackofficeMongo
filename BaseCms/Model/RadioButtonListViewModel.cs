using System;
using BaseCms.Model.LinkedList.Base;

namespace BaseCms.Model
{
    public class RadioButtonListViewModel : LinkedListViewModel
    {
        public string Selected { get; set; }
        public bool IsEditable { get; set; }
        public RadioButtonListViewModel()
        {
            Selected = "0";
        }
        public RadioButtonListViewModel(string propertyName, string collectionName, Type type)
            : base(propertyName, collectionName, type)
        {
            Selected = "0";
        }
    }
}
