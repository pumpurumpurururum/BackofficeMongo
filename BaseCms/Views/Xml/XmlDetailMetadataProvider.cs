using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using BaseCms.Common.Validation.Attributes.Base;
using BaseCms.Views.Detail;
using BaseCms.Views.Detail.Attributes;
using BaseCms.Views.Detail.Interfaces;

namespace BaseCms.Views.Xml
{
    public class XmlDetailMetadataProvider
    {
        public IDetailMetadata GetMetadata(Type metadataType, Dictionary<string, string> properties, string dictPropName)
        {
            if (properties == null)
            {
                return new DetailMetadata
                {
                    Items = new DetailMetadataItem[0]
                };
            }

            var props = metadataType == null
                            ? new PropertyInfo[0]
                            : metadataType.GetProperties(BindingFlags.Instance | BindingFlags.Public |
                                                         BindingFlags.GetProperty |
                                                         BindingFlags.SetProperty);

            //Выводим колонки для которых переопределен ToString()
            Func<object, string, object> getValueByName = (obj, propName) =>
            {
                try
                {
                    var dictionary = obj as Dictionary<string, string>;
                    if (dictionary != null)
                    {
                        var result = dictionary[propName];
                        return result;
                    }
                }
                catch
                {
                    //Мы не смогли прочитать значение. оставляем его пустым
                }
                return null;
            };

            var metadataItems = properties.Select(p => CreateMetadata(p, props.FirstOrDefault(x => x.Name == p.Key), getValueByName, dictPropName)).ToList();


            return new DetailMetadata
            {
                Items = metadataItems
            };
        }

        private DetailMetadataItem CreateMetadata(KeyValuePair<string, string> propertyInfo, PropertyInfo overridedPropertyInfo,
                                                  Func<object, string, object> getValueByName, string dictPropName)
        {
            var detailAttr = overridedPropertyInfo == null ? new DetailMetadataAttribute() : overridedPropertyInfo.GetCustomAttribute<DetailMetadataAttribute>();

            var result = new DetailMetadataItem((info, o, value, resolver) => info.SetValue(o, value))
            {
                Label = String.IsNullOrEmpty(detailAttr.Display) ? propertyInfo.Key : detailAttr.Display,
                PropertyName = "xml_" + dictPropName + "_" + propertyInfo.Key,
                GetValue = o => getValueByName(o, propertyInfo.Key),
                ItemType = GetTemplateForProperty(propertyInfo, detailAttr),
                IsEditable = detailAttr.IsEditableValue ?? true,
                //IsKey = detailAttr.IsKeyValue ?? false,
                //IsRequired = detailAttr.IsRequiredValue ?? false,
                //IsHidden =
                //    detailAttr.IsHiddenValue ??
                //    !propertyInfo.PropertyType.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                //                 .Any(
                //                     a =>
                //                     a.Name == "ToString" && !a.GetParameters().Any() &&
                //                     a.DeclaringType != typeof(object)),
                Format = detailAttr.Format,
                Order = detailAttr.OrderValue ?? 0,
                MaxLength = detailAttr.MaxLength
            };

            result.ItemType.Template = detailAttr.Template ?? result.ItemType.Template;

            // Validation attributes...
            if (overridedPropertyInfo != null)
            {
                var validationAttributes = overridedPropertyInfo.GetCustomAttributes<ValidationAttribute>().ToList();

                foreach (var item in validationAttributes)
                {
                    result.ValidationAttributes.Add(item);
                }
            }

            return result;
        }

        private ITypeItem GetTemplateForProperty(KeyValuePair<string, string> propertyInfo, DetailMetadataAttribute dtmattr)
        {
            var propertyType = Type.GetType(propertyInfo.Value);

            var typeItem = (_typeList.FirstOrDefault(w => w.Type == propertyType) ??
                            _typeList.FirstOrDefault(w => propertyType != null && propertyType.GetInterfaces().Any(p => p == w.Type))) ??
                           new TypeItem { ParseMethod = c => c, Type = propertyType };
            if (typeItem.Type == typeof(String) && dtmattr.IsPasswordValue == true)
            {
                typeItem.Template = "Password";
            }
            return (ITypeItem)typeItem.Clone();
        }

        private readonly List<TypeItem> _typeList = new List<TypeItem>
                {
                    new TypeItem { Type = typeof(String),   ParseMethod = s => s , Template = "String"},
                    new TypeItem { Type = typeof(Guid),   ParseMethod = s => s , Template = "String"},
                    new TypeItem { Type = typeof(Byte),     ParseMethod = s => Byte.Parse(s), Template = "String" },
                    new TypeItem { Type = typeof(Int16),    ParseMethod = s => Int16.Parse(s), Template = "String" },
                    new TypeItem { Type = typeof(Int32),    ParseMethod = s => Int32.Parse(s), Template = "String" },
                    new TypeItem { Type = typeof(Int64),    ParseMethod = s => Int64.Parse(s), Template = "String" },
                    new TypeItem { Type = typeof(Single),   ParseMethod = s => Single.Parse(s.Replace(".", ",")), Template = "String" },
                    new TypeItem { Type = typeof(Double),   ParseMethod = s => Double.Parse(s.Replace(".", ",")), Template = "String" },
                    new TypeItem { Type = typeof(Decimal),  ParseMethod = s => Decimal.Parse(s), Template = "String" },
                    new TypeItem { Type = typeof(Boolean),  ParseMethod = s =>
                        {
                            string booleanVal = s.ToLowerInvariant();
                            if ((booleanVal == "true,false") || (booleanVal.Equals("on,off"))) return true;
                            return false;
                        }, Template = "Boolean" },
                    new TypeItem { Type = typeof(DateTime), ParseMethod = s => DateTime.Parse(s), Template = "DateTime" },
                    new TypeItem { Type = typeof(Guid?),   ParseMethod = s => s , Template = "String"},
                    new TypeItem { Type = typeof(Byte?),    ParseMethod = s => !String.IsNullOrEmpty(s) ? (Byte?)Byte.Parse(s) : null, Template = "String" },
                    new TypeItem { Type = typeof(Int16?),   ParseMethod = s => !String.IsNullOrEmpty(s) ? (Int16?)Int16.Parse(s) : null, Template = "String" },
                    new TypeItem { Type = typeof(Int32?),   ParseMethod = s => !String.IsNullOrEmpty(s) ? (Int32?)Int32.Parse(s) : null, Template = "String" },
                    new TypeItem { Type = typeof(Int64?),   ParseMethod = s => !String.IsNullOrEmpty(s) ? (Int64?)Int64.Parse(s) : null, Template = "String" },
                    new TypeItem { Type = typeof(Single?),  ParseMethod = s => !String.IsNullOrEmpty(s) ? (Single?)Single.Parse(s.Replace(".", ",")) : null, Template = "String"},
                    new TypeItem { Type = typeof(Double?),  ParseMethod = 
                        s =>
                            {
                                if (string.IsNullOrEmpty(s)) return null;

                                double value;

                                if (double.TryParse(s, out value)) return value;

                                var separator = CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator;

                                if (!separator.Equals(",")) s = s.Replace(",", separator);
                                if (!separator.Equals(".")) s = s.Replace(".", separator);
                                
                                return double.Parse(s);
                            }, Template = "String"},
                    new TypeItem { Type = typeof(Decimal?), ParseMethod = s => !String.IsNullOrEmpty(s) ? (Decimal?)Decimal.Parse(s) : null, Template = "String" },
                    new TypeItem { Type = typeof(Boolean?), ParseMethod = s =>
                        {
                            if (s == null) return null;
                            string booleanVal = s.ToLowerInvariant();
                            if ((booleanVal == "true,false") || (booleanVal.Equals("on,off"))) return true;
                            return false;
                        }, Template = "Boolean" }, 
                    new TypeItem { Type = typeof(DateTime?),ParseMethod = s => !String.IsNullOrEmpty(s) ? (DateTime?)DateTime.Parse(s) : null, Template = "DateTime" },
                    new TypeItem { Type = typeof(IEnumerable),ParseMethod = s => s, Template = "CheckBoxGroup" }
                };
    }
}
