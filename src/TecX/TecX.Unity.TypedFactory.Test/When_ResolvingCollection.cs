using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Unity.TypedFactory.Test.TestObjects;

namespace TecX.Unity.TypedFactory.Test
{
    [TestClass]
    public class When_ResolvingCollection : Given_FooAndSnafuRegistration
    {
        private ICollection<IFoo> resolved;

        protected override void When()
        {
            resolved = _factory.CreateCollection();
        }

        [TestMethod]
        public void Then_ReturnsFooAndSnafu()
        {
            Assert.AreEqual(2, resolved.Count());
        }
    }
}