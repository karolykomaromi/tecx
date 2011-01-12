using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

using TecX.Agile.Peer;
using TecX.Agile.Push;
using TecX.Common;

namespace TecX.Agile.Planner.Silverlight.Web.Service
{
    public class CustomServiceHost : ServiceHost
    {
        private readonly ISocketServer _socketServer;
        private readonly IPeerClient _peerClient;
        private readonly Type _serviceType;

        public CustomServiceHost(ISocketServer socketServer, IPeerClient peerClient, Type serviceType, 
            params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
            Guard.AssertNotNull(socketServer, "socketServer");
            Guard.AssertNotNull(peerClient, "peerClient");
            Guard.AssertNotNull(serviceType, "serviceType");

            _socketServer = socketServer;
            _peerClient = peerClient;
            _serviceType = serviceType;
        }

        protected override void OnOpening()
        {
            CustomServiceBehavior behavior = new CustomServiceBehavior(_socketServer, _peerClient, _serviceType);

            Description.Behaviors.Add(behavior);

            base.OnOpening();
        }
    }
}