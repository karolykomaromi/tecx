namespace Hydra.Test.Configuration
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Hydra.Queries;

    public class MyLongRunningHandler : IQueryHandler<MyQuery, MyResponse>
    {
        public Task<MyResponse> Handle(MyQuery query)
        {
            return Task<MyResponse>.Factory.StartNew(() =>
            {
                Thread.Sleep(50);

                return new MyResponse { Bar = new string(query.Foo.Reverse().ToArray()) };
            });
        }
    }
}