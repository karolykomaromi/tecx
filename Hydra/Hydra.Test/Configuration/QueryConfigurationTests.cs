namespace Hydra.Test.Configuration
{
    using System.Linq;
    using System.Threading;
    using Hydra.Configuration;
    using Hydra.Queries;
    using Microsoft.Practices.Unity;
    using Xunit;

    public class QueryConfigurationTests
    {
        [Fact]
        public void Should_Use_Mediator_To_Resolve_Correct_Handler()
        {
            var container =
                new UnityContainer().AddNewExtension<QueryConfiguration>()
                    .RegisterType<IQueryHandler<MyQuery, MyResponse>, MyQueryHandler>();

            IMediator sut = container.Resolve<IMediator>();

            MyResponse actual = sut.Request(new MyQuery { Foo = "123456789" });

            Assert.Equal("987654321", actual.Bar);
        }

        [Fact]
        public async void Should_Allow_Async_Request_To_Sync_Handler()
        {
            var container =
                new UnityContainer().AddNewExtension<QueryConfiguration>()
                    .RegisterType<IQueryHandler<MyQuery, MyResponse>, MyLongRunningHandler>();

            IMediator sut = container.Resolve<IMediator>();

            MyResponse actual = await sut.RequestAsync(new MyQuery { Foo = "123456789" });

            Assert.Equal("987654321", actual.Bar);
        }
    }

    public class MyLongRunningHandler : IQueryHandler<MyQuery, MyResponse>
    {
        public MyResponse Handle(MyQuery query)
        {
            Thread.Sleep(50);

            return new MyResponse { Bar = new string(query.Foo.Reverse().ToArray()) };
        }
    }

    public class MyQueryHandler : IQueryHandler<MyQuery, MyResponse>
    {
        public MyResponse Handle(MyQuery query)
        {
            return new MyResponse { Bar = new string(query.Foo.Reverse().ToArray()) };
        }
    }

    public class MyQuery : IQuery<MyResponse>
    {
        public string Foo { get; set; }
    }

    public class MyResponse
    {
        public string Bar { get; set; }
    }
}
