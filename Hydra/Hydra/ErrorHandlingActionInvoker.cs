namespace Hydra
{
    using System.Diagnostics.Contracts;
    using System.Web.Mvc;
    using System.Web.Mvc.Async;

    /// <summary>
    /// Must derive from <see cref="AsyncControllerActionInvoker"/> to support async controller actions.
    /// <seealso cref="http://stackoverflow.com/questions/26163046/async-controller-method-displays-type-name-of-taskactionresult"/>
    /// </summary>
    public class ErrorHandlingActionInvoker : AsyncControllerActionInvoker
    {
        private readonly IExceptionFilter filter;

        public ErrorHandlingActionInvoker(IExceptionFilter filter)
        {
            Contract.Requires(filter != null);

            this.filter = filter;
        }

        protected override FilterInfo GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var filterInfo = base.GetFilters(controllerContext, actionDescriptor);

            filterInfo.ExceptionFilters.Add(this.filter);

            return filterInfo;
        }
    }
}