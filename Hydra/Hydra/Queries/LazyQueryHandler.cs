namespace Hydra.Queries
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;

    public class LazyQueryHandler<TQuery, TResponse> : IQueryHandler<TQuery, TResponse> 
        where TQuery : class, IQuery<TResponse>
    {
        private readonly Lazy<IQueryHandler<TQuery, TResponse>> instance;

        public LazyQueryHandler(Func<IQueryHandler<TQuery, TResponse>> factory)
        {
            Contract.Requires(factory != null);

            this.instance = new Lazy<IQueryHandler<TQuery, TResponse>>(factory);
        }

        public async Task<TResponse> Handle(TQuery query)
        {
            return await this.instance.Value.Handle(query);
        }
    }
}