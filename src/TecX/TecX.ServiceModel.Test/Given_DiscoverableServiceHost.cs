namespace TecX.ServiceModel.Test
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Discovery;

    using Microsoft.Practices.Unity;

    using TecX.ServiceModel.Test.TestClasses;
    using TecX.TestTools;

    public abstract class Given_DiscoverableServiceHost : ArrangeActAssert
    {
        protected ServiceHost host;

        protected IUnityContainer container;

        protected override void Arrange()
        {
            this.host = new ServiceHost(typeof(SyncService), Constants.BaseAddress);

            // configure service host
            this.host.AddServiceEndpoint(typeof(ISyncService), new NetTcpBinding(SecurityMode.None), string.Empty);

            this.host.Description.Behaviors.Add(new ServiceDiscoveryBehavior());

            this.host.AddServiceEndpoint(new UdpDiscoveryEndpoint());

            this.host.Open();

            this.container = new UnityContainer();
        }

        protected override void Teardown()
        {
            if (this.host != null)
            {
                ((IDisposable)this.host).Dispose();
            }
        }
    }
}