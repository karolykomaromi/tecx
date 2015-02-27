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
        public async Task Should_Use_Mediator_To_Resolve_Correct_Handler()
        {
            var container = new UnityContainer()
                .AddNewExtension<MediatorConfiguration>();

            IMediator sut = container.Resolve<IMediator>();

            MyResponse actual = await sut.Send(new MyRequest { Foo = "123456789" });

            Assert.Equal("987654321", actual.Bar);
        }

        [Fact]
        public async Task Should_Resolve_With_Decorator()
        {
            IUnityContainer container = new UnityContainer()
                .AddNewExtension<DecoratorExtension>()
                .AddNewExtension<MediatorConfiguration>();

            var handler = container.Resolve<IRequestHandler<MyRequest, MyResponse>>();

            Assert.IsNotType<MyRequestHandler>(handler);

            MyResponse actual = await handler.Handle(new MyRequest { Foo = "12345" });

            Assert.Equal("54321", actual.Bar);
        }
    }
}
