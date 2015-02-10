namespace Hydra.Queries
{
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;
    using Hydra.Commands;

    [ContractClass(typeof(MediatorContract))]
    public interface IMediator
    {
        Task<TResult> Query<TResult>(IQuery<TResult> query);

        Task<TResult> Send<TResult>(ICommand<TResult> command);
    }

    [ContractClassFor(typeof(IMediator))]
    internal abstract class MediatorContract : IMediator
    {
        public Task<TResult> Query<TResult>(IQuery<TResult> query)
        {
            Contract.Requires(query != null);

            return default(Task<TResult>);
        }

        public Task<TResult> Send<TResult>(ICommand<TResult> command)
        {
            Contract.Requires(command != null);

            return default(Task<TResult>);
        }
    }
}