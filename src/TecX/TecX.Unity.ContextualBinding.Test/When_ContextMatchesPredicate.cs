namespace TecX.Unity.ContextualBinding.Test
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.ContextualBinding.Test.TestObjects;

    [TestClass]
    public class When_ContextMatchesPredicate : Given_ContainerWithContextualBindingExtension
    {
        protected override void When()
        {
            container.RegisterType<IMyInterface, MyParameterLessClass>((bindingContext, builderContext) => true);
        }

        [TestMethod]
        public void Then_ResolvesProperly()
        {
            MyParameterLessClass myClass = container.Resolve<IMyInterface>() as MyParameterLessClass;

            Assert.IsNotNull(myClass);
        }
    }
}