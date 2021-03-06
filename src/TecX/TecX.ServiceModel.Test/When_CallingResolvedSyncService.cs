namespace TecX.ServiceModel.Test
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.ServiceModel.Test.TestClasses;
    using TecX.ServiceModel.Unity.AutoMagic;

    [TestClass]
    public class When_CallingResolvedSyncService : Given_DiscoverableServiceHost
    {
        private ISyncService service;

        private string response;

        protected override void Arrange()
        {
            base.Arrange();

            this.container.RegisterType<ISyncService>(new AutoDiscoveryProxyFactory());

            this.service = this.container.Resolve<ISyncService>();
        }

        protected override void Act()
        {
            this.response = this.service.DoWork("input");
        }

        [TestMethod]
        [TestCategory("LongRunning")]
        public void Then_ServiceReturnsExpectedValue()
        {
            Assert.AreEqual("Service called", response);
        }
    }
}