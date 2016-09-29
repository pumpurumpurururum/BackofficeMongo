using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BaseCms.CRUDRepository;
using BaseCms.CRUDRepository.Core;
using BaseCms.Common;
using BaseCms.Controllers.Base;
using BaseCms.Model;
using BaseCms.Model.Interfaces;
using BaseCms.Views.Tree;

namespace BaseCms.Controllers
{
    public class TreeController : CmsControllerBase
    {
        //
        // GET: /List/


        public TreeController(QueryResolver queryResolver)
            : base(queryResolver)
        {
        }

        [Authorize]
        public ActionResult Index(string collectionNameWithInitFilters)
        {
            var temp = collectionNameWithInitFilters.Split(new[] { '|' });

            var query = QueryResolver.Execute(new GetTypeQueryBase(), temp[0]);
            var type = query;
            var structureWithCollectionName = new DataWithIdentifier<object, string>(type, null, temp[0],
                                                                                     "/Tree/Read/?collectionName=" +
                                                                                     temp[0] +
                                                                                     (temp.Length > 1
                                                                                          ? "&filters=" + temp[1]
                                                                                          : String.Empty));
            return View("Index", structureWithCollectionName);
        }

        [Authorize]
        public ActionResult Read(string collectionName, string filters, string filterContext, string listMetadataProvider, string detailViewGuid, string itemId)
        {
            var filterItems = filters != null
                                  ? filters.Split(',').ToList()
                                  : new List<string>();

            for (var i = 0; i < filterItems.Count; i++)
            {
                filterItems[i] = filterItems[i].Replace("{comma}", ",");
            }

            filterItems.Insert(0, itemId);

            return ReadData(collectionName, filterItems, filterContext, listMetadataProvider, detailViewGuid);
        }

        private ActionResult ReadData(string collectionName, List<string> filters, string filterContext, string treeMetadataProvider, string detailViewGuid)
        {
            var collectionType = QueryResolver.Execute(new GetTypeQueryBase(), collectionName);

            var gs = String.IsNullOrEmpty(treeMetadataProvider)
                     ? new DefaultTreeMetadataProvider()
                     : InstanceCreator.CreateInstance<IMetadataProvider<TreeMetadata>>(treeMetadataProvider);

            var structure = gs.GetMetadata(collectionType);

            var collection = QueryResolver.Execute(new GetListQueryBase(0, int.MaxValue, "Id asc", filters, filterContext, detailViewGuid), collectionName);

            return Json(new { data = structure.Serialize(collection), status = "OK" });
        }
    }
}
