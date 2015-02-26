namespace Hydra.Test.Configuration
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Hydra.Infrastructure.Mediator;

    public class MyLongRunningRequestHandler : IRequestHandler<MyLongRunningRequest, MyLongRunningResponse>
    {
        public async Task<MyLongRunningResponse> Handle(MyLongRunningRequest request)
        {
            return await Task<MyLongRunningResponse>.Factory.StartNew(() =>
            {
                Thread.Sleep(50);

                return new MyLongRunningResponse { Bar = new string(request.Foo.Reverse().ToArray()) };
            });
        }
    }
}