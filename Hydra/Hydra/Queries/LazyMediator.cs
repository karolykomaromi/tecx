namespace Hydra.Queries
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

        public TResponse Request<TResponse>(IQuery<TResponse> query)
        {
            return this.instance.Value.Request(query);
        }

        public Task<TResponse> RequestAsync<TResponse>(IQuery<TResponse> query)
        {
            return this.instance.Value.RequestAsync(query);
        }
    }
}