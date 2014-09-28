namespace Hydra
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Microsoft.Practices.Unity;

    public class UnityControllerFactory : DefaultControllerFactory
    {
        private readonly IUnityContainer container;

        public UnityControllerFactory(IUnityContainer container)
        {
            Contract.Requires(container != null);

            this.container = container;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            Contract.Requires(controllerType != null);
            Contract.Requires(typeof(IController).IsAssignableFrom(controllerType));
            Contract.Ensures(Contract.Result<IController>() != null);

            return (IController)this.container.Resolve(controllerType);
        }
    }
}