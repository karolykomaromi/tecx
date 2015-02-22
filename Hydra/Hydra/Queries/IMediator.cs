namespace Hydra.Queries
{
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;
    using Hydra.Commands;
    using Hydra.Infrastructure;

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

            return Default.Value<Task<TResult>>();
        }

        public Task<TResult> Send<TResult>(ICommand<TResult> command)
        {
            Contract.Requires(command != null);

            return Default.Value<Task<TResult>>();
        }
    }
}