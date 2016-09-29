using System.Web.Mvc;

namespace Helper
{
    public static class UrlHelperExtensions
    {
        /// <summary>
        /// Returns a full qualified action URL
        /// </summary>
        public static string QualifiedAction(this UrlHelper url, string actionName, string controllerName, object routeValues = null)
        {
            return url.Action(actionName, controllerName, routeValues, url.RequestContext.HttpContext.Request.Url.Scheme);
        }

        /// <summary>
        /// Returns a full qualified route URL
        /// </summary>
        public static string QualifiedRoute(this UrlHelper url, string routeName, object routeValues = null)
        {
            return url.RouteUrl(routeName, routeValues, url.RequestContext.HttpContext.Request.Url.Scheme);
        }

    }
}
