namespace TecX.Unity.TypedFactory.Test
{
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.TypedFactory.Test.TestObjects;

    [TestClass]
    public class When_ResolvingCollection : Given_FooAndSnafuRegistration
    {
        private ICollection<IFoo> resolved;

        protected override void Act()
        {
            this.resolved = this.Factory.CreateCollection();
        }

        [TestMethod]
        public void Then_ReturnsFooAndSnafu()
        {
            Assert.AreEqual(2, resolved.Count);
        }
    }
}