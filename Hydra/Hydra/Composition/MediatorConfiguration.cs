namespace Hydra.Composition
{
    using System;
    using Hydra.Infrastructure.Mediator;
    using Hydra.Unity.Decoration;
    using Hydra.Unity.Mediator;
    using Microsoft.Practices.Unity;

    public class MediatorConfiguration : UnityContainerExtension
    {
        private bool IsDecoratorExtensionInstalled
        {
            get { return this.Container.Configure<DecoratorExtension>() != null; }
        }

        protected override void Initialize()
        {
            if (this.IsDecoratorExtensionInstalled)
            {
                this.Container.RegisterTypes(new RequestHandlerRegistrationConvention(RequestHandlers.Pipeline), true);
            }
            else
            {
                this.Container.RegisterTypes(new RequestHandlerRegistrationConvention(RequestHandlers.None));
            }

            this.Container.RegisterTypes(new NotificationHandlerRegistrationConvention());

            this.Container.RegisterType<IMediator, UnityMediator>(new ContainerControlledLifetimeManager());
        }
    }
}