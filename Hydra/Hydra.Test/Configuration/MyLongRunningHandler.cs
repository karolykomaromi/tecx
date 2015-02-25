namespace Hydra.Test.Configuration
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Hydra.Infrastructure.Mediator;

    public class MyLongRunningHandler : IRequestHandler<MyRequest, MyResponse>
    {
        public async Task<MyResponse> Handle(MyRequest request)
        {
            return await Task<MyResponse>.Factory.StartNew(() =>
            {
                Thread.Sleep(50);

                return new MyResponse { Bar = new string(request.Foo.Reverse().ToArray()) };
            });
        }
    }
}