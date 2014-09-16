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

            this.Application["unity"] = container;

            IControllerFactory controllerFactory = new UnityCompositionRoot(container);

            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }

        protected void Application_BeginRequest()
        {
            IUnityContainer container = (IUnityContainer)this.Application["unity"];

            this.Context.Items["unity"] = container.CreateChildContainer();
        }

        protected void Application_EndRequest()
        {
            IUnityContainer childContainer = (IUnityContainer)this.Context.Items["unity"];

            if (childContainer != null)
            {
                childContainer.Dispose();
            }
        }
    }
}
