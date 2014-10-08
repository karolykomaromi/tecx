namespace Hydra.Configuration
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Threading.Tasks;
    using Hydra.Infrastructure.Reflection;
    using Hydra.Queries;
    using Microsoft.Practices.Unity;

    public class QueryConfiguration : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Container.RegisterTypes(
                AllClasses.FromAssemblies(typeof(IQuery<>).Assembly).Where(t => TypeHelper.ImplementsOpenGenericInterface(t, typeof(IQueryHandler<,>))),
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

            public TResponse Request<TResponse>(IQuery<TResponse> query)
            {
                Type handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResponse));

                dynamic handler = this.container.Resolve(handlerType);

                dynamic q = query;

                TResponse response = (TResponse)handler.Handle(q);

                return response;
            }

            public async Task<TResponse> RequestAsync<TResponse>(IQuery<TResponse> query)
            {
                Task<TResponse> t = Task<TResponse>.Factory.StartNew(() => this.Request<TResponse>(query));

                return await t;
            }
        }
    }
}