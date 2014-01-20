using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Microsoft.Practices.Unity;

namespace Modular.Web.Hosting
{
    public class UnityServiceBehavior : IServiceBehavior
    {
        private readonly IUnityContainer container;

        public UnityServiceBehavior(IUnityContainer container)
        {
            Contract.Requires(container != null);

            this.container = container;
        }

        public IUnityContainer Container
        {
            get
            {
                return this.container;
            }
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            Contract.Requires(serviceHostBase != null);
            Contract.Requires(serviceHostBase.ChannelDispatchers != null);

            for (int dispatcherIndex = 0; dispatcherIndex < serviceHostBase.ChannelDispatchers.Count; dispatcherIndex++)
            {
                ChannelDispatcherBase dispatcher = serviceHostBase.ChannelDispatchers[dispatcherIndex];
                ChannelDispatcher channelDispatcher = (ChannelDispatcher)dispatcher;

                for (int endpointIndex = 0; endpointIndex < channelDispatcher.Endpoints.Count; endpointIndex++)
                {
                    EndpointDispatcher endpointDispatcher = channelDispatcher.Endpoints[endpointIndex];

                    endpointDispatcher.DispatchRuntime.InstanceProvider = new UnityInstanceProvider(this.Container, serviceDescription.ServiceType);
                }
            }
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
            /* intentionally left blank */
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            /* intentionally left blank */
        }
    }
}