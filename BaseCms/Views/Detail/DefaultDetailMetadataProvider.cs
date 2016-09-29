using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using BaseCms.CRUDRepository;
using BaseCms.CRUDRepository.Core;
using BaseCms.Common.Attributes;
using BaseCms.Common.Image.Attributes;
using BaseCms.Common.Validation.Attributes.Base;
using BaseCms.Model.Interfaces;
using BaseCms.Views.Detail.Attributes;
using BaseCms.Views.Detail.Enums;
using BaseCms.Views.Detail.Interfaces;
using BaseCms.Views.Link.Attributes;
using BaseCms.Views.List;
using BaseCms.Views.List.Attributes;
using BaseCms.Views.List.ListViewMetadata;
using BaseCms.Views.List.ListViewMetadata.Attributes;
using BaseCms.Views.Xml;
using BaseCms.Views.Xml.Attributes;
using static System.String;

namespace BaseCms.Views.Detail
{
    public class DefaultDetailMetadataProvider : MetadataProviderBase, IMetadataProvider<IDetailMetadata>
    {
        public bool IsHandler(string collectionName)
        {
            return true;
        }

        public IDetailMetadata GetMetadata(Type type)
        {
            var overridedProperties = new PropertyInfo[0];
            var forbidReadMode = false;
            var customEditView = Empty;
            var customReadView = Empty;
            var listViewHeaderPlural = Empty;
            var tabsWithBlocks = Empty;
            var overridedObjectTypeAttribute = type.GetCustomAttribute<CmsMetadataTypeAttribute>(true);
            if (overridedObjectTypeAttribute != null)
            {
                var overridedType = overridedObjectTypeAttribute.Type;
                overridedProperties =
                    overridedType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty |
                                                BindingFlags.SetProperty);

                customEditView = overridedObjectTypeAttribute.CustomEditView;
                forbidReadMode = overridedObjectTypeAttribute.ForbidReadMode;
                listViewHeaderPlural = overridedObjectTypeAttribute.CollectionTitlePlural;
                tabsWithBlocks = overridedObjectTypeAttribute.TabsWithBlocks;
            }

            //Выводим колонки для которых переопределен ToString()
            Func<object, string, object> getValueByName = (obj, propName) =>
            {
                if (propName.StartsWith("Tech_"))
                {
                    return null;
                }
                var prop = type.GetProperty(propName);
                try
                {
                    var result = prop.GetValue(obj);
                    return result;
                }
                catch
                {
                    //Мы не смогли прочитать значение. оставляем его пустым
                }
                return null;
            };
            var originalProperies =
                type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty |
                                   BindingFlags.SetProperty);
            var rows = overridedProperties.Select(s =>
            {
                var origProp = originalProperies.FirstOrDefault(p => p.Name == s.Name) ?? s;
                var meta =  CreateMetadata(origProp, s, getValueByName);
                return meta;

            }).ToList();
            //var rows = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty |
            //                       BindingFlags.SetProperty)
            //        .Select(s => CreateMetadata(s, overridedProperties.FirstOrDefault(p => p.Name == s.Name), getValueByName))
            //        .ToList();

            return new DetailMetadata
            {
                Items = rows,
                ForbidReadMode = forbidReadMode,
                CustomEditView = customEditView,
                CustomReadView = customReadView,
                ListViewHeaderPlural = listViewHeaderPlural,
                TabsWithBlocks = tabsWithBlocks
            };
        }

        private DetailMetadataItem CreateMetadata(PropertyInfo propertyInfo, PropertyInfo overridedPropertyInfo,
                                                  Func<object, string, object> getValueByName)
        {
            //"Наложение аттрибутов, если их несколько на одном свойстве
            var detailMetadataAttr = GetDetailMetadata(propertyInfo, overridedPropertyInfo);
            var liknMetaData = GetLinkMetadata(propertyInfo, overridedPropertyInfo);
            Action<PropertyInfo, object, object, QueryResolver> setter;
            if (liknMetaData != null && liknMetaData.Multiplier == LinkMultiplier.ToMany && !IsNullOrEmpty(liknMetaData.CollectionName))
            {
                setter = (s, obj, value, resolver) =>
                    {
                        var id = resolver.Execute(new GetIdentifierQueryBase(obj), liknMetaData.CollectionName);

                        resolver.Execute(new ToManySave(id, ((string)value ?? Empty).Split(',').Where(p => !IsNullOrEmpty(p)).ToArray()), liknMetaData.CollectionName);
                    };
            }
            else
            {
                setter = (s, obj, value, resolver) =>
                {
                    if (s.PropertyType == typeof(Guid))
                    {
                        s.SetValue(obj, Guid.Parse(value.ToString()));
                        return;
                    }
                    s.SetValue(obj, value);
                };
            }
            var result = new DetailMetadataItem(setter)
            {
                Label = detailMetadataAttr.Display ?? propertyInfo.Name,
                PropertyName = propertyInfo.Name,
                GetValue = o => getValueByName(o, propertyInfo.Name),
                ItemType = GetTemplateForProperty(propertyInfo, detailMetadataAttr),
                IsEditable = detailMetadataAttr.IsEditableValue ?? true,
                IsKey = detailMetadataAttr.IsKeyValue ?? false,
                IsRequired = detailMetadataAttr.IsRequiredValue ?? false,
                IsHidden =
                    detailMetadataAttr.IsHiddenValue ??
                    !propertyInfo.PropertyType.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                                 .Any(
                                     a =>
                                     a.Name == "ToString" && !a.GetParameters().Any() &&
                                     a.DeclaringType != typeof(object)),
                Format = detailMetadataAttr.Format,
                LinkMetadata = liknMetaData,
                Order = detailMetadataAttr.OrderValue ?? 0,
                IsHeaderDisplayProperty = detailMetadataAttr.IsHeaderDisplayProperty,
                MaxLength = detailMetadataAttr.MaxLength,
                Tab = detailMetadataAttr.Tab,
                Block = detailMetadataAttr.Block == 0 ? 1 : detailMetadataAttr.Block
            };

            result.ItemType.Template = detailMetadataAttr.Template ?? result.ItemType.Template;

            // Validation attributes...
            var validationAttributes = propertyInfo.GetCustomAttributes<ValidationAttribute>().ToList();
            if (overridedPropertyInfo != null)
            {
                var overridedValidationAttributes =
                    overridedPropertyInfo.GetCustomAttributes<ValidationAttribute>().ToList();
                foreach (var item in validationAttributes)
                {
                    var overAtr = overridedValidationAttributes.FirstOrDefault(p => p.GetType() == item.GetType());
                    result.ValidationAttributes.Add(overAtr ?? item);
                }
                foreach (var item in overridedValidationAttributes.Where(item => !result.ValidationAttributes.Contains(item)))
                {
                    result.ValidationAttributes.Add(item);
                }
            }
            else
            {
                foreach (var item in validationAttributes)
                {
                    result.ValidationAttributes.Add(item);
                }
            }

            // ImageAttributes...
            result.ImageMetadata = GetImageMetadata(overridedPropertyInfo);

            // ListViewAttributes
            result.ListViewMetadata = GetListViewMetadata(overridedPropertyInfo);

            // HtmlEditorAttributes
            result.HtmlEditorMetadata = GetHtmlEditorMetadata(overridedPropertyInfo);

            // YandexAttributes
            //result.YandexMetadata = GetYandexMetadata(overridedPropertyInfo);

            // AutoTypographMetadata
            result.AutoTypographMetadata = GetAutoTypographMetadata(overridedPropertyInfo);

            // XmlEditorMetadata
            result.XmlEditorMetadata = GetXmlEditorMetadata(overridedPropertyInfo);

            // EditableCollectionMetadata
            result.EditableCollectionMetadata = GetEditableCollectionMetadata(overridedPropertyInfo);

            // Popup
            result.PopupMetadata = GetPopupMetadata(overridedPropertyInfo);

            return result;
        }

        private LinkMetadata GetLinkMetadata(PropertyInfo propertyInfo, PropertyInfo overridedPropertyInfo)
        {
            var aggregatedMetaData = AggregateAttributes(propertyInfo, overridedPropertyInfo, (a, b) =>
                (a == null || b == null) ? null : new DynamicLinkAttribute(a.TargetCollection ?? b.TargetCollection),
                () => (DynamicLinkAttribute)null);
            var groupMetaData = AggregateAttributes(propertyInfo, overridedPropertyInfo, (a, b) =>
                (a == null || b == null) ? null : new LinkedListsAttribute(a.GroupName ?? b.GroupName, a.RelatedList ?? b.RelatedList),
                () => (LinkedListsAttribute)null);
            if (aggregatedMetaData != null)
            {
                var ret = new LinkMetadata
                    {
                    CollectionName = aggregatedMetaData.TargetCollection,
                    DetailViewSave = aggregatedMetaData.DetailViewSave,
                    IsPreloaded = aggregatedMetaData.IsPreloaded,
                    Multiplier = propertyInfo.PropertyType != typeof(String) && propertyInfo.PropertyType.GetInterfaces().Intersect(new[] { typeof(IEnumerable), typeof(IEnumerable<>) }).Any() ? LinkMultiplier.ToMany : LinkMultiplier.ToOne,
                };
                if (groupMetaData != null)
                {
                    ret.GroupName = groupMetaData.GroupName;
                    ret.RelatedList = groupMetaData.RelatedList;
                }
                return ret;
            }
            return null;
        }

        private DetailMetadataAttribute GetDetailMetadata(PropertyInfo propertyInfo, PropertyInfo overridedPropertyInfo)
        {
            return AggregateAttributes(propertyInfo, overridedPropertyInfo, (a, b) => 
                new DetailMetadataAttribute
                    {
                    Display = b.Display ?? a.Display,
                    IsEditableValue = b.IsEditableValue ?? a.IsEditableValue,
                    IsHiddenValue = b.IsHiddenValue ?? a.IsHiddenValue,
                    IsKeyValue = b.IsKeyValue ?? a.IsKeyValue,
                    IsRequiredValue = b.IsRequiredValue ?? a.IsRequiredValue,
                    OrderValue = b.OrderValue ?? a.OrderValue,
                    Template = b.Template ?? a.Template,
                    Format = b.Format ?? a.Format,
                    IsPasswordValue = b.IsPasswordValue ?? a.IsPasswordValue,
                    Tab = b.Tab > a.Tab ? b.Tab : a.Tab
                },
                () => new DetailMetadataAttribute()
            );
        }

        private static ImageMetadata GetImageMetadata(PropertyInfo overridedPropertyInfo)
        {
            if (overridedPropertyInfo != null)
            {
                var imageAttributes = overridedPropertyInfo.GetCustomAttributes<ImageMetadataAttribute>().ToArray();
                if (imageAttributes.Any())
                {
                    var attr = imageAttributes.First();
                    return new ImageMetadata
                    {
                        Width = attr.Width,
                        Height = attr.Height,
                        FreeSize = attr.FreeSize,
                        PoolName = attr.PoolName,
                        FileSuffix = attr.FileSuffix
                    };
                }
            }
            return null;
        }

        private static ListViewMetadata GetListViewMetadata(PropertyInfo overridedPropertyInfo)
        {
            ListViewMetadataAttribute attr = null;
            ListViewGroupingMetadataAttribute grAttr = null;
            if (overridedPropertyInfo != null)
            {
                var listViewAttributes = overridedPropertyInfo.GetCustomAttributes<ListViewMetadataAttribute>().ToList();
                if (listViewAttributes.Any())
                {
                    attr = listViewAttributes.First();
                }
                grAttr =
                    overridedPropertyInfo.GetCustomAttributes<ListViewGroupingMetadataAttribute>().FirstOrDefault();
            }

            if (attr == null) attr = new ListViewMetadataAttribute();

            return new ListViewMetadata
            {
                Buttons = attr.Buttons,
                CustomButtonsContent = attr.CustomButtonsContent,
                CustomButtonsWidth = attr.CustomButtonsWidth,
                FilterPartial = attr.FilterPartial,
                FooterPartial = attr.FooterPartial,
                JDataTableDom = attr.JDataTableDom,
                JScriptFuctionPartial = attr.JScriptFuctionPartial,
                LinkedCollectionType = attr.LinkedCollectionType,
                ListMetadataProviderType = attr.ListMetadataProviderType,
                Paginate = attr.Paginate,
                PopupMetadataProviderType = attr.PopupMetadataProviderType,
                UploadImagesUrl = attr.UploadImagesUrl,
                UploadFileUrl = attr.UploadFileUrl,
                ViewName = attr.ViewName,
                GroupingMetadata = grAttr == null ? null : new ListViewGroupingMetadata
                {
                    ExpandableGrouping = grAttr.ExpandableGrouping,
                    HideGroupingColumn = grAttr.HideGroupingColumn
                }
            };
        }

        private static HtmlEditorMetadata GetHtmlEditorMetadata(PropertyInfo overridedPropertyInfo)
        {
            if (overridedPropertyInfo != null)
            {
                var htmlEditorAttributes = overridedPropertyInfo.GetCustomAttributes<HtmlEditorMetadataAttribute>().ToList();
                if (htmlEditorAttributes.Any())
                {
                    var attr = htmlEditorAttributes.First();
                    return new HtmlEditorMetadata
                    {
                        CssFromConfig = attr.CssFromConfig,
                        Css = attr.Css,
                        CssSelectorsForRemoveFormat = attr.CssSelectorsForRemoveFormat,
                        ImagesUrlFromConfig = attr.ImagesUrlFromConfig,
                        ImagesUrl = attr.ImagesUrl
                    };
                }
            }

            return null;
        }

        //private static YandexMetadata GetYandexMetadata(PropertyInfo overridedPropertyInfo)
        //{
        //    if (overridedPropertyInfo != null)
        //    {
        //        var yandexAttributes = overridedPropertyInfo.GetCustomAttributes<YandexMetadataAttribute>().ToList();
        //        if (yandexAttributes.Any())
        //        {
        //            var attr = yandexAttributes.First();
        //            return new YandexMetadata
        //            {
        //                UseWebmaster = attr.UseWebmaster
        //            };
        //        }
        //    }

        //    return null;
        //}

        private static AutoTypographMetadata GetAutoTypographMetadata(PropertyInfo overridedPropertyInfo)
        {
            if (overridedPropertyInfo != null)
            {
                var autoTypographAttributes = overridedPropertyInfo.GetCustomAttributes<AutoTypographMetadataAttribute>().ToList();
                if (autoTypographAttributes.Any())
                {
                    var attr = autoTypographAttributes.First();
                    return new AutoTypographMetadata
                    {
                        AutoTypographHelperType = attr.AutoTypographHelperType
                    };
                }
            }

            return null;
        }

        private static XmlEditorMetadata GetXmlEditorMetadata(PropertyInfo overridedPropertyInfo)
        {
            XmlEditorMetadataAttribute attr = null;
            if (overridedPropertyInfo != null)
            {
                var xmlEditorAttributes = overridedPropertyInfo.GetCustomAttributes<XmlEditorMetadataAttribute>().ToList();
                if (xmlEditorAttributes.Any())
                {
                    attr = xmlEditorAttributes.First();
                }
            }

            if (attr == null) attr = new XmlEditorMetadataAttribute();

            return new XmlEditorMetadata
            {
                ViewName = attr.ViewName,
                XmlMetadataCollectionType = attr.XmlMetadataCollectionType,
                XmlMetadataPropertyName = attr.XmlMetadataPropertyName,
                XmlMetadataValueProviderType = attr.XmlMetadataValueProviderType
            };
        }

        private static EditableCollectionMetadata GetEditableCollectionMetadata(PropertyInfo overridedPropertyInfo)
        {
            EditableCollectionMetadataAttribute attr = null;
            if (overridedPropertyInfo != null)
            {
                var xmlEditorAttributes = overridedPropertyInfo.GetCustomAttributes<EditableCollectionMetadataAttribute>().ToList();
                if (xmlEditorAttributes.Any())
                {
                    attr = xmlEditorAttributes.First();
                }
            }

            if (attr == null) attr = new EditableCollectionMetadataAttribute();

            return new EditableCollectionMetadata
            {
                ViewName = attr.ViewName
            };
        }

        private static PopupMetadata GetPopupMetadata(PropertyInfo overridedPropertyInfo)
        {
            PopupMetadataAttribute attr = null;
            if (overridedPropertyInfo != null)
            {
                var xmlEditorAttributes = overridedPropertyInfo.GetCustomAttributes<PopupMetadataAttribute>().ToList();
                if (xmlEditorAttributes.Any())
                {
                    attr = xmlEditorAttributes.First();
                }
            }

            if (attr == null) attr = new PopupMetadataAttribute();

            return new PopupMetadata
            {
                ViewName = attr.ViewName,
                Title = attr.Title,
                Height = attr.Height,
                Width = attr.Width,
                NoScroll = attr.NoScroll,
                Top = attr.Top,
                Left = attr.Left
            };
        }

        private ITypeItem GetTemplateForProperty(PropertyInfo propertyInfo, DetailMetadataAttribute dtmattr)
        {
            var typeItem = (_typeList.FirstOrDefault(w => w.Type == propertyInfo.PropertyType) ??
                            _typeList.FirstOrDefault(w => propertyInfo.PropertyType.GetInterfaces().Any(p => p == w.Type))) ??
                           new TypeItem { ParseMethod = c => c, Type = propertyInfo.PropertyType };
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
                    new TypeItem { Type = typeof(Int16),    ParseMethod = s => Int16.Parse(s), Template = "Int" },
                    new TypeItem { Type = typeof(Int32),    ParseMethod = s => Int32.Parse(s), Template = "Int" },
                   
                    //new TypeItem { Type = typeof(Decimal),    ParseMethod = s => Decimal.Parse(s, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture), Template = "Decimal" },
                    new TypeItem { Type = typeof(Single),   ParseMethod = s => Single.Parse(s.Replace(".", ",")), Template = "String" },
                    new TypeItem { Type = typeof(Double),   ParseMethod = s => Double.Parse(s.Replace(".", ",")), Template = "String" },
                    new TypeItem { Type = typeof(TimeSpan),   ParseMethod = s =>
                    {
                        DateTime t = DateTime.ParseExact(s, "h:mm tt", CultureInfo.InvariantCulture); 
                        TimeSpan ts = t.TimeOfDay;
                        return ts;
                    }, Template = "Time" },
                    new TypeItem { Type = typeof(Decimal),  ParseMethod =
                        s =>
                        {
                            //var ret = s.Replace(".", ",").Replace(" ", "");
                            //return Decimal.Parse(ret);
                            decimal value;
                            var ss = s.Replace(" ", "");
                            var separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                            if (!separator.Equals(",")) ss = ss.Replace(",", separator);
                            if (!separator.Equals(".")) ss = ss.Replace(".", separator);

                            return decimal.TryParse(ss, out value) ? value : decimal.Parse(ss);
                        }, Template = "Decimal" },
                    
                    new TypeItem { Type = typeof(Boolean),  ParseMethod = s =>
                        {
                            string booleanVal = s.ToLowerInvariant();
                            if ((booleanVal == "true,false") || (booleanVal.Equals("on,off"))) return true;
                            return false;
                        }, Template = "Boolean" },
                    new TypeItem { Type = typeof(DateTime), ParseMethod = s => DateTime.Parse(s), Template = "DateTime" },
                    new TypeItem { Type = typeof(Guid?),   ParseMethod = s => s , Template = "String"},
                    new TypeItem { Type = typeof(Byte?),    ParseMethod = s => !IsNullOrEmpty(s) ? (Byte?)Byte.Parse(s) : null, Template = "String" },
                    new TypeItem { Type = typeof(Int16?),   ParseMethod = s => !IsNullOrEmpty(s) ? (Int16?)Int16.Parse(s) : null, Template = "Int" },
                    new TypeItem { Type = typeof(Int32?),   ParseMethod = s => !IsNullOrEmpty(s) ? (Int32?)Int32.Parse(s) : null, Template = "Int" },
                    new TypeItem { Type = typeof(Int64?),   ParseMethod = s => !IsNullOrEmpty(s) ? (Int64?)Int64.Parse(s) : null, Template = "Int" },
                    new TypeItem { Type = typeof(Single?),  ParseMethod = s => !IsNullOrEmpty(s) ? (Single?)Single.Parse(s.Replace(".", ",")) : null, Template = "String"},
                    new TypeItem { Type = typeof(Double?),  ParseMethod = 
                        s =>
                            {
                                if (IsNullOrEmpty(s)) return null;

                                double value;

                                if (double.TryParse(s, out value)) return value;

                                var separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

                                if (!separator.Equals(",")) s = s.Replace(",", separator);
                                if (!separator.Equals(".")) s = s.Replace(".", separator);
                                
                                return double.Parse(s);
                            }, Template = "String"},
                    new TypeItem { Type = typeof(Decimal?),  ParseMethod = 
                        s =>
                            {
                                if (IsNullOrEmpty(s)) return null;

                                decimal value;
                                var ss = s.Replace(" ", "");
                                var separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                                if (!separator.Equals(",")) ss = ss.Replace(",", separator);
                                if (!separator.Equals(".")) ss = ss.Replace(".", separator);
                            
                                if (decimal.TryParse(ss, out value)) return value;

                                return decimal.Parse(ss);
                            }, Template = "Decimal"},
                    new TypeItem { Type = typeof(Boolean?), ParseMethod = s =>
                        {
                            if (s == null) return null;
                            string booleanVal = s.ToLowerInvariant();
                            if ((booleanVal == "true") || (booleanVal == "true,false") || (booleanVal.Equals("on,off"))) return true;
                            return false;
                        }, Template = "Boolean" }, 
                    new TypeItem { Type = typeof(DateTime?),ParseMethod = s => !IsNullOrEmpty(s) ? (DateTime?)DateTime.Parse(s) : null, Template = "DateTime" },
                    new TypeItem { Type = typeof(IEnumerable),ParseMethod = s => s, Template = "CheckBoxGroup" }
                };
    }
}
