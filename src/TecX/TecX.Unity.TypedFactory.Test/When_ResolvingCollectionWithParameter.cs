using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TecX.Unity.TypedFactory.Test.TestObjects;

namespace TecX.Unity.TypedFactory.Test
{
    [TestClass]
    public class When_ResolvingCollectionWithParameter : Given_FooAndSnafuRegistration
    {
        private ICollection<IFoo> resolved;

        protected override void Given()
        {
            base.Given();

            container.RegisterType<IFoo, Bar>("Bar");
        }

        protected override void When()
        {
            resolved = _factory.CreateCollection("1");
        }

        [TestMethod]
        public void Then_ReturnsBarResolvedUsingAdditionalParameter()
        {
            Assert.AreEqual(3, resolved.Count);

            Bar bar = resolved.OfType<Bar>().Single();

            Assert.AreEqual("1", bar.Name);
        }
    }
}