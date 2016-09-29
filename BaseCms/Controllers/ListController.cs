using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using BaseCms.CRUDRepository;
using BaseCms.CRUDRepository.Core;
using BaseCms.Common;
using BaseCms.Common.Attributes;
using BaseCms.Common.Binders;
using BaseCms.Controllers.Base;
using BaseCms.Model;
using BaseCms.Model.Filters;
using BaseCms.Model.Interfaces;
using BaseCms.Views.List;
using static System.String;

namespace BaseCms.Controllers
{
    public class ListController : CmsControllerBase
    {
        public ListController(QueryResolver queryResolver)
            : base(queryResolver)
        {
        }

        [Authorize]
        public ActionResult Index(string collectionNameWithInitFilters)
        {
            var temp = collectionNameWithInitFilters.Split('|');

            var query = QueryResolver.Execute(new GetTypeQueryBase(), temp[0]);
            var type = query;
            var structureWithCollectionName = new DataWithIdentifier<object, string>(type, null, temp[0],
                                                                                     "/List/Read/?collectionName=" +
                                                                                     temp[0] +
                                                                                     (temp.Length > 1
                                                                                          ? "&filters=" + temp[1]
                                                                                          : Empty));


            var metadata =  query.GetCustomAttributes(typeof(CmsMetadataTypeAttribute), true)
                                                                  .OfType<CmsMetadataTypeAttribute>()
                                                                  .FirstOrDefault();


            return View(!IsNullOrEmpty(metadata?.CustomListView) ? metadata.CustomListView : "Index", structureWithCollectionName);
        }

        [Authorize]
        public ActionResult Read(string collectionName, string filters, string filterContext, string listMetadataProvider, string detailViewGuid, JDataTableSettings jDataTableSettings)
        {
            var filterItems = filters?.Split(',').ToList() ?? new List<string>();

            for (var i = 0; i < filterItems.Count; i++)
            {
                filterItems[i] = filterItems[i].Replace("{comma}", ",");
            }

            return ReadData(collectionName, filterItems, filterContext, listMetadataProvider, detailViewGuid, jDataTableSettings);
        }

        private ActionResult ReadData(string collectionName, List<string> filters, string filterContext, string listMetadataProvider, string detailViewGuid, JDataTableSettings jDataTableSettings)
        {
            var page = jDataTableSettings.DisplayStart / jDataTableSettings.DisplayLength + 1;
            var rows = jDataTableSettings.DisplayLength;

            var collectionType = QueryResolver.Execute(new GetTypeQueryBase(), collectionName);

            var gs = IsNullOrEmpty(listMetadataProvider)
                     ? new DefaultListMetadataProvider()
                     : InstanceCreator.CreateInstance<IMetadataProvider<ListMetadata>>(listMetadataProvider);

            var structure = gs.GetMetadata(collectionType);

            string sortBy = GetSortExpression(structure, jDataTableSettings);

            var collection = QueryResolver.Execute(new GetListQueryBase(page - 1, rows, sortBy, filters, filterContext, detailViewGuid), collectionName);

            var count = QueryResolver.Execute(new GetListCountQueryBase(filters, filterContext, detailViewGuid), collectionName);

            var jsonData1 = new
            {
                jDataTableSettings.Echo,
                iTotalRecords = count,
                iTotalDisplayRecords = count,
                aaData = structure.Serialize(collection)
            };

            return Json(jsonData1, JsonRequestBehavior.AllowGet);
        }

        private string GetSortExpression(ListMetadata structure, JDataTableSettings jDataTableSettings)
        {
            string[] sortColumnNames =
                structure.Items.Where(w => !w.IsHidden)
                         .OrderBy(p => p.Order).Select(metaDataItem => metaDataItem.PropertyName).ToArray();

            var sortExpressionBuilder = new StringBuilder();
            int sortColumnsLength = jDataTableSettings.SortIndexes.Length;
            for (int i = 0; i < sortColumnsLength; i++)
            {
                sortExpressionBuilder.AppendFormat("{0} {1}", sortColumnNames[jDataTableSettings.SortIndexes[i]],
                                                   jDataTableSettings.SortDirections[i]);
                if (i < sortColumnsLength - 1) sortExpressionBuilder.Append(", ");
            }

            return sortExpressionBuilder.ToString();
        }

        public ActionResult CollectionFilter(string id, string collection, string caption, bool isPreloaded, string emptyValueText, string emptyValue, string relatedValue)
        {
            var preloaded = new List<LookupItem>();
            if (isPreloaded)
            {
                var itemsCount = QueryResolver.Execute(new GetListCountQueryBase(), collection);
                preloaded =
                    QueryResolver.Execute(new LookupListQueryBase(null, itemsCount, ""), collection)
                                 .Select(p => (LookupItem)p)
                                 .ToList();
            }
            return View(new CollectionFilterViewModel
            {
                Caption = caption,
                CollectionName = collection,
                Id = id,
                IsPreloaded = isPreloaded,
                PreloadedItems = preloaded,
                EmptyValueText = emptyValueText,
                EmptyValue = emptyValue,
                RelatedId = relatedValue
            });
        }

        public ActionResult StringPropertyFilter(string id, string caption)
        {
            return View(new StringPropertyFilterViewModel
            {
                Caption = caption,
                Id = id
            });
        }

        public ActionResult StringPropertyFilterText(string id, string caption)
        {
            return View(new StringPropertyFilterViewModel
            {
                Caption = caption,
                Id = id
            });
        }

        public ActionResult DateTimePropertyFilter(string id, string caption, string defaultValue)
        {
            return View(new StringPropertyFilterViewModel
                {
                Caption = caption,
                Id = id,
                DefaultValue = defaultValue
            });
        }
        public ActionResult DateRangePropertyFilter(string id, string caption, string defaultValue)
        {
            return View(new StringPropertyFilterViewModel
                {
                Caption = caption,
                Id = id,
                DefaultValue = defaultValue
            });
        }
        public ActionResult CheckBoxPropertyFilter(string id, string caption)
        {
            return View(new StringPropertyFilterViewModel
            {
                Caption = caption,
                Id = id
            });
        }
    }
}
