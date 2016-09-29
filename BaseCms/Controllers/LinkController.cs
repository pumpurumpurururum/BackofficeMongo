using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BaseCms.CRUDRepository;
using BaseCms.CRUDRepository.Core;
using BaseCms.Controllers.Base;
using BaseCms.Model;
using BaseCms.Model.LinkedList;
using BaseCms.Views.List;

namespace BaseCms.Controllers
{
    public class LinkController : CmsControllerBase
    {
        public LinkController(QueryResolver queryResolver)
            : base(queryResolver)
        {
        }

        public ActionResult PreloadedList(string propertyName, string collectionName, string identifier, string filter, string additionalClass, string validate, bool isEditable)
        {
            var type = QueryResolver.Execute(new GetTypeQueryBase(), collectionName);
            var itemsCount = QueryResolver.Execute(new GetListCountQueryBase(), collectionName);
            var model = new LinkedListSingleViewModel(propertyName, collectionName, type, additionalClass: additionalClass);
            var query = new LookupListQueryBase(null, itemsCount, filter);
            model.Preloaded =
                QueryResolver.Execute(query, collectionName)
                             .Select(p => (LookupItem)p)
                             .ToList();
            model.Selected = model.Preloaded.FirstOrDefault(p => p.Id == identifier);
            model.Validate = validate;
            model.IsEditable = isEditable;
            return View(model);
        }

        public ActionResult ServerList(string propertyName, string collectionName, string identifier, string filter, string additionalClass, string validate, bool isEditable)
        {
            var type = QueryResolver.Execute(new GetTypeQueryBase(), collectionName);
            var model = new LinkedListSingleViewModel(propertyName, collectionName, type, filter, additionalClass);
            if (!string.IsNullOrEmpty(identifier))
            {
                model.Selected = (LookupItem)QueryResolver.Execute(new LookupGetItemQueryBase(identifier), collectionName);
            }
            model.Validate = validate;
            model.IsEditable = isEditable;
            return View(model);
        }
        public ActionResult ServerLinkedList(string propertyName, string collectionName, string identifier, string filter, string additionalClass, string validate, bool isEditable, string parentProperty)
        {
            var type = QueryResolver.Execute(new GetTypeQueryBase(), collectionName);
            var model = new LinkedListSingleViewModel(propertyName, collectionName, type, filter, additionalClass, parentProperty);
            if (!string.IsNullOrEmpty(identifier))
            {
                model.Selected = (LookupItem)QueryResolver.Execute(new LookupGetItemQueryBase(identifier), collectionName);
            }
            model.Validate = validate;
            model.IsEditable = isEditable;
            return View(model);
        }
        public ActionResult PreloadedLinkedList(string propertyName, string collectionName, string identifier, string filter, string additionalClass, string validate, bool isEditable)
        {
            var type = QueryResolver.Execute(new GetTypeQueryBase(), collectionName);
            var itemsCount = QueryResolver.Execute(new GetListCountQueryBase(), collectionName);
            var model = new LinkedListSingleViewModel(propertyName, collectionName, type, additionalClass: additionalClass);
            var query = new LookupListQueryBase(null, itemsCount, filter);
            model.Preloaded =
                QueryResolver.Execute(query, collectionName)
                             .Select(p => (LookupItem)p)
                             .ToList();
            model.Selected = model.Preloaded.FirstOrDefault(p => p.Id == identifier);
            model.Validate = validate;
            model.IsEditable = isEditable;
            return View(model);
        }

        public ActionResult GroupBox(string propertyName, string collectionName, string identifier, bool isEditable)
        {
            var type = QueryResolver.Execute(new GetTypeQueryBase(), collectionName);

            var model = new LinkedListMultipleViewModel(propertyName, collectionName, type);
            var count = QueryResolver.Execute(new GetListCountQueryBase(new List<string>()), collectionName);
            model.Preloaded =
                QueryResolver.Execute(new LookupListQueryBase(null, count, ""), collectionName)
                             .Select(p => (LookupItem)p)
                             .ToList();
            if (!string.IsNullOrEmpty(identifier))
            {
                model.Selected =
                    QueryResolver.Execute(new ToManyGetQueryBase(identifier), collectionName)
                                 .Select(p => (LookupItem)p);
            }

            model.IsEditable = isEditable;
            return View(model);
        }

        public ActionResult RadioButtonGroup(string propertyName, string collectionName, string identifier, bool isEditable)
        {
            var type = QueryResolver.Execute(new GetTypeQueryBase(), collectionName);
            var model = new RadioButtonListViewModel(propertyName, collectionName, type);
            var count = QueryResolver.Execute(new GetListCountQueryBase(new List<string>()), collectionName);
            model.Preloaded =
                QueryResolver.Execute(new LookupListQueryBase(null, count, ""), collectionName)
                             .Select(p => (LookupItem)p)
                             .ToList();
            if (!string.IsNullOrEmpty(identifier))
            {
                var asd = QueryResolver.Execute(new ToManyGetQueryBase(identifier), collectionName).Select(p => (LookupItem)p).FirstOrDefault();
                model.Selected = asd != null ? asd.Id : "0";
            }
            model.IsEditable = isEditable;
            return View(model);
        }



        [HttpPost]
        public ActionResult ServerListFetch(string search, string collectionName, string filter)
        {
            var items = QueryResolver.Execute(new LookupListQueryBase(Server.UrlDecode(search), 10, filter), collectionName).Select(p => (LookupItem)p);
            return Json(items.Select(p => new { id = p.Id, text = p.Name }));
        }

        [HttpPost]
        public ActionResult ServerLinkedListFetch(string search, string collectionName, string upperIdentifier, string detailViewGuid, string linkedCollectionName, string filter)
        {
            var items =
                QueryResolver.Execute(new GetLinkedListQueryBase(Server.UrlDecode(search), 10, upperIdentifier, detailViewGuid, filter), collectionName).Select(p => (LookupItem)p);
            return Json(items.Select(p => new { id = p.Id, text = p.Name }));
        }

        public ActionResult ToManyRead(string collectionName, string identifier, string sidx, string sord, int page, int rows)
        {
            var collectionType = QueryResolver.Execute(new GetTypeQueryBase(), collectionName);
            var collection = QueryResolver.Execute(new ToManyGetQueryBase(identifier), collectionName);
            var gs = new DefaultListMetadataProvider();
            var structure = gs.GetMetadata(collectionType);
            var count = QueryResolver.Execute(new GetListCountQueryBase(), collectionName);
            var totalCount = count % rows == 0
                                 ? count / rows
                                 : count / rows + 1;
            var jsonData1 = new
            {
                total = totalCount,
                page,
                records = rows,
                rows = structure.Serialize(collection)
            };
            return Json(jsonData1, JsonRequestBehavior.AllowGet);
        }

        public object[] SerializeRowsForGrid(IEnumerable<object> objects, ListMetadata listMetadata)
        {
            return null;
        }

        public ActionResult PreloadedListMultiple(string propertyName, string collectionName, string identifier, string filter, string additionalClass)
        {
            var type = QueryResolver.Execute(new GetTypeQueryBase(), collectionName);
            var model = new LinkedListMultipleViewModel(propertyName, collectionName, type);
            var count = QueryResolver.Execute(new GetListCountQueryBase(new List<string>()), collectionName);
            model.Preloaded =
                QueryResolver.Execute(new LookupListQueryBase(null, count, ""), collectionName)
                             .Select(p => (LookupItem)p)
                             .ToList();
            if (!String.IsNullOrEmpty(identifier))
            {
                model.Selected =
                    QueryResolver.Execute(new ToManyGetQueryBase(identifier), collectionName)
                                 .Select(p => (LookupItem)p);
            }
            return View(model);
        }

        public ActionResult ServerListMultiple(string propertyName, string collectionName, string identifier, string filter, string additionalClass)
        {
            var type = QueryResolver.Execute(new GetTypeQueryBase(), collectionName);
            var model = new LinkedListMultipleViewModel(propertyName, collectionName, type);
            if (!String.IsNullOrEmpty(identifier))
            {
                model.Selected =
                    QueryResolver.Execute(new ToManyGetQueryBase(identifier), collectionName)
                                 .Select(p => (LookupItem)p);
            }
            return View(model);
        }

        public ActionResult PreloadedListWithAddData(string propertyName, string collectionName, string identifier, string filter, string additionalClass, string validation, bool isEditable)
        {
            var type = QueryResolver.Execute(new GetTypeQueryBase(), collectionName);
            var itemsCount = QueryResolver.Execute(new GetListCountQueryBase(), collectionName);
            var model = new LinkedListWithAddDataSingleViewModel(propertyName, collectionName, type, additionalClass: additionalClass, validation: validation)
                {
                    Preloaded =
                        QueryResolver.Execute(new LookupListWithAddDataQueryBase(null, itemsCount, filter),
                                              collectionName)
                                     .Select(p => (LookupItemWithAddData) p)
                                     .ToList()
                };
            model.Selected = model.Preloaded.FirstOrDefault(p => p.Id == identifier);
            model.IsEditable = isEditable;
            return View(model);
        }
    }
}
