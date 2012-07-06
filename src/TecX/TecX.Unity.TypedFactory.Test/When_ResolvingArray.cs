namespace TecX.Unity.TypedFactory.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.TypedFactory.Test.TestObjects;

    [TestClass]
    public class When_ResolvingArray : Given_FooAndSnafuRegistration
    {
        private IFoo[] resolved;

        protected override void When()
        {
            this.resolved = this.Factory.CreateArray();
        }

        [TestMethod]
        public void Then_ReturnsFooAndSnafu()
        {
            Assert.AreEqual(2, resolved.Length);
        }
    }
}