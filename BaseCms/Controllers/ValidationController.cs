using System.Web.Mvc;
using BaseCms.CRUDRepository;
using BaseCms.CRUDRepository.Core;
using BaseCms.Controllers.Base;

namespace BaseCms.Controllers
{
    public class ValidationController : CmsControllerBase
    {
        public ValidationController(QueryResolver queryResolver)
            : base(queryResolver) { }

        [HttpPost]
        public bool Unique(string collectionName, string propertyName, string value, string identifier)
        {
            var query = new CheckUniqueQueryBase(propertyName, value, identifier);
            return QueryResolver
                .Execute(query, collectionName);
        }

    }
}
