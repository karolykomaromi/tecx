namespace TecX.ServiceModel.Unity.AutoMagic
{
    using System;
    using System.Collections.ObjectModel;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Discovery;
    using System.Xml.Linq;

    using TecX.Common;
    using TecX.Common.Extensions.Collections;

    /// <summary>
    /// Adds information about the binding used to the discovery information of an endpoint and makes
    /// the endoint discoverable by adding a <see cref="System.ServiceModel.Discovery.ServiceDiscoveryBehavior"/> if neccessary
    /// </summary>
    /// <remarks>The ServiceHost applies behaviors in the following order
    /// <list type="bullet">
    ///     <item>Contract</item>
    ///     <item>Operation</item>
    ///     <item>Endpoint</item>
    ///     <item>Service</item>
    /// </list>
    /// </remarks>
    public class BindingDiscoveryServiceBehavior : Attribute, IServiceBehavior
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BindingDiscoveryServiceBehavior"/> class
        /// </summary>
        /// <param name="scopes">A list of scopes </param>
        public BindingDiscoveryServiceBehavior(params string[] scopes)
        {
            this.Scopes = new Collection<Uri>();

            if (scopes != null)
            {
                foreach (string scope in scopes)
                {
                    Uri uri;

                    if (Uri.TryCreate(scope, UriKind.RelativeOrAbsolute, out uri))
                    {
                        this.Scopes.Add(uri);
                    }
                }
            }
        }

        public Collection<Uri> Scopes { get; private set; }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
            /* intentionally left blank */
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            Guard.AssertNotNull(serviceDescription, "serviceDescription");
            Guard.AssertNotNull(serviceHostBase, "serviceHostBase");

            // make the endpoints of a service discoverable by adding
            // the neccessary behavior
            ServiceEndpointCollection endpoints = serviceDescription.Endpoints;

            foreach (ServiceEndpoint endpoint in endpoints)
            {
                this.MakeEndpointDiscoverable(endpoint);
            }
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            /* intentionally left blank */
        }

        /// <summary>
        /// Adds an <see cref="System.ServiceModel.Discovery.EndpointDiscoveryBehavior"/> to an endpoint as well as the
        /// metadata of the endpoint to the <see cref="System.ServiceModel.Discovery.EndpointDiscoveryBehavior.Extensions"/>
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        private void MakeEndpointDiscoverable(ServiceEndpoint endpoint)
        {
            EndpointDiscoveryBehavior endpointDiscoveryBehavior = new EndpointDiscoveryBehavior();

            // if any scopes were defined add them to the endpoint
            endpointDiscoveryBehavior.Scopes.AddRange(this.Scopes);

            // add the behavior to the endpoint
            endpoint.Behaviors.Add(endpointDiscoveryBehavior);

            XElement endpointMetadataExtension = WcfServiceHelper.GetServiceMetadataAsXml(endpoint);

            // add the binding information to the endpoint
            endpointDiscoveryBehavior.Extensions.Add(endpointMetadataExtension);
        }
    }
}