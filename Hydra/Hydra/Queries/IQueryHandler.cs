namespace Hydra.Queries
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(QueryHandlerContract<,>))]
    public interface IQueryHandler<in TQuery, out TResponse>
        where TQuery : class, IQuery<TResponse>
    {
        TResponse Handle(TQuery query);
    }

    [ContractClassFor(typeof(IQueryHandler<,>))]
    internal abstract class QueryHandlerContract<TQuery, TResponse> : IQueryHandler<TQuery, TResponse>
        where TQuery : class, IQuery<TResponse>
    {
        public TResponse Handle(TQuery query)
        {
            Contract.Requires(query != null);

            return default(TResponse);
        }
    }
}