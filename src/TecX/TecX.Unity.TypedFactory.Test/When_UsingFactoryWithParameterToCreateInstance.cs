namespace TecX.Unity.TypedFactory.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class When_UsingFactoryWithParameterToCreateInstance : Given_ContainerWithBarRegistration
    {
        protected override void Act()
        {
            this.Foo = this.Factory.Create("some name");
        }

        [TestMethod]
        public void Then_ParameterIsUsed()
        {
            Assert.IsNotNull(this.Foo);
            Assert.AreEqual("some name", this.Foo.Name);
        }
    }
}
