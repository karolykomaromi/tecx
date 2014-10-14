namespace Hydra.Test.Configuration
{
    using System.Linq;
    using Hydra.Queries;

    public class MyQueryHandler : IQueryHandler<MyQuery, MyResponse>
    {
        public MyResponse Handle(MyQuery query)
        {
            return new MyResponse { Bar = new string(query.Foo.Reverse().ToArray()) };
        }
    }
}