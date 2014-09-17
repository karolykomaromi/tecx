namespace Hydra
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Hydra.Unity;
    using Microsoft.Practices.Unity;

    public class MvcApplication : HttpApplication
    {
        private const string ContainerKey = "unity";

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            IUnityContainer container = new UnityContainer().AddNewExtension<UnityContainerConfiguration>();

            this.Application[ContainerKey] = container;

            IControllerFactory controllerFactory = new UnityCompositionRoot(new ContainerPerRequestAdapter());

            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }

        protected void Application_BeginRequest()
        {
            IUnityContainer container = (IUnityContainer)this.Application[ContainerKey];

            this.Context.Items[ContainerKey] = container.CreateChildContainer();
        }

        protected void Application_EndRequest()
        {
            IUnityContainer childContainer = (IUnityContainer)this.Context.Items[ContainerKey];

            if (childContainer != null)
            {
                childContainer.Dispose();

                this.Context.Items.Remove(ContainerKey);
            }
        }
    }
}
