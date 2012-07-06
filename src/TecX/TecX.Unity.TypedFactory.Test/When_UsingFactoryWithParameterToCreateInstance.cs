using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TecX.Unity.TypedFactory.Test
{
    [TestClass]
    public class When_UsingFactoryWithParameterToCreateInstance : Given_ContainerWithBarRegistration
    {
        protected override void When()
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
