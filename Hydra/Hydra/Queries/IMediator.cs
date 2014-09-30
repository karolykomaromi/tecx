namespace Hydra.Queries
{
    public interface IMediator
    {
        TResponse Request<TResponse>(IQuery<TResponse> request);
    }
}