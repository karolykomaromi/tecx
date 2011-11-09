namespace TecX.Unity.ContextualBinding.Test
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.ContextualBinding.Test.TestObjects;

    [TestClass]
    public class When_RegisteringInstanceWithContext : Given_Instance
    {
        protected override void When()
        {
            container.RegisterInstance<IMyInterface>(instance, (bindingContext, builderContext) => true);
        }

        [TestMethod]
        public void Then_PullsInstanceOnMatch()
        {
            MyParameterLessClass myClass = container.Resolve<IMyInterface>() as MyParameterLessClass;

            Assert.AreSame(instance, myClass);
        }
    }
}