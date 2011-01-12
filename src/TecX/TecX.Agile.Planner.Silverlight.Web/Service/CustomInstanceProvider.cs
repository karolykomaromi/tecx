using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Web;

using TecX.Agile.Peer;
using TecX.Agile.Push;
using TecX.Common;

namespace TecX.Agile.Planner.Silverlight.Web.Service
{
    public class CustomInstanceProvider : IInstanceProvider
    {
        private readonly ISocketServer _socketServer;
        private readonly IPeerClient _peerClient;
        private readonly Type _serviceType;

        public CustomInstanceProvider(ISocketServer socketServer, IPeerClient peerClient, Type serviceType)
        {
            Guard.AssertNotNull(socketServer, "socketServer");
            Guard.AssertNotNull(peerClient, "peerClient");
            Guard.AssertNotNull(serviceType, "serviceType");

            _socketServer = socketServer;
            _peerClient = peerClient;
            _serviceType = serviceType;
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return GetInstance(instanceContext, null);
        }

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            object serviceInstance = new SilverlightPlanningService(_socketServer, _peerClient);

            return serviceInstance;
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
        }
    }
}