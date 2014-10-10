namespace Hydra.Queries
{
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;

    [ContractClass(typeof(MediatorContract))]
    public interface IMediator
    {
        TResponse Request<TResponse>(IQuery<TResponse> query);

        Task<TResponse> RequestAsync<TResponse>(IQuery<TResponse> query);
    }

    [ContractClassFor(typeof(IMediator))]
    internal abstract class MediatorContract : IMediator
    {
        public TResponse Request<TResponse>(IQuery<TResponse> query)
        {
            Contract.Requires(query != null);

            return default(TResponse);
        }

        public Task<TResponse> RequestAsync<TResponse>(IQuery<TResponse> query)
        {
            Contract.Requires(query != null);

            return default(Task<TResponse>);
        }
    }
}