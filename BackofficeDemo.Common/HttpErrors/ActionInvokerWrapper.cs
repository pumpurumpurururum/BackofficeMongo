using System.Web;
using System.Web.Mvc;

namespace BackofficeDemo.Common.HttpErrors
{
    internal class ActionInvokerWrapper : IActionInvoker
    {
        private readonly IActionInvoker _actionInvoker;

        public ActionInvokerWrapper(IActionInvoker actionInvoker)
        {
            _actionInvoker = actionInvoker;
        }

        public bool InvokeAction(ControllerContext controllerContext, string actionName)
        {
            if (!InvokeActionWith404Catch(controllerContext, actionName))
                ExecuteNotFoundControllerAction(controllerContext);
            return true;
        }

        private static void ExecuteNotFoundControllerAction(ControllerContext controllerContext)
        {
            var controller = NotFoundHandler.CreateNotFoundController != null
                ? (NotFoundHandler.CreateNotFoundController(controllerContext.RequestContext) ??
                   new ErrorController())
                : new ErrorController();

            controller.Execute(controllerContext.RequestContext);
        }

        private bool InvokeActionWith404Catch(ControllerContext controllerContext, string actionName)
        {
            try
            {
                return _actionInvoker.InvokeAction(controllerContext, actionName);
            }
            catch (HttpException ex)
            {
                if (ex.GetHttpCode() == 404)
                {
                    return false;
                }

                throw;
            }
        }
    }
}
