namespace Hydra.Queries
{
    using System.Threading.Tasks;

    public interface IMediator
    {
        TResponse Request<TResponse>(IQuery<TResponse> query);

        Task<TResponse> RequestAsync<TResponse>(IQuery<TResponse> query);
    }
}