using System.Web.Mvc;
using BaseCms.Manager.Interfaces;
using BaseCms.Security;

namespace BaseCms.Controllers
{
    public class RootController : Controller
    {
        private readonly SecurityProvider _securityProvider;
        //private readonly IBackofficeManager _backOfficeManager;

        public RootController(SecurityProvider securityProvider/*, IBackofficeManager backofficeManager*/)
        {
            _securityProvider = securityProvider;
            //_backOfficeManager = backofficeManager;
        }

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult Header()
        {
            return PartialView(_securityProvider.CurrentUser);
        }
    }
}
