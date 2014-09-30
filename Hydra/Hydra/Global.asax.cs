namespace Hydra
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using FluentValidation.Mvc;
    using Hydra.Filters;
    using Hydra.Unity;
    using Microsoft.Practices.Unity;

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // setup fluent validation
            GlobalFilters.Filters.Add(new ValidatorActionFilter());
            FluentValidationModelValidatorProvider.Configure();

            // create application master container
            IUnityContainer container = new UnityContainer().AddNewExtension<UnityContainerConfiguration>();
            this.Application[Constants.ContainerKey] = container;

            // unity backed controller factory
            IControllerFactory factory = new UnityControllerFactory(new ContainerPerRequestAdapter());
            ControllerBuilder.Current.SetControllerFactory(factory);

            // fubu conventions need container backed dependency resolver but we don't want to figure out the whole
            // configuration for all the other components so we use the default resolver as fallback.
            IDependencyResolver fallback = DependencyResolver.Current;
            IDependencyResolver resolver = new UnityDependencyResolver(fallback, new ContainerPerRequestAdapter());
            DependencyResolver.SetResolver(resolver);
        }

        protected void Application_BeginRequest()
        {
            // we use child containers per request as lifetime scope. disposing the child container at the
            // end of each request will trigger a cleanup that disposes all objects that implement IDisposable and where
            // pulled from the container.
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
