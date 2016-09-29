using System;

namespace BaseCms.Views.Link.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class LinkedListsAttribute : Attribute
    {
        public LinkedListsAttribute(string groupName, string relatedList = "")
        {
            GroupName = groupName;
            RelatedList = relatedList;
        }

        public string GroupName { get; set; }
        public string RelatedList { get; set; }


    }
}
