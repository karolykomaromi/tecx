namespace TecX.Unity.ContextualBinding.Test
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.ContextualBinding.Test.TestObjects;

    [TestClass]
    public class When_RegisteringContextualAfterDefaultMapping : Given_ContainerWithContextualBindingExtension
    {
        protected override void When()
        {
            container.RegisterType<IMyInterface, MyOtherClass>();

            container.RegisterType<IMyInterface, MyClass>((bindingContext, builderContext) => false);
        }

        [TestMethod]
        public void Then_CanStillResolveDefaultMapping()
        {
            MyOtherClass myOtherClass = container.Resolve<IMyInterface>() as MyOtherClass;

            Assert.IsNotNull(myOtherClass);
        }
    }
}