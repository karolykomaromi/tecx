namespace TecX.Unity.ContextualBinding.Test
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.ContextualBinding.Test.TestObjects;

    [TestClass]
    public class When_RegisteringInstanceWithFailingPredicate : Given_Instance
    {
        protected override void When()
        {
            container.RegisterInstance<IMyInterface>(instance, (bindingContext, builderContext) => false);
        }

        [TestMethod]
        [ExpectedException(typeof(ResolutionFailedException))]
        public void Then_Throws()
        {
            container.Resolve<IMyInterface>();
        }
    }
}