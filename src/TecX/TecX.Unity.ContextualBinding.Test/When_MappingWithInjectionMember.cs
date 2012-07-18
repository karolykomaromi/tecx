namespace TecX.Unity.ContextualBinding.Test
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.ContextualBinding.Test.TestObjects;

    [TestClass]
    public class When_MappingWithInjectionMember : Given_BuilderAndContainerWithContextualBindingExtension
    {
        protected override void When()
        {
            container.RegisterType<IMyInterface, MyClass>(request => true, new InjectionConstructor("c'tor with parameter"));
        }

        [TestMethod]
        public void Then_ResolvesUsingSpecifiedInjectionMember()
        {
            var myClass = container.Resolve<IMyInterface>() as MyClass;

            Assert.IsNotNull(myClass);

            Assert.AreEqual("c'tor with parameter", myClass.Str);
        }
    }
}