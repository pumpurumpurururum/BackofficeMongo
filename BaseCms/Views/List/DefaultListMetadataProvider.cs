using System;
using System.Collections.Generic;

using System.Linq;
using System.Reflection;
using System.Web;

using BaseCms.Common.Attributes;
using BaseCms.Model.Interfaces;
using BaseCms.Views.Detail.Attributes;
using BaseCms.Views.List.Attributes;

namespace BaseCms.Views.List
{
    public class DefaultListMetadataProvider : MetadataProviderBase, IMetadataProvider<ListMetadata>
    {
        const string IdentityPropertyName = "Id";
        
        public ListMetadata GetMetadata(Type type)
        {
            PropertyInfo[] overridedProperties;
            var overridedObjectTypeAttribute = type.GetCustomAttribute<CmsMetadataTypeAttribute>(true);
            if (overridedObjectTypeAttribute != null)
            {
                var overridedType = overridedObjectTypeAttribute.Type;
                overridedProperties =
                    overridedType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty |
                                                BindingFlags.SetProperty);
            }
            else throw new Exception("Невозможно получить метаданные коллекции " + type.Name);

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

            var keyItem = type.GetProperties().Where(p => p.Name == IdentityPropertyName)
                .Select(
                    s =>
                        CreateMetadata(s, overridedProperties.FirstOrDefault(p => p.Name == s.Name),
                            getStringValueByName)).FirstOrDefault() ?? overridedProperties.Where(x =>
                            {
                                var detailMetadata = x.GetCustomAttribute<DetailMetadataAttribute>();

                                return (detailMetadata != null) && detailMetadata.IsKey;
                            }).Select(s => CreateMetadata(s, overridedProperties.FirstOrDefault(p => p.Name == s.Name),
                                getStringValueByName)).FirstOrDefault();

            if (keyItem == null) throw new Exception(String.Format("У коллекции {0} не распознано ключевое поле", type.Name));

            keyItem.IsHidden = false;

            //Выводим колонки для которых переопределен ToString()
            var items =
                type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty |
                                   BindingFlags.SetProperty)
                    .Select(s => CreateMetadata(s, overridedProperties.FirstOrDefault(p => p.Name == s.Name), getStringValueByName))
                    .ToList();

            items.Insert(0, keyItem);
            //string json = JsonConvert.SerializeObject(items);
            Func<object, object[]> getCollectionValues = obj => items.Where(w => !w.IsHidden).OrderBy(p => p.Order).Select(s => s.GetValue(obj)).Concat(new object[] { obj.GetType().ToString() }).ToArray();

            Func<IEnumerable<object>, object[]> serializer = objs => objs.Select(s => getCollectionValues(s)).ToArray();

            return new ListMetadata(serializer, items);
        }

        private ListMetadataItem CreateMetadata(PropertyInfo propertyInfo, PropertyInfo overridedPropertyInfo,
                                                Func<object, string, string, string> getStringValueByName)
        {
            //"Наложение аттрибутов, если их несколько на одном свойстве

            var attribute = AggregateAttributes(
                propertyInfo,
                overridedPropertyInfo,
                (a, b) => new ListMetadataAttribute
                {
                    Display = b.Display ?? a.Display,
                    IsHiddenValue = b.IsHiddenValue ?? a.IsHiddenValue,
                    WidthValue = b.WidthValue ?? b.WidthValue,
                    Format = b.Format ?? a.Format,
                    OrderValue = b.OrderValue ?? a.OrderValue
                },
                () => new ListMetadataAttribute { IsHidden = true });


            return new ListMetadataItem
            {
                GetValue = o => getStringValueByName(o, propertyInfo.Name, attribute.Format),
                PropertyName = propertyInfo.Name,
                IsHidden =
                    attribute.IsHiddenValue ??
                    !propertyInfo.PropertyType.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                                 .Any(
                                     a =>
                                     a.Name == "ToString" && !a.GetParameters().Any() &&
                                     a.DeclaringType != typeof(object)),
                Display = attribute.Display ?? propertyInfo.Name,
                Width = attribute.WidthValue,
                Format = attribute.Format,
                Template = attribute.Template,
                Order = attribute.OrderValue ?? 0,
                InitSort = attribute.InitSortValue,
                InitSortDir = attribute.InitSortDirValue,
                NoSort = attribute.NoSort,
                ColumnClass = attribute.ColumnClass,
                IsGroupColumn = attribute.IsGroupColumn
            };
        }
    }
}
