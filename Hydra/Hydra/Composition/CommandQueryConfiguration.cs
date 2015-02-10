namespace Hydra.Composition
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Threading.Tasks;
    using Hydra.Commands;
    using Hydra.Infrastructure.Reflection;
    using Hydra.Queries;
    using Microsoft.Practices.Unity;

    public class CommandQueryConfiguration : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Container.RegisterTypes(
                AllClasses.FromAssemblies(typeof(IQuery<>).Assembly).Where(t => TypeHelper.ImplementsOpenGenericInterface(t, typeof(IQueryHandler<,>))),
                WithMappings.FromAllInterfaces);

            this.Container.RegisterTypes(
                AllClasses.FromAssemblies(typeof(ICommand<>).Assembly).Where(t => TypeHelper.ImplementsOpenGenericInterface(t, typeof(ICommandHandler<,>))),
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

            public async Task<TResult> Query<TResult>(IQuery<TResult> query)
            {
                Type handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));

                dynamic handler = this.container.Resolve(handlerType);

                dynamic q = query;

                return await (Task<TResult>)handler.Handle(q);
            }

            public async Task<TResult> Send<TResult>(ICommand<TResult> command)
            {
                Type handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));

                dynamic handler = this.container.Resolve(handlerType);

                dynamic c = command;

                return await (Task<TResult>)handler.Handle(c);
            }
        }
    }
}