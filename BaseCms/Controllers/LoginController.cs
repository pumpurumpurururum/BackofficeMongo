using System.Web.Mvc;
using System.Web.Security;
using BaseCms.Events;
using BaseCms.Manager.Interfaces;
using BaseCms.Security;

namespace BaseCms.Controllers
{
    public class LoginController : Controller
    {
        private readonly SecurityProvider _securityProvider;
        private readonly EventSource _eventSource;
        private readonly IBackofficeManager _backOfficeManager;

        public LoginController(SecurityProvider securityProvider, EventSource eventSource, IBackofficeManager backOfficeManager)
        {
            _securityProvider = securityProvider;
            _eventSource = eventSource;
            _backOfficeManager = backOfficeManager;
        }

        [HttpGet]
        public ActionResult Index()
        {
            if (Request.IsAjaxRequest())
                return new ContentResult()
                {
                    Content = "timeout"
                };

            var partials = _backOfficeManager.GetPartials();
            if (partials.ContainsKey("LoginHeader"))
                ViewBag.LoginHeader = partials["LoginHeader"];

            return View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            _eventSource.OnLogoutEvent();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", new { returnurl = "/" });
        }

        [HttpPost]
        public ActionResult Index(string login, string password, string remember)
        {
            var user = _securityProvider.GetUserByLoginAndPassword(login, password);
            if (user != null)
            {
                _eventSource.OnLoginEvent(user.Id);
                FormsAuthentication.RedirectFromLoginPage(user.Id.ToString(), remember == "on");
            }

            var partials = _backOfficeManager.GetPartials();
            if (partials.ContainsKey("LoginHeader"))
                ViewBag.LoginHeader = partials["LoginHeader"];

            return View((object)"Ошибка входа. Пожалуйста, введите данные заново. Символы в паролях вводятся с учетом регистра.");
        }

    }
}
