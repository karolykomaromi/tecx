namespace Hydra.Configuration
{
    using System.Linq;
    using System.Web.Mvc;
    using Microsoft.Practices.Unity;

    public class ErrorHandlingConfiguration : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Container.RegisterTypes(
                    AllClasses.FromLoadedAssemblies().Where(t => typeof(Controller).IsAssignableFrom(t)),
                    getInjectionMembers: t => new InjectionMember[] { new InjectionProperty("ActionInvoker") });

            this.Container.RegisterType<IActionInvoker, ErrorHandlingActionInvoker>();
            this.Container.RegisterType<IExceptionFilter, HandleErrorAttribute>();
        }
    }
}