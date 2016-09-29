using System;
using System.Linq;
using System.Reflection;
using System.Web;
using BaseCms.Common.Attributes;
using BaseCms.Model.Interfaces;
using BaseCms.Views.Tree.Attributes;

namespace BaseCms.Views.Tree
{
    public class DefaultTreeMetadataProvider : MetadataProviderBase, IMetadataProvider<TreeMetadata>
    {
        public TreeMetadata GetMetadata(Type type)
        {
            var overridedProperties = new PropertyInfo[0];
            var overridedObjectTypeAttribute = type.GetCustomAttribute<CmsMetadataTypeAttribute>(true);
            if (overridedObjectTypeAttribute != null)
            {
                var overridedType = overridedObjectTypeAttribute.Type;
                overridedProperties =
                    overridedType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty |
                                                BindingFlags.SetProperty).Where(p => p.GetCustomAttribute<TreeMetadataAttribute>() != null).ToArray();
            }

            Func<object, string, string, string> getStringValueByName = (obj, propName, format) =>
            {
                var prop = type.GetProperty(propName);
                try
                {
                    var result = prop.GetValue(obj);
                    if (!String.IsNullOrEmpty(format))
                    {
                        var template = "{0:" + format + "}";
                        return result != null ? HttpUtility.HtmlEncode(String.Format(template, result)) : "";
                    }

                    if (result is DateTime) return HttpUtility.HtmlEncode(String.Format("{0:dd.MM.yyyy}", result));

                    return result != null ? HttpUtility.HtmlEncode(result.ToString()) : "";
                }
                catch (Exception)
                {
                    //Мы не смогли прочитать значение. оставляем его пустым
                }
                return "";
            };

            var items = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty |
                                   BindingFlags.SetProperty)
                    .Select(s => CreateMetadata(s, overridedProperties.FirstOrDefault(p => p.Name == s.Name), getStringValueByName)).ToArray();

            return new TreeMetadata(items);
        }

        private TreeMetadataItem CreateMetadata(PropertyInfo propertyInfo, PropertyInfo overridedPropertyInfo,
                                                Func<object, string, string, string> getStringValueByName)
        {
            //"Наложение аттрибутов, если их несколько на одном свойстве

            var attribute = AggregateAttributes(
                propertyInfo,
                overridedPropertyInfo,
                (a, b) => new TreeMetadataAttribute
                    {
                    IsHiddenValue = b.IsHiddenValue ?? a.IsHiddenValue,
                    Format = b.Format ?? a.Format,
                    HasChildrenPropertyValue = b.HasChildrenPropertyValue ?? a.HasChildrenPropertyValue,
                    IsKeyValue = b.IsKeyValue ?? a.IsKeyValue,
                    IsNameValue = b.IsNameValue ?? a.IsNameValue
                },
                () => new TreeMetadataAttribute { IsHidden = true });


            return new TreeMetadataItem
                {
                GetValue = o => getStringValueByName(o, propertyInfo.Name, attribute.Format),
                PropertyName = propertyInfo.Name,
                IsHidden =
                    attribute.IsHiddenValue ??
                    overridedPropertyInfo == null,
                Format = attribute.Format,
                HasChildrenProperty = attribute.HasChildrenPropertyValue ?? false,
                IsKey = attribute.IsKeyValue ?? false,
                IsName = attribute.IsNameValue ?? false
            };
        }
    }
}
