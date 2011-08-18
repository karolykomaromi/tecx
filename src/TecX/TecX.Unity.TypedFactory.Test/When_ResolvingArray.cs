using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Unity.TypedFactory.Test.TestObjects;

namespace TecX.Unity.TypedFactory.Test
{
    [TestClass]
    public class When_ResolvingArray : Given_FooAndSnafuRegistration
    {
        private IFoo[] resolved;

        protected override void When()
        {
            resolved = _factory.CreateArray();
        }

        [TestMethod]
        public void Then_ReturnsFooAndSnafu()
        {
            Assert.AreEqual(2, resolved.Length);
        }
    }
}