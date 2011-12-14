namespace TecX.ServiceModel.Test
{
    using System;
    using System.Threading;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.ServiceModel.Test.TestClasses;

    [TestClass]
    public class When_CallingMockService : Given_MockAsyncService
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

            this.service = this.container.Resolve<IAsyncService>();
        }

        protected override void When()
        {
            this.result = this.service.BeginDoWork(
                "input",
                ar =>
                    {
                        string fromService = this.service.EndDoWork(ar);

                        Assert.AreEqual("MockService called", fromService);

                        this.callbackOccured = true;

                        this.manualResetEvent.Set();
                    },
                0);

            this.manualResetEvent.WaitOne(50, false);
        }

        [TestMethod]
        public void Then_BehaviorIsSameAsWithRealService()
        {
            Assert.IsTrue(this.callbackOccured);
            Assert.IsNotNull(this.result);
        }
    }
}