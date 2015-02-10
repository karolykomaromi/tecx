namespace Hydra.Test.Configuration
{
    using System.Linq;
    using System.Threading.Tasks;
    using Hydra.Queries;

    public class MyQueryHandler : IQueryHandler<MyQuery, MyResponse>
    {
        public async Task<MyResponse> Handle(MyQuery query)
        {
            return await Task<MyResponse>.Factory.StartNew(() => new MyResponse { Bar = new string(query.Foo.Reverse().ToArray()) });
        }
    }
}