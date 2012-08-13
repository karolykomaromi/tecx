namespace TecX.Unity.ContextualBinding.Test
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.ContextualBinding.Test.TestObjects;

    [TestClass]
    public class When_MappingWithInjectionMember : Given_BuilderAndContainerWithContextualBindingExtension
    {
        protected override void Act()
        {
            container.RegisterType<MyClass>(request => true, new InjectionConstructor("c'tor with parameter"));
        }

        [TestMethod]
        public void Then_ResolvesUsingSpecifiedInjectionMember()
        {
            var myClass = container.Resolve<MyClass>();

            Assert.AreEqual("c'tor with parameter", myClass.Str);
        }
    }
}