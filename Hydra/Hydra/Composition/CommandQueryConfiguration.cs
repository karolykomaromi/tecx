namespace Hydra.Composition
{
    using Hydra.Infrastructure.Mediator;
    using Hydra.Unity.Mediator;
    using Microsoft.Practices.Unity;

    public class CommandQueryConfiguration : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Container.RegisterTypes(new RequestHandlerRegistrationConvention());

            this.Container.RegisterTypes(new NotificationHandlerRegistrationConvention());

            this.Container.RegisterType<IMediator, UnityMediator>(new ContainerControlledLifetimeManager());
        }
    }
}