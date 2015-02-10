namespace Hydra.Queries
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;
    using Hydra.Commands;

    public class LazyMediator : IMediator
    {
        private readonly Lazy<IMediator> instance;

        public LazyMediator(Func<IMediator> factory)
        {
            Contract.Requires(factory != null);

            this.instance = new Lazy<IMediator>(factory);
        }

        public Task<TResult> Query<TResult>(IQuery<TResult> query)
        {
            return this.instance.Value.Query(query);
        }

        public Task<TResult> Send<TResult>(ICommand<TResult> command)
        {
            return this.instance.Value.Send(command);
        }
    }
}