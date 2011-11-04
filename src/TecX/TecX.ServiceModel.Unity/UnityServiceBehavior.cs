using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.ServiceModel.Unity
{
    public class UnityServiceBehavior : IServiceBehavior
    {
        private readonly IUnityContainer _container;

        public UnityServiceBehavior(IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

            _container = container;
        }

        public IUnityContainer Container
        {
            get
            {
                return this._container;
            }
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            Guard.AssertNotNull(serviceHostBase, "serviceHostBase");
            Guard.AssertNotNull(serviceHostBase.ChannelDispatchers, "serviceHostBase.ChannelDispatchers");

            for (int dispatcherIndex = 0; dispatcherIndex < serviceHostBase.ChannelDispatchers.Count; dispatcherIndex++)
            {
                ChannelDispatcherBase dispatcher = serviceHostBase.ChannelDispatchers[dispatcherIndex];
                ChannelDispatcher channelDispatcher = (ChannelDispatcher)dispatcher;

                for (int endpointIndex = 0; endpointIndex < channelDispatcher.Endpoints.Count; endpointIndex++)
                {
                    EndpointDispatcher endpointDispatcher = channelDispatcher.Endpoints[endpointIndex];

                    endpointDispatcher.DispatchRuntime.InstanceProvider = new UnityInstanceProvider(Container, serviceDescription.ServiceType);
                }
            }
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, 
            ServiceHostBase serviceHostBase,
            Collection<ServiceEndpoint> endpoints,
            BindingParameterCollection bindingParameters)
        {
            /* intentionally left blank */
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            /* intentionally left blank */
        }
    }
}