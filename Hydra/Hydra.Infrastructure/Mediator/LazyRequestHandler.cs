namespace Hydra.Infrastructure.Mediator
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;

    public class LazyRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> 
        where TRequest : class, IRequest<TResponse>
    {
        private readonly Lazy<IRequestHandler<TRequest, TResponse>> instance;

        public LazyRequestHandler(Func<IRequestHandler<TRequest, TResponse>> factory)
        {
            Contract.Requires(factory != null);

            this.instance = new Lazy<IRequestHandler<TRequest, TResponse>>(factory);
        }

        public async Task<TResponse> Handle(TRequest request)
        {
            return await this.instance.Value.Handle(request);
        }
    }
}