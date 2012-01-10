using System.Collections.Generic;
using System.Linq;

using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Unity.TypedFactory.Test.TestObjects;

namespace TecX.Unity.TypedFactory.Test
{
    [TestClass]
    public class When_ResolvingEnumerable : Given_FooAndSnafuRegistration
    {
        private IEnumerable<IFoo> resolved;

        protected override void When()
        {
            resolved = _factory.CreateEnumerable();
        }

        [TestMethod]
        public void Then_ReturnsFooAndSnafu()
        {
            Assert.AreEqual(2, resolved.Count());
        }
    }
}