using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TecX.Unity.TypedFactory.Test
{
    [TestClass]
    public class When_UsingFactoryWithParameterToCreateInstance : Given_ContainerWithBarRegistration
    {
        protected override void When()
        {
            _foo = _factory.Create("some name");
        }

        [TestMethod]
        public void Then_ParameterIsUsed()
        {
            Assert.IsNotNull(_foo);
            Assert.AreEqual("some name", _foo.Name);
        }
    }
}
