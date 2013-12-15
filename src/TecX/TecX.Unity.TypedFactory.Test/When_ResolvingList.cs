namespace TecX.Unity.TypedFactory.Test
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.TypedFactory.Test.TestObjects;

    [TestClass]
    public class When_ResolvingList : Given_FooAndSnafuRegistration
    {
        private IList<IFoo> resolved;

        protected override void Act()
        {
            this.resolved = this.Factory.CreateList();
        }

        [TestMethod]
        public void Then_ReturnsFooAndSnafu()
        {
            Assert.AreEqual(2, resolved.Count());
        }
    }
}