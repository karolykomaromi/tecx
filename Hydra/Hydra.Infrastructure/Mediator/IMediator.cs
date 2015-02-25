namespace Hydra.Infrastructure.Mediator
{
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;

    [ContractClass(typeof(MediatorContract))]
    public interface IMediator
    {
        Task<TResponse> Send<TResponse>(IRequest<TResponse> request);

        Task Publish<TNotification>(TNotification notification) where TNotification : class;
    }

    [ContractClassFor(typeof(IMediator))]
    internal abstract class MediatorContract : IMediator
    {
        public Task<TResult> Send<TResult>(IRequest<TResult> request)
        {
            Contract.Requires(request != null);

            return Default.Value<Task<TResult>>();
        }

        public Task Publish<TNotification>(TNotification notification) where TNotification : class
        {
            Contract.Requires(notification != null);

            return Default.Value<Task>();
        }
    }
}