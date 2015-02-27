namespace Hydra.Unity.Test.Decoration
{
    using Hydra.Unity.Decoration;
    using Microsoft.Practices.Unity;
    using Xunit;

    public class DecoratorExtensionTests
    {
        [Fact]
        public void Should_Decorate_Non_Generic_Interfaces_With_Default_Name()
        {
            IUnityContainer container = new UnityContainer().AddNewExtension<DecoratorExtension>();

            container.RegisterType<INonGenericInterface, ImplementationOfNonGenericInterface>();
            container.RegisterType<INonGenericInterface, DecoratorForNonGenericInterface>();

            var actual = container.Resolve<INonGenericInterface>();

            Assert.Equal(42 * 2, actual.Foo());
        }

        [Fact]
        public void Should_Decorate_Non_Generic_Interfaces_With_Non_Default_Name()
        {
            IUnityContainer container = new UnityContainer().AddNewExtension<DecoratorExtension>();

            container.RegisterType<INonGenericInterface, ImplementationOfNonGenericInterface>("1");
            container.RegisterType<INonGenericInterface, DecoratorForNonGenericInterface>("1");

            var actual = container.Resolve<INonGenericInterface>("1");

            Assert.Equal(42 * 2, actual.Foo());
        }

        [Fact]
        public void Should_Decorate_Generic_Interfaces_With_Default_Name()
        {
            IUnityContainer container = new UnityContainer().AddNewExtension<DecoratorExtension>();

            container.RegisterType<IGenericInterface<int>, ImplementationOfGenericInterface>();
            container.RegisterType<IGenericInterface<int>, DecoratorForGenericInterface>();

            var actual = container.Resolve<IGenericInterface<int>>();

            Assert.Equal(42 * 2, actual.Foo());
        }

        [Fact]
        public void Should_Decorate_Generic_Interfaces_With_Non_Default_Name()
        {
            IUnityContainer container = new UnityContainer().AddNewExtension<DecoratorExtension>();

            container.RegisterType<IGenericInterface<int>, ImplementationOfGenericInterface>("1");
            container.RegisterType<IGenericInterface<int>, DecoratorForGenericInterface>("1");

            var actual = container.Resolve<IGenericInterface<int>>("1");

            Assert.Equal(42 * 2, actual.Foo());
        }

        [Fact]
        public void Should_Apply_InjectionMembers()
        {
            IUnityContainer container = new UnityContainer().AddNewExtension<DecoratorExtension>();

            container.RegisterType<IGenericInterface<int>, ImplementationOfGenericInterface>("1");
            container.RegisterType<IGenericInterface<int>, DecoratorForGenericInterface>("1", new InjectionProperty("Bar", 21));

            var actual = container.Resolve<IGenericInterface<int>>("1");

            Assert.Equal(42 * 2, actual.Foo());

            var decorator = Assert.IsType<DecoratorForGenericInterface>(actual);

            Assert.Equal(21, decorator.Bar);
        }
    }
}
