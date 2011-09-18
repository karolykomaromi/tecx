using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TecX.Unity.Enrichment;
using TecX.Unity.Test.TestObjects;

namespace TecX.Unity.Test
{
    [TestClass]
    public class EnrichmentFixture
    {
        [TestMethod]
        public void CanEnrichCreatedObject()
        {
            var container = new UnityContainer();

            container.AddNewExtension<EnrichmentExtension>();

            container.RegisterType<IFoo, Snafu>(new Enrichment<Snafu>((s, context) => s.IsEnriched = true));

            var snafu = container.Resolve<IFoo>() as Snafu;

            Assert.IsNotNull(snafu);
            Assert.IsTrue(snafu.IsEnriched);
        }
    }
}
