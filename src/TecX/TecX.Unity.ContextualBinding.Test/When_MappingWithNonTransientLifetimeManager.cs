namespace TecX.Unity.ContextualBinding.Test
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.ContextualBinding.Test.TestObjects;

    [TestClass]
    public class When_MappingWithNonTransientLifetimeManager : Given_BuilderAndContainerWithContextualBindingExtension
    {
        private IMyInterface first, second;

        protected override void When()
        {
            container.RegisterType<IMyInterface, MyParameterLessClass>(new ContainerControlledLifetimeManager(), request => true);

            first = container.Resolve<IMyInterface>();

            second = container.Resolve<IMyInterface>();
        }

        [TestMethod]
        public void Then_ResolvesWithCorrectLifetime()
        {
            Assert.AreSame(first, second);
        }
    }
}