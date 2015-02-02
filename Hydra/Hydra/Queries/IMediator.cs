namespace Hydra.Queries
{
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;
    using Hydra.Commands;

    [ContractClass(typeof(MediatorContract))]
    public interface IMediator
    {
        TResult Query<TResult>(IQuery<TResult> query);

        Task<TResult> QueryAsync<TResult>(IQuery<TResult> query);

        TResult Command<TResult>(ICommand<TResult> command);

        Task<TResult> CommandAsync<TResult>(ICommand<TResult> command);
    }

    [ContractClassFor(typeof(IMediator))]
    internal abstract class MediatorContract : IMediator
    {
        public TResult Query<TResult>(IQuery<TResult> query)
        {
            Contract.Requires(query != null);

            return default(TResult);
        }

        public Task<TResult> QueryAsync<TResult>(IQuery<TResult> query)
        {
            Contract.Requires(query != null);

            return default(Task<TResult>);
        }

        public TResult Command<TResult>(ICommand<TResult> command)
        {
            Contract.Requires(command != null);

            return default(TResult);
        }

        public Task<TResult> CommandAsync<TResult>(ICommand<TResult> command)
        {
            Contract.Requires(command != null);

            return default(Task<TResult>);
        }
    }
}