using System;
using System.ServiceModel;
using System.ServiceModel.Discovery;

using Microsoft.Practices.Unity;

using Moq;

using TecX.ServiceModel.AutoMagic;
using TecX.ServiceModel.Test.TestClasses;

namespace TecX.ServiceModel.Test
{
    /// <summary>
    /// Helper methods to support UnitTesting
    /// </summary>
    internal static class TestHelper
    {
        #region Constants

        #endregion Constants

        ////////////////////////////////////////////////////////////

        /// <summary>
        /// Configures the DI container to use WCF Discovery when resolving <see cref="IAsyncService"/>
        /// </summary>
        public static IUnityContainer ConfigureContainerForRealService()
        {
            IUnityContainer container = new UnityContainer();

            //add the custom extension to the DI container
            container.AddNewExtension<WcfProxyContainerExtension>();

            //tell the extension to take care of calls that ask for resolutions of the type IAsyncService
            container.Configure<IWcfProxyConfiguration>().RegisterType<IAsyncService>();

            return container;
        }

        /// <summary>
        /// Configures the DI container to use a mock object when resolving <see cref="IAsyncService"/>
        /// </summary>
        public static IUnityContainer ConfigureContainerForMockService()
        {
            const string expectedData = "MockService called";

            IAsyncResult asyncResult = new Mock<IAsyncResult>().Object;

            var asyncServiceMock = new Mock<IAsyncService>();

            asyncServiceMock
                //expected call
                .Setup(asm => asm.BeginDoWork("input", It.IsAny<AsyncCallback>(), 0))
                //what to do when the method is called
                .Callback((string input, AsyncCallback callback, object state) =>
                              {
                                  //simulate callback at the end of the operation
                                  callback(asyncResult);
                              })
                //return prepared result
                .Returns(asyncResult);

            asyncServiceMock
                //expected call
                .Setup(asm => asm.EndDoWork(It.IsAny<IAsyncResult>()))
                //return prepared result
                .Returns(expectedData);

            //register the mock object with the DI container
            IUnityContainer container = new UnityContainer();
            container.RegisterInstance(asyncServiceMock.Object);
            return container;
        }

        /// <summary>
        /// Configures a <see cref="ServiceHost"/> to serve an endpoint with contract <see cref="ISyncService"/>
        /// and make it discoverable by adding a <see cref="ServiceDiscoveryBehavior"/> and a 
        /// <see cref="UdpDiscoveryEndpoint"/>
        /// </summary>
        public static ServiceHost SetupHostForDiscoverableSyncService()
        {
            ServiceHost host = new ServiceHost(typeof(SyncService), Constants.BaseAddress);

            //configure service host
            host.AddServiceEndpoint(typeof(ISyncService), new NetTcpBinding(SecurityMode.None), String.Empty);

            host.Description.Behaviors.Add(new ServiceDiscoveryBehavior());

            host.AddServiceEndpoint(new UdpDiscoveryEndpoint());
            return host;
        }
    }
}