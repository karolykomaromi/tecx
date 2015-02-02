namespace TecX.Unity.ContextualBinding.Test
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.ContextualBinding.Test.TestObjects;

    [TestClass]
    public class When_ContextDoesNotMatchPredicate : Given_BuilderAndContainerWithContextualBindingExtension
    {
        protected override void Act()
        {
            container.RegisterType<IMyInterface, MyParameterLessClass>(request => false);
        }

        [TestMethod]
        [ExpectedException(typeof(ResolutionFailedException))]
        public void Then_Throws()
        {
            container.Resolve<IMyInterface>();
        }
    }
}