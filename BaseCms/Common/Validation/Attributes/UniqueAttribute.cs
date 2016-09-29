using System;
using BaseCms.Common.Validation.Attributes.Base;

namespace BaseCms.Common.Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class UniqueAttribute : ValidationAttribute
    {
        private string CollectionName { get; set; }
        private string PropertyNames { get; set; }

        public UniqueAttribute(string collectionName, string propertyNames, string errorMessage = "",
                               int order = 0)
            : base(errorMessage, order)
        {
            CollectionName = collectionName;
            PropertyNames = propertyNames;
        }

        public override string Render()
        {
            return String.Format("unique(Значение должно быть уникальным!),{0};{1}", CollectionName, PropertyNames);
        }
    }
}
