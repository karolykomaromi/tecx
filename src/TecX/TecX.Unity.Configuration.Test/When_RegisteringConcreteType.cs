namespace TecX.Unity.Configuration.Test
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.Configuration.Test.TestObjects;

    [TestClass]
    public class When_RegisteringConcreteType : Given_ContainerAndBuilder
    {
        protected override void Act()
        {
            base.Act();

            this.builder.ForConcreteType<Foo>();
        }

        [TestMethod]
        public void Then_ResolvesProperly()
        {
            var foo = container.Resolve<Foo>();

            Assert.IsNotNull(foo);
        }
    }
}
