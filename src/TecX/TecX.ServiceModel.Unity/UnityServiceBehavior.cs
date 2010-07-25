using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.ServiceModel.Unity
{
    /// <summary>
    /// A ServiceBehavior that allows 
    /// </summary>
    public class UnityServiceBehavior : IServiceBehavior
    {
        #region Fields

        private readonly UnityInstanceProvider _instanceProvider;

        #endregion Fields

        ////////////////////////////////////////////////////////////

        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityServiceBehavior"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="serviceType"></param>
        public UnityServiceBehavior(IUnityContainer container, Type serviceType)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotNull(serviceType, "serviceType");

            _instanceProvider = new UnityInstanceProvider(container, serviceType);
        }

        #endregion c'tor

        ////////////////////////////////////////////////////////////

        #region Implementation of IServiceBehavior

        /// <summary>
        /// Provides the ability to change run-time property values or insert custom extension objects such as error handlers, message or parameter interceptors, security extensions, and other custom extension objects.
        /// </summary>
        /// <param name="serviceDescription">The service description.</param>
        /// <param name="serviceHostBase">The host that is currently being built.</param>
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription,
                                          ServiceHostBase serviceHostBase)
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

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase,
                                         Collection<ServiceEndpoint> endpoints,
                                         BindingParameterCollection bindingParameters)
        { /* intentionally left blank */ }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        { /* intentionally left blank */ }

        #endregion Implementation of IServiceBehavior
    }
}