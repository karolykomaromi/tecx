namespace Hydra.Test.Configuration
{
    using Hydra.Composition;
    using Hydra.Infrastructure.Mediator;
    using Microsoft.Practices.Unity;
    using Xunit;

    public class QueryConfigurationTests
    {
        [Fact]
        public async void Should_Use_Mediator_To_Resolve_Correct_Handler()
        {
            var container =
                new UnityContainer().AddNewExtension<CommandQueryConfiguration>()
                    .RegisterType<IRequestHandler<MyRequest, MyResponse>, MyRequestHandler>();

            IMediator sut = container.Resolve<IMediator>();

            MyResponse actual = await sut.Send(new MyRequest { Foo = "123456789" });

            Assert.Equal("987654321", actual.Bar);
        }
    }
}
