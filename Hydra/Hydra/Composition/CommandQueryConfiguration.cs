namespace Hydra.Composition
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Threading.Tasks;
    using Hydra.Infrastructure.Mediator;
    using Hydra.Infrastructure.Reflection;
    using Microsoft.Practices.Unity;

    public class CommandQueryConfiguration : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Container.RegisterTypes(
                AllClasses.FromLoadedAssemblies().Where(t => TypeHelper.ImplementsOpenGenericInterface(t, typeof(IRequestHandler<,>))),
                WithMappings.FromAllInterfaces);

            this.Container.RegisterTypes(
                AllClasses.FromLoadedAssemblies().Where(t => TypeHelper.ImplementsOpenGenericInterface(t, typeof(INotificationHandler<>))),
                WithMappings.FromAllInterfaces);

            this.Container.RegisterType<IMediator, UnityMediator>(new ContainerControlledLifetimeManager());
        }

        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1008:OpeningParenthesisMustBeSpacedCorrectly",
            Justification = "StyleCop does not recognize await plus cast operator.")]
        private class UnityMediator : IMediator
        {
            private readonly IUnityContainer container;

            public UnityMediator(IUnityContainer container)
            {
                Contract.Requires(container != null);

                this.container = container;
            }

            public async Task<TResult> Send<TResult>(IRequest<TResult> request)
            {
                Type handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResult));

                dynamic handler = this.container.Resolve(handlerType);

                dynamic q = request;

                return await (Task<TResult>)handler.Handle(q);
            }

            public async Task Publish<TNotification>(TNotification notification) where TNotification : class
            {
                Type handlerType = typeof(INotificationHandler<>).MakeGenericType(typeof(TNotification));

                dynamic handler = this.container.Resolve(handlerType);

                dynamic c = notification;

                await (Task)handler.Handle(c);
            }
        }
    }
}