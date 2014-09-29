namespace Hydra.Unity.Test
{
    using Microsoft.Practices.Unity;
    using Xunit;

    public class Tests
    {
        [Fact]
        public void Should_Map_Open_Generic_Interface_To_Open_Generic_Implementation()
        {
            var container = new UnityContainer().RegisterType(typeof(IElementGenerator<>), typeof(ElementGenerator<>));

            IElementGenerator<Foo> actual = container.Resolve<IElementGenerator<Foo>>();

            Assert.IsType<ElementGenerator<Foo>>(actual);
        }
    }

    public class Foo
    {
        
    }

    public interface IElementGenerator<T>
    {
        
    }

    public class ElementGenerator<T> : IElementGenerator<T>
    {
        
    }
}
