using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TecX.Unity.TypedFactory.Test
{
    [TestClass]
    public class When_UsingFactoryWithParameterToCreateInstance : Given_ContainerWithBarRegistration
    {
        protected override void When()
        {
            this.foo = this.factory.Create("some name");
        }

        [TestMethod]
        public void Then_ParameterIsUsed()
        {
            Assert.IsNotNull(this.foo);
            Assert.AreEqual("some name", this.foo.Name);
        }
    }
}
