namespace Hydra
{
    using System;
    using System.Web;
    using System.Web.Hosting;
    using System.Web.Mvc;
    using System.Web.Routing;
    using FluentValidation.Mvc;
    using Hydra.Configuration;
    using Hydra.Filters;
    using Hydra.Hosting;
    using Hydra.Infrastructure.Logging;
    using Hydra.Theming.Green;
    using Hydra.Unity;
    using Microsoft.Practices.Unity;

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            HydraEventSource.Log.Startup();

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new FeatureViewLocationRazorViewEngine());

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // setup fluent validation
            GlobalFilters.Filters.Add(new ValidatorActionFilter());
            FluentValidationModelValidatorProvider.Configure();

            // create application master container
            IUnityContainer container = new UnityContainer().AddNewExtension<UnityContainerConfiguration>();
            this.Application[Unity.Constants.ContainerKey] = container;

            // unity backed controller factory
            IControllerFactory factory = new UnityControllerFactory(new ContainerPerRequestAdapter());
            ControllerBuilder.Current.SetControllerFactory(factory);

            // fubu conventions need container backed dependency resolver but we don't want to figure out the whole
            // configuration for all the other components so we use the default resolver as fallback.
            IDependencyResolver fallback = DependencyResolver.Current;
            ////IDependencyResolver resolver = new UnityDependencyResolver(fallback, new ContainerPerRequestAdapter());
            IDependencyResolver resolver = new UnityDependencyResolver(fallback, new ContainerPerRequestAdapter());
            DependencyResolver.SetResolver(resolver);

            // this alternative virtual path provider uses embedded resources instead of the file system. if no matching resource is found it will use
            // the default, file-based vpp as fallback. the ervpp enables theming by packaging all theme resources into an assembly that can
            // be picked up from a yet to define folder and dynamically loaded into the app domain.
            VirtualPathProvider provider = EmbeddedResourceVirtualPathProvider.Create(new VirtualPathUtilityWrapper(), typeof(Green).Assembly);
            HostingEnvironment.RegisterVirtualPathProvider(provider);
        }

        protected void Application_End()
        {
            //// Logging ASP.NET Application Shutdown Events by ScottGu http://weblogs.asp.net/scottgu/433194
            HydraEventSource.Log.Shutdown();

            IUnityContainer container = this.Application[Unity.Constants.ContainerKey] as IUnityContainer;

            if (container != null)
            {
                container.Dispose();
            }
        }

        protected void Application_BeginRequest()
        {
            // we use child containers per request as lifetime scope. disposing the child container at the
            // end of each request will trigger a cleanup that disposes all objects that implement IDisposable and where
            // pulled from the container.
            IUnityContainer container = (IUnityContainer)this.Application[Unity.Constants.ContainerKey];

            IUnityContainer childContainer = container.CreateChildContainer();

            this.Context.Items[Unity.Constants.ContainerKey] = childContainer;
        }

        protected void Application_EndRequest()
        {
            IUnityContainer childContainer = (IUnityContainer)this.Context.Items[Unity.Constants.ContainerKey];

            if (childContainer != null)
            {
                this.Context.Items.Remove(Unity.Constants.ContainerKey);

                childContainer.Dispose();
            }
        }

        protected void Application_Error()
        {
            Exception ex = this.Server.GetLastError();

            if (ex is HttpUnhandledException)
            {
                ex = ex.InnerException;
            }

            this.Session["exception"] = ex; 

            // TODO weberse 2014-10-01 write exception to log
            HydraEventSource.Log.Error(ex);
        }
    }
}
