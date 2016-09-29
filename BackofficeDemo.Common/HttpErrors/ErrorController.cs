using System.Web.Mvc;

namespace BackofficeDemo.Common.HttpErrors
{
    public class ErrorController : Controller
    {
        public ActionResult NotFound()
        {
            return new NotFoundViewResult();
        }

        //protected override void ExecuteCore()
        //{
        //    new NotFoundViewResult().ExecuteResult(this.ControllerContext);
        //}

        public ActionResult Http500()
        {
            return new Error500ViewResult();
        }

    }
}
