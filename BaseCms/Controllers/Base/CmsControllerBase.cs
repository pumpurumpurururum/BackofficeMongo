using System.Web.Mvc;
using BaseCms.CRUDRepository.Core;

namespace BaseCms.Controllers.Base
{
    /// <summary>
    /// Базовый класс для контроллеров CMS
    /// </summary>
    public class CmsControllerBase : Controller
    {
        public QueryResolver QueryResolver { get; set; }

        public CmsControllerBase(QueryResolver queryResolver)
        {
            QueryResolver = queryResolver;
        }
    }
}
