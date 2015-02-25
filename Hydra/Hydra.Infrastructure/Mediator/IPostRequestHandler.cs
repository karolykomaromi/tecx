namespace Hydra.Infrastructure.Mediator
{
    using System.Threading.Tasks;

    public interface IPostRequestHandler<in TRequest, in TResponse>
    {
        Task Handle(TRequest request, TResponse response);
    }
}