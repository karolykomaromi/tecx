namespace TecX.Unity.ContextualBinding.Test
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.ContextualBinding.Test.TestObjects;

    [TestClass]
    public class When_RegisteringInstanceAfterDefaultMapping : Given_Instance
    {
        protected override void When()
        {
            container.RegisterType<IMyInterface, MyOtherClass>();

            container.RegisterInstance<IMyInterface>(new MyParameterLessClass(), (bindingContext, builderContext) => false);
        }

        [TestMethod]
        public void Then_CanResolveDefault()
        {
            MyOtherClass myOtherClass = container.Resolve<IMyInterface>() as MyOtherClass;

            Assert.IsNotNull(myOtherClass);
        }
    }
}