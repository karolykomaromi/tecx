namespace Hydra
{
    using System.Diagnostics.Contracts;
    using System.Web.Mvc;

    public class ErrorHandlingActionInvoker : ControllerActionInvoker
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