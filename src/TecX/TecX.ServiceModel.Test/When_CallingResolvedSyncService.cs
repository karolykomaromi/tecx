namespace TecX.ServiceModel.Test
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.ServiceModel.AutoMagic;
    using TecX.ServiceModel.Test.TestClasses;

    [TestClass]
    public class When_CallingResolvedSyncService : Given_DiscoverableServiceHost
    {
        private ISyncService service;

        private string response;

        protected override void Given()
        {
            base.Given();

            this.container.RegisterForAutoDiscovery<ISyncService>();

            this.service = this.container.Resolve<ISyncService>();
        }

        protected override void When()
        {
            this.response = this.service.DoWork("input");
        }

        [TestMethod]
        public void Then_ServiceReturnsExpectedValue()
        {
            Assert.AreEqual("Service called", response);
        }
    }
}