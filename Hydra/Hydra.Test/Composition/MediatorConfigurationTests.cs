namespace Hydra.Test.Composition
{
    using System.Threading.Tasks;
    using Hydra.Composition;
    using Hydra.Infrastructure.Mediator;
    using Hydra.Unity.Decoration;
    using Microsoft.Practices.Unity;
    using Xunit;

    public class MediatorConfigurationTests
    {
        [Fact]
        public async Task Should_Use_Mediator_To_Resolve_Correct_Request_Handler()
        {
            var container = new UnityContainer()
                .AddNewExtension<MediatorConfiguration>();

            IMediator sut = container.Resolve<IMediator>();

            MyResponse actual = await sut.Send(new MyRequest { Foo = "123456789" });

            Assert.Equal("987654321", actual.Bar);
        }

        [Fact]
        public async Task Should_Resolve_Request_Handler_With_Decorator()
        {
            IUnityContainer container = new UnityContainer()
                .AddNewExtension<DecoratorExtension>()
                .AddNewExtension<MediatorConfiguration>();

            var handler = container.Resolve<IRequestHandler<MyRequest, MyResponse>>();

            Assert.IsNotType<MyRequestHandler>(handler);

            MyResponse actual = await handler.Handle(new MyRequest { Foo = "12345" });

            Assert.Equal("54321", actual.Bar);
        }

        [Fact]
        public async Task Should_Resolve_Notification_Handlers()
        {
            FooOccuredCounter counter = new FooOccuredCounter();

            IUnityContainer container = new UnityContainer()
                .RegisterInstance(counter)
                .AddNewExtension<MediatorConfiguration>();
            
            IMediator sut = container.Resolve<IMediator>();

            await sut.Publish(new FooOccured());

            Assert.Equal(2, counter.Count);
        }

        public class Foo1NotificationHandler : INotificationHandler<FooOccured>
        {
            private readonly FooOccuredCounter counter;

            public Foo1NotificationHandler(FooOccuredCounter counter)
            {
                this.counter = counter;
            }

            public async Task Handle(FooOccured notification)
            {
                await Task.Factory.StartNew(() => { this.counter.Count++; });
            }
        }

        public class FooOccuredCounter
        {
            public int Count { get; set; }
        }

        public class Foo2NotificationHandler : INotificationHandler<FooOccured>
        {
            private readonly FooOccuredCounter counter;

            public Foo2NotificationHandler(FooOccuredCounter counter)
            {
                this.counter = counter;
            }

            public async Task Handle(FooOccured notification)
            {
                await Task.Factory.StartNew(() => { this.counter.Count++; });
            }
        }

        public class FooOccured
        {
        }
    }
}
