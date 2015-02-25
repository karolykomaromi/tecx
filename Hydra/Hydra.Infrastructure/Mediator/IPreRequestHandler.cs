namespace Hydra.Infrastructure.Mediator
{
    using System.Threading.Tasks;

    public interface IPreRequestHandler<in TRequest>
    {
        Task Handle(TRequest request);
    }
}