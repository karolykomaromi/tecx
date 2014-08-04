namespace TecX.ServiceModel.Test
{
    using System;
    using System.ServiceModel.Discovery;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.ServiceModel.Test.TestClasses;

    [TestClass]
    public class When_DiscoveringServiceDecoratedWithBindingDiscoveryServiceBehavior : Given_DiscoverableServiceHost
    {
        private FindResponse response;

        protected override void Act()
        {
            DiscoveryClient client = new DiscoveryClient(new UdpDiscoveryEndpoint());

            FindCriteria criteria = new FindCriteria(typeof(ISyncService))
                {
                    Duration = new TimeSpan(0, 0, 2)
                };

            this.response = client.Find(criteria);
        }

        [TestMethod]
        [TestCategory("LongRunning")]
        public void Then_FindsCorrectEndPoints()
        {
            Assert.AreEqual(1, this.response.Endpoints.Count);
            Assert.AreEqual(Constants.BaseAddress.ToString(), this.response.Endpoints[0].Address.ToString());
        }
    }
}