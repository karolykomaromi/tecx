namespace Hydra.Unity
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reflection;
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
            Contract.Ensures(Contract.Result<IController>() != null);

            string url;
            if (requestContext.HttpContext.Request.Url != null)
            {
                url = requestContext.HttpContext.Request.Url.ToString();
            }
            else
            {
                url = "~/" + controllerType.Name;
            }

            HydraEventSource.Log.PageStart(url);

            try
            {
                object o = this.container.Resolve(controllerType);

                return (IController)o;
            }
            catch (ResolutionFailedException ex)
            {
                HydraEventSource.Log.MissingMapping(ex.TypeRequested, typeof(Missing).Name);
                throw;
            }
        }
    }
}