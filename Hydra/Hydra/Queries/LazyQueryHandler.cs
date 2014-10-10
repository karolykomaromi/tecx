namespace Hydra.Queries
{
    using System;
    using System.Diagnostics.Contracts;

    public class LazyQueryHandler<TQuery, TResponse> : IQueryHandler<TQuery, TResponse> 
        where TQuery : class, IQuery<TResponse>
    {
        private readonly Lazy<IQueryHandler<TQuery, TResponse>> instance;

        public LazyQueryHandler(Func<IQueryHandler<TQuery, TResponse>> factory)
        {
            Contract.Requires(factory != null);

            this.instance = new Lazy<IQueryHandler<TQuery, TResponse>>(factory);
        }

        public TResponse Handle(TQuery query)
        {
            return this.instance.Value.Handle(query);
        }
    }
}