using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Threading;
using System.Web;

using TecX.Agile.Peer;
using TecX.Agile.Push;

namespace TecX.Agile.Planner.Silverlight.Web.Service
{
    public class CustomServiceHostFactory : ServiceHostFactory
    {
        private readonly Thread _socketServerThread;
        private readonly Thread _policyServerThread;

        private readonly ISocketServer _socketServer;
        private readonly PolicySocketServer _policySocketServer;
        private readonly IPeerClient _peerClient;

        public CustomServiceHostFactory()
        {
            _policySocketServer = new PolicySocketServer();
            _socketServer = new SocketServer();
            _peerClient = new PeerClient();

            _policyServerThread = new Thread(_policySocketServer.Start);
            _socketServerThread = new Thread(_socketServer.Start);
        }

        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            if(serviceType == typeof(SilverlightPlanningService))
            {
                return new CustomServiceHost(_socketServer, _peerClient, serviceType, baseAddresses);
            }

            return base.CreateServiceHost(serviceType, baseAddresses);
        }
    }
}