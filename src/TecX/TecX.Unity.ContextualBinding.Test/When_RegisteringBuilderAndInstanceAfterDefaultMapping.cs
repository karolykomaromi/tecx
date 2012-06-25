namespace TecX.Unity.ContextualBinding.Test
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.ContextualBinding.Test.TestObjects;

    [TestClass]
    public class When_RegisteringBuilderAndInstanceAfterDefaultMapping : Given_BuilderAndInstance
    {
        protected override void When()
        {
            container.RegisterType<IMyInterface, MyOtherClass>();

            container.RegisterInstance<IMyInterface>(new MyParameterLessClass(), request => false);
        }

        [TestMethod]
        public void Then_CanResolveDefault()
        {
            MyOtherClass myOtherClass = container.Resolve<IMyInterface>() as MyOtherClass;

            Assert.IsNotNull(myOtherClass);
        }
    }
}