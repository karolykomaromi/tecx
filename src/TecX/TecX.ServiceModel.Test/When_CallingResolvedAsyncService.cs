namespace TecX.ServiceModel.Test
{
    using System;
    using System.Threading;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.ServiceModel.AutoMagic;
    using TecX.ServiceModel.Test.TestClasses;

    [TestClass]
    public class When_CallingResolvedAsyncService : Given_DiscoverableServiceHost
    {
        private IAsyncService service;

        private bool callbackOccured;

        private ManualResetEvent manualResetEvent;

        private IAsyncResult result;

        protected override void Given()
        {
            base.Given();

            this.callbackOccured = false;

            this.manualResetEvent = new ManualResetEvent(false);

            this.container.RegisterForAutoDiscovery<IAsyncService>();

            this.service = this.container.Resolve<IAsyncService>();
        }

        protected override void When()
        {
            this.result = this.service.BeginDoWork(
                "input",
                ar =>
                    {
                        string fromService = service.EndDoWork(ar);
                        Assert.AreEqual("Service called", fromService);
                        callbackOccured = true;
                        manualResetEvent.Set();
                    },
                0);

            this.manualResetEvent.WaitOne(5000, false);
        }

        [TestMethod]
        public void Then_CallIsExecutedAsynchronously()
        {
            Assert.IsTrue(this.callbackOccured);
            Assert.IsNotNull(this.result);
        }
    }
}