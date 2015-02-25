namespace Hydra.Test.Configuration
{
    using System.Linq;
    using System.Threading.Tasks;
    using Hydra.Infrastructure.Mediator;

    public class MyRequestHandler : IRequestHandler<MyRequest, MyResponse>
    {
        public async Task<MyResponse> Handle(MyRequest request)
        {
            return await Task<MyResponse>.Factory.StartNew(() => new MyResponse { Bar = new string(request.Foo.Reverse().ToArray()) });
        }
    }
}