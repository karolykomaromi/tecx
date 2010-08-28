using System;
using System.ServiceModel;
using System.ServiceModel.Discovery;
using System.Threading;

using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.ServiceModel.AutoMagic;
using TecX.ServiceModel.Test.TestClasses;

namespace TecX.ServiceModel.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class WcfAutoMagicFixture
    {
        [TestMethod]
        public void CanAddDiscoveryBehaviorUsingAttribute()
        {
            using (ServiceHost host = new ServiceHost(typeof (SyncService), Constants.BaseAddress))
            {
                BindingDiscoveryServiceBehavior bindingDiscoveryServiceBehavior = 
                    host.Description.Behaviors.Find<BindingDiscoveryServiceBehavior>();

                Assert.IsNotNull(bindingDiscoveryServiceBehavior);
            }
        }

        [TestMethod]
        public void CanDiscoverServiceDecoratedWithBindingDiscoveryServiceBehavior()
        {
            ServiceHost host = TestHelper.SetupHostForDiscoverableSyncService();

            using (host)
            {
                host.Open();

                //start discovery
                DiscoveryClient client = new DiscoveryClient(new UdpDiscoveryEndpoint());

                FindCriteria criteria = new FindCriteria(typeof (ISyncService))
                                            {
                                                Duration = new TimeSpan(0, 0, 2)
                                            };

                FindResponse found = client.Find(criteria);

                Assert.AreEqual(1, found.Endpoints.Count);
                Assert.AreEqual(Constants.BaseAddress.ToString(), found.Endpoints[0].Address.ToString());
            }
        }

        [TestMethod]
        public void CanResolveAsyncServiceUsingUnity()
        {
            ServiceHost host = TestHelper.SetupHostForDiscoverableSyncService();

            using (host)
            {
                host.Open();

                //configure unity
                IUnityContainer container = TestHelper.ConfigureContainerForRealService();

                //resolve the service using IASyncService as the contract (!)
                IAsyncService service = container.Resolve<IAsyncService>();

                bool callbackOccured = false;

                ManualResetEvent manualEvent = new ManualResetEvent(false);

                IAsyncResult result = service.BeginDoWork("input", ar =>
                                                              {
                                                                  string fromService = service.EndDoWork(ar);
                                                                  Assert.AreEqual("Service called", fromService);
                                                                  callbackOccured = true;
                                                                  manualEvent.Set();
                                                              }, 0);

                manualEvent.WaitOne(5000, false);

                Assert.IsTrue(callbackOccured);
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void CanResolveSyncServiceUsingUnity()
        {
            ServiceHost host = TestHelper.SetupHostForDiscoverableSyncService();

            using (host)
            {
                host.Open();

                //configure unity
                IUnityContainer container = new UnityContainer();

                container.AddNewExtension<WcfProxyContainerExtension>();

                container.Configure<IWcfProxyConfiguration>().RegisterType<ISyncService>();

                //resolve and call service
                ISyncService service = container.Resolve<ISyncService>();

                string response = service.DoWork("input");

                Assert.AreEqual("Service called", response);
            }
        }

        [TestMethod]
        public void CanInjectMockService()
        {
            IUnityContainer container = TestHelper.ConfigureContainerForMockService();

            IAsyncService service = container.Resolve<IAsyncService>();

            bool callbackOccured = false;

            ManualResetEvent manualEvent = new ManualResetEvent(false);

            IAsyncResult result = service.BeginDoWork("input", ar =>
                                                          {
                                                              string fromService = service.EndDoWork(ar);

                                                              Assert.AreEqual("MockService called", fromService);

                                                              callbackOccured = true;

                                                              manualEvent.Set();
                                                          }, 0);

            manualEvent.WaitOne(50, false);

            Assert.IsTrue(callbackOccured);
            Assert.IsNotNull(result);
        }
    }
}