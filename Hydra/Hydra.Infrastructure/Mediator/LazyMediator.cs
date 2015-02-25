namespace Hydra.Infrastructure.Mediator
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;

    public class LazyMediator : IMediator
    {
        private readonly Lazy<IMediator> instance;

        public LazyMediator(Func<IMediator> factory)
        {
            Contract.Requires(factory != null);

            this.instance = new Lazy<IMediator>(factory);
        }

        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
        {
            return await this.instance.Value.Send(request);
        }

        public async Task Publish<TNotification>(TNotification notification) where TNotification : class
        {
            await this.instance.Value.Publish(notification);
        }
    }
}