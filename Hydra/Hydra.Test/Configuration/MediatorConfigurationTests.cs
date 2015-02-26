namespace Hydra.Test.Configuration
{
    using System.Threading.Tasks;
    using Hydra.Composition;
    using Hydra.Infrastructure.Mediator;
    using Microsoft.Practices.Unity;
    using Xunit;

    public class MediatorConfigurationTests
    {
        [Fact]
        public async Task Should_Use_Mediator_To_Resolve_Correct_Handler()
        {
            var container = new UnityContainer().AddNewExtension<MediatorConfiguration>();

            IMediator sut = container.Resolve<IMediator>();

            MyResponse actual = await sut.Send(new MyRequest { Foo = "123456789" });

            Assert.Equal("987654321", actual.Bar);
        }
    }
}
