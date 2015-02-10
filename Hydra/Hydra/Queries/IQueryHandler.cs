namespace Hydra.Queries
{
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;

    [ContractClass(typeof(QueryHandlerContract<,>))]
    public interface IQueryHandler<in TQuery, TResponse>
        where TQuery : class, IQuery<TResponse>
    {
        Task<TResponse> Handle(TQuery query);
    }

    [ContractClassFor(typeof(IQueryHandler<,>))]
    internal abstract class QueryHandlerContract<TQuery, TResponse> : IQueryHandler<TQuery, TResponse>
        where TQuery : class, IQuery<TResponse>
    {
        public Task<TResponse> Handle(TQuery query)
        {
            Contract.Requires(query != null);

            return default(Task<TResponse>);
        }
    }
}