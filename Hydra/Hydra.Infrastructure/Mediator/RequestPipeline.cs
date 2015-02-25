namespace Hydra.Infrastructure.Mediator
{
    using System.Threading.Tasks;

    public class RequestPipeline<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : class, IRequest<TResponse>
    {
        private readonly IRequestHandler<TRequest, TResponse> inner;
        private readonly IPreRequestHandler<TRequest>[] preRequestHandlers;
        private readonly IPostRequestHandler<TRequest, TResponse>[] postRequestHandlers;

        public RequestPipeline(IRequestHandler<TRequest, TResponse> inner, IPreRequestHandler<TRequest>[] preRequestHandlers, IPostRequestHandler<TRequest, TResponse>[] postRequestHandlers)
        {
            this.inner = inner;
            this.preRequestHandlers = preRequestHandlers;
            this.postRequestHandlers = postRequestHandlers;
        }

        public async Task<TResponse> Handle(TRequest request)
        {
            foreach (var preRequestHandler in this.preRequestHandlers)
            {
                await preRequestHandler.Handle(request);
            }

            TResponse response = await this.inner.Handle(request);

            foreach (var postRequestHandler in this.postRequestHandlers)
            {
                await postRequestHandler.Handle(request, response);
            }

            return response;
        }
    }
}