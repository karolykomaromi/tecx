using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Hydra
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Microsoft.Practices.Unity;

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            IUnityContainer container = new UnityContainer().AddNewExtension<UnityContainerConfiguration>();

            this.Application[Constants.ContainerKey] = container;

            IControllerFactory factory = new UnityControllerFactory(new ContainerPerRequestAdapter());

            ControllerBuilder.Current.SetControllerFactory(factory);

            IDependencyResolver current = DependencyResolver.Current;

            IDependencyResolver resolver = new UnityDependencyResolver(current, new ContainerPerRequestAdapter());

            DependencyResolver.SetResolver(resolver);
        }

        protected void Application_BeginRequest()
        {
            IUnityContainer container = (IUnityContainer)this.Application[Constants.ContainerKey];

            this.Context.Items[Constants.ContainerKey] = container.CreateChildContainer();
        }

        protected void Application_EndRequest()
        {
            IUnityContainer childContainer = (IUnityContainer)this.Context.Items[Constants.ContainerKey];

            if (childContainer != null)
            {
                this.Context.Items.Remove(Constants.ContainerKey);

                childContainer.Dispose();
            }
        }
    }

    public class UnityDependencyResolver : IDependencyResolver
    {
        private readonly IDependencyResolver forward;
        private readonly IUnityContainer container;

        public UnityDependencyResolver(IDependencyResolver forward, IUnityContainer container)
        {
            Contract.Requires(forward != null);
            Contract.Requires(container != null);

            this.forward = forward;
            this.container = container;
        }

        public object GetService(Type serviceType)
        {
            try
            {
                object o = this.container.Resolve(serviceType);

                return o;
            }
            catch(Exception ex)
            {
                return this.forward.GetService(serviceType);
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                IEnumerable<object> all = this.container.ResolveAll(serviceType);

                return all;
            }
            catch(Exception ex)
            {
                return this.forward.GetServices(serviceType);
            }
        }
    }
}
