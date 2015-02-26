namespace Hydra.Unity.Mediator
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;
    using Hydra.Infrastructure.Mediator;
    using Microsoft.Practices.Unity;

    [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1008:OpeningParenthesisMustBeSpacedCorrectly",
        Justification = "StyleCop does not recognize await plus cast operator.")]
    public class UnityMediator : IMediator
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

            dynamic handler = this.container.ResolveAll(handlerType);

            dynamic c = notification;

            await (Task)handler.Handle(c);
        }
    }
}