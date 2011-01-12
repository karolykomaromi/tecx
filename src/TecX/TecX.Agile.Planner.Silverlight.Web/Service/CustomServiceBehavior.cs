using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Web;

using TecX.Agile.Peer;
using TecX.Agile.Push;
using TecX.Common;

namespace TecX.Agile.Planner.Silverlight.Web.Service
{
    public class CustomServiceBehavior : IServiceBehavior
    {
        private readonly ISocketServer _socketServer;
        private readonly IPeerClient _peerClient;
        private readonly Type _serviceType;
        private readonly CustomInstanceProvider _instanceProvider;

        public CustomServiceBehavior(ISocketServer socketServer, IPeerClient peerClient, Type serviceType)
        {
            Guard.AssertNotNull(socketServer, "socketServer");
            Guard.AssertNotNull(peerClient, "peerClient");
            Guard.AssertNotNull(serviceType, "serviceType");

            _socketServer = socketServer;
            _peerClient = peerClient;
            _serviceType = serviceType;

            _instanceProvider = new CustomInstanceProvider(socketServer, peerClient, serviceType);
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            Guard.AssertNotNull(serviceHostBase, "serviceHostBase");
            Guard.AssertNotNull(serviceHostBase.ChannelDispatchers, "serviceHostBase.ChannelDispatchers");

            foreach (ChannelDispatcherBase channelDispatcherBase in serviceHostBase.ChannelDispatchers)
            {
                ChannelDispatcher channelDispatcher = channelDispatcherBase as ChannelDispatcher;

                if (channelDispatcher != null)
                {
                    foreach (EndpointDispatcher endpointDispatcher in channelDispatcher.Endpoints)
                    {
                        endpointDispatcher.DispatchRuntime.InstanceProvider = _instanceProvider;
                    }
                }
            }
        }
    }
}