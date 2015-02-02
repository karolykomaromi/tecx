namespace Hydra.Test.Configuration
{
    using System.Linq;
    using System.Threading;
    using Hydra.Queries;

    public class MyLongRunningHandler : IQueryHandler<MyQuery, MyResponse>
    {
        public MyResponse Handle(MyQuery query)
        {
            Thread.Sleep(50);

            return new MyResponse { Bar = new string(query.Foo.Reverse().ToArray()) };
        }
    }
}