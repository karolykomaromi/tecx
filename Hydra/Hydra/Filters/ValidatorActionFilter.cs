namespace Hydra.Filters
{
    using System.Web.Mvc;
    using Hydra.Infrastructure;
    using Newtonsoft.Json;

    /// <summary>
    /// <seealso cref="http://timgthomas.com/2013/09/simplify-client-side-validation-by-adding-a-server/"/>
    /// </summary>
    public class ValidatorActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.Controller.ViewData.ModelState.IsValid)
            {
                return;
            }

            var serializationSettings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

            var serializedModelState = JsonConvert.SerializeObject(
              filterContext.Controller.ViewData.ModelState,
              serializationSettings);

            var result = new ContentResult
                {
                    Content = serializedModelState,
                    ContentType = Constants.ContentTypes.Json
                };

            filterContext.HttpContext.Response.StatusCode = HttpStatusCodes.ClientError4xx.BadRequest;
            filterContext.Result = result;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            RedirectToRouteResult redirect = filterContext.Result as RedirectToRouteResult;

            if (redirect == null)
            {
                return;
            }

            var helper = new UrlHelper(filterContext.RequestContext);

            string url = helper.RouteUrl(redirect.RouteName, redirect.RouteValues);

            var data = new { redirect = url };

            var serializationSettings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

            var searializedRedirect = JsonConvert.SerializeObject(data, serializationSettings);

            var result = new ContentResult
                {
                    Content = searializedRedirect,
                    ContentType = Constants.ContentTypes.Json
                };

            filterContext.HttpContext.Response.StatusCode = HttpStatusCodes.Successfull2xx.Ok;
            filterContext.Result = result;
        }
    }
}