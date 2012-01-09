using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Unity.TypedFactory.Test.TestObjects;

namespace TecX.Unity.TypedFactory.Test
{
    [TestClass]
    public class When_ResolvingList : Given_FooAndSnafuRegistration
    {
        private IList<IFoo> resolved;

        protected override void When()
        {
            resolved = _factory.CreateList();
        }

        [TestMethod]
        public void Then_ReturnsFooAndSnafu()
        {
            Assert.AreEqual(2, resolved.Count());
        }
    }
}