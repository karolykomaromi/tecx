namespace Hydra.Composition
{
    using System;
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

        private class UnityMediator : IMediator
        {
            private readonly IUnityContainer container;

            public UnityMediator(IUnityContainer container)
            {
                Contract.Requires(container != null);

                this.container = container;
            }

            public TResult Query<TResult>(IQuery<TResult> query)
            {
                Type handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));

                dynamic handler = this.container.Resolve(handlerType);

                dynamic q = query;

                TResult result = (TResult)handler.Handle(q);

                return result;
            }

            public async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query)
            {
                Task<TResult> t = Task<TResult>.Factory.StartNew(() => this.Query<TResult>(query));

                return await t;
            }

            public TResult Command<TResult>(ICommand<TResult> command)
            {
                Type handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));

                dynamic handler = this.container.Resolve(handlerType);

                dynamic c = command;

                TResult result = (TResult)handler.Handle(c);

                return result;
            }

            public async Task<TResult> CommandAsync<TResult>(ICommand<TResult> command)
            {
                Task<TResult> t = Task<TResult>.Factory.StartNew(() => this.Command<TResult>(command));

                return await t;
            }
        }
    }
}