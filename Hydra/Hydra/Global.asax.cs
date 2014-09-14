using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Hydra
{
    using Microsoft.Practices.Unity;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            IUnityContainer container = new UnityContainer().AddNewExtension<UnityContainerConfiguration>();

            IControllerFactory controllerFactory = new UnityCompositionRoot(container);

            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }
    }
}
