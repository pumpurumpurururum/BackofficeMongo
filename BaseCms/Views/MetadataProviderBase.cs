using System;
using System.Linq;
using System.Reflection;

namespace BaseCms.Views
{
    public class MetadataProviderBase
    {
        protected T AggregateAttributes<T>(PropertyInfo propertyInfo, PropertyInfo overridedPropertyInfo, Func<T, T, T> agregator, Func<T> defaultAttr) where T : Attribute
        {
            var attributesList = propertyInfo.GetCustomAttributes<T>().ToList();
            var overridedAttributesList = Enumerable.Empty<T>();
            if (overridedPropertyInfo != null)
            {
                overridedAttributesList = overridedPropertyInfo.GetCustomAttributes<T>().ToList();
            }
            return attributesList.Concat(overridedAttributesList).DefaultIfEmpty(defaultAttr()).Aggregate(agregator);
        }
    }
}
