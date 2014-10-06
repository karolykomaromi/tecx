namespace TecX.Unity.ContextualBinding.Test
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.ContextualBinding.Test.TestObjects;

    [TestClass]
    public class When_RegisteringDefaultAfterContextualMapping : Given_BuilderAndContainerWithContextualBindingExtension
    {
        protected override void Act()
        {
            container.RegisterType<IMyInterface, MyParameterLessClass>(request => true);

            container.RegisterType<IMyInterface, MyOtherClass>();
        }

        [TestMethod]
        public void Then_ContextualMappingHasPrecedence()
        {
            MyParameterLessClass myParameterLessClass = container.Resolve<IMyInterface>() as MyParameterLessClass;

            Assert.IsNotNull(myParameterLessClass);
        }
    }
}