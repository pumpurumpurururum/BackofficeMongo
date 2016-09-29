using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BackofficeDemo.Common.HttpErrors
{
    public class NotFoundHandler : IHttpHandler
    {
        private static Func<RequestContext, IController> _createNotFoundController = context => new ErrorController();

        public static Func<RequestContext, IController> CreateNotFoundController
        {
            get
            {
                return _createNotFoundController;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _createNotFoundController = value;
            }
        }

        public bool IsReusable => false;

        public void ProcessRequest(HttpContext context)
        {
            this.ProcessRequest(new HttpContextWrapper(context));
        }

        private void ProcessRequest(HttpContextBase context)
        {
            var requestContext = this.CreateRequestContext(context);
            var controller = _createNotFoundController(requestContext);
            controller.Execute(requestContext);
        }

        private RequestContext CreateRequestContext(HttpContextBase context)
        {
            var routeData = new RouteData();
            routeData.Values.Add("controller", "NotFound");
            var requestContext = new RequestContext(context, routeData);
            return requestContext;
        }
    }
}
