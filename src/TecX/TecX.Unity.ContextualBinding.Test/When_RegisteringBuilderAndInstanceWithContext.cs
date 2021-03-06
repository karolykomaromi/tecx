namespace TecX.Unity.ContextualBinding.Test
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.ContextualBinding.Test.TestObjects;

    [TestClass]
    public class When_RegisteringBuilderAndInstanceWithContext : Given_BuilderAndInstance
    {
        protected override void Act()
        {
            container.RegisterInstance<IMyInterface>(instance, request => true);
        }

        [TestMethod]
        public void Then_PullsInstanceOnMatch()
        {
            MyParameterLessClass myClass = container.Resolve<IMyInterface>() as MyParameterLessClass;

            Assert.AreSame(instance, myClass);
        }
    }
}