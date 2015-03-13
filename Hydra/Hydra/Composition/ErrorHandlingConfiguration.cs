namespace Hydra.Composition
{
    using System.Linq;
    using System.Web.Mvc;
    using Hydra.Infrastructure.Reflection;
    using Hydra.Unity;
    using Microsoft.Practices.Unity;

    public class ErrorHandlingConfiguration : UnityContainerExtension
    {
        protected override void Initialize()
        {
            string actionInvokerPropertyName = TypeHelper.GetPropertyName((Controller ctrl) => ctrl.ActionInvoker);

            this.Container.RegisterTypes(
                    AllTypes.FromHydraAssemblies().Where(t => typeof(Controller).IsAssignableFrom(t)),
                    getInjectionMembers: t => new InjectionMember[] { new InjectionProperty(actionInvokerPropertyName) });

            this.Container.RegisterType<IActionInvoker, ErrorHandlingActionInvoker>();
            this.Container.RegisterType<IExceptionFilter, HandleErrorAttribute>();
        }
    }
}