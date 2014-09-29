namespace Hydra
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Hydra.Unity;
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
}
