namespace TecX.Unity.TypedFactory.Test
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.TypedFactory.Test.TestObjects;

    [TestClass]
    public class When_ResolvingEnumerable : Given_FooAndSnafuRegistration
    {
        private IEnumerable<IFoo> resolved;

        protected override void Act()
        {
            resolved = this.Factory.CreateEnumerable();
        }

        [TestMethod]
        public void Then_ReturnsFooAndSnafu()
        {
            Assert.AreEqual(2, resolved.Count());
        }
    }
}