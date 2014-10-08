namespace Hydra.Unity
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Hydra.Infrastructure.Logging;
    using Microsoft.Practices.Unity;

    public class UnityControllerFactory : DefaultControllerFactory
    {
        private readonly IUnityContainer container;

        public UnityControllerFactory(IUnityContainer container)
        {
            Contract.Requires(container != null);

            this.container = container;
        }

        public override void ReleaseController(IController controller)
        {
            Contract.Requires(controller != null);

            this.container.Teardown(controller);
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            Contract.Requires(controllerType != null);
            Contract.Requires(typeof(IController).IsAssignableFrom(controllerType));
            Contract.Ensures(Contract.Result<IController>() != null);

            string url;
            if (requestContext.HttpContext != null && requestContext.HttpContext.Request != null &&
                requestContext.HttpContext.Request.Url != null)
            {
                url = requestContext.HttpContext.Request.Url.ToString();
            }
            else
            {
                url = "~/" + controllerType.Name;
            }

            HydraEventSource.Log.PageStart(url);

            return (IController)this.container.Resolve(controllerType);
        }
    }
}