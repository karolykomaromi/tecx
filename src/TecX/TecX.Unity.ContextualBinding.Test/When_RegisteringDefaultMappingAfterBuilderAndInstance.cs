namespace TecX.Unity.ContextualBinding.Test
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.ContextualBinding.Test.TestObjects;

    [TestClass]
    public class When_RegisteringDefaultMappingAfterBuilderAndInstance : Given_BuilderAndInstance
    {
        protected override void When()
        {
            container.RegisterInstance<IMyInterface>(new MyClass(), (bindingContext, builderContext) => true);

            container.RegisterType<IMyInterface, MyOtherClass>();
        }

        [TestMethod]
        public void Then_CanResolveDefault()
        {
            MyClass myClass = container.Resolve<IMyInterface>() as MyClass;

            Assert.IsNotNull(myClass);
        }
    }
}
