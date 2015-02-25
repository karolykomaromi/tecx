namespace Hydra.Infrastructure.Mediator
{
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;

    [ContractClass(typeof(RequestHandlerContract<,>))]
    public interface IRequestHandler<in TQuery, TResponse>
        where TQuery : class, IRequest<TResponse>
    {
        Task<TResponse> Handle(TQuery request);
    }

    [ContractClassFor(typeof(IRequestHandler<,>))]
    internal abstract class RequestHandlerContract<TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
        where TQuery : class, IRequest<TResponse>
    {
        public Task<TResponse> Handle(TQuery request)
        {
            Contract.Requires(request != null);

            return Default.Value<Task<TResponse>>();
        }
    }
}