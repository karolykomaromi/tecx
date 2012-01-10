namespace TecX.ServiceModel.AutoMagic
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Discovery;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;

    using TecX.Common;
    using TecX.Common.Extensions.Error;
    using TecX.Common.Extensions.Primitives;

    /// <summary>
    /// Utility methods for WCF
    /// </summary>
    public static class WcfServiceHelper
    {
        /// <summary>
        /// Creates the WCF channel proxy through reflection
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <param name="address">The address</param>
        /// <param name="binding">The binding.</param>
        /// <returns></returns>
        public static object CreateWcfChannelProxy(Type contract, EndpointAddress address, Binding binding)
        {
            Guard.AssertNotNull(contract, "contract");
            Guard.AssertNotNull(binding, "binding");
            Guard.AssertNotNull(address, "address");

            // get the open type for the channelfactory
            Type channelFactoryType = typeof(ChannelFactory<>);

            // make the generic open type a specific type and thus tell the factory for which contract
            // it will be used
            channelFactoryType = channelFactoryType.MakeGenericType(contract);

            // create the concrete factory and hand in the binding and address to the constructor
            object channelFactory = Activator.CreateInstance(channelFactoryType, binding, address);

            // get the method that creates the channel
            MethodInfo createchannel = channelFactory.GetType().GetMethod("CreateChannel", new Type[0]);

            return createchannel.Invoke(channelFactory, null);
        }

        public static object CreateWcfChannelProxy(Type contract, string endpointConfigurationName)
        {
            Guard.AssertNotNull(contract, "contract");
            Guard.AssertNotNull(endpointConfigurationName, "endpointConfigurationName");

            // get the open type for the channelfactory
            Type channelFactoryType = typeof(ChannelFactory<>);

            // make the generic open type a specific type and thus tell the factory for which contract
            // it will be used
            channelFactoryType = channelFactoryType.MakeGenericType(contract);

            // create the concrete factory and hand in the name of the endpoint configuration to the constructor
            object channelFactory = Activator.CreateInstance(channelFactoryType, endpointConfigurationName);

            // get the method that creates the channel
            MethodInfo createchannel = channelFactory.GetType().GetMethod("CreateChannel", new Type[0]);

            return createchannel.Invoke(channelFactory, null);
        }

        /// <summary>
        /// Creates the WCF channel proxy through reflection using WCF auto-discovery
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <param name="scopes">The Scopes used to identify the correct endpoint for auto discovery. 
        /// <seealso cref="FindCriteria.Scopes"/></param>
        /// <returns></returns>
        public static object CreateWcfChannelProxy(Type contract, params Uri[] scopes)
        {
            Guard.AssertNotNull(contract, "contract");

            Collection<Uri> uris = scopes == null ? new Collection<Uri>() : new Collection<Uri>(scopes);

            ServiceInfo serviceInfo;

            if (TryGetServiceInfoFromDiscoveryMetadata(contract, uris, out serviceInfo))
            {
                return CreateWcfChannelProxy(contract, serviceInfo.Address, serviceInfo.Binding);
            }

            if (TryGetServiceInfoFromMex(contract, out serviceInfo))
            {
                return CreateWcfChannelProxy(contract, serviceInfo.Address, serviceInfo.Binding);
            }

            throw new ProxyCreationFailedException("Could not find a suitable service via WCF Discovery or MEX");
        }

        /// <summary>
        /// Extracts metadata information from an endpoint and converts the information to xml
        /// </summary>
        /// <param name="endpoint">The endpoint from which information should be extracted</param>
        /// <returns>Endpoint metadata as xml</returns>
        public static XElement GetServiceMetadataAsXml(ServiceEndpoint endpoint)
        {
            Guard.AssertNotNull(endpoint, "endpoint");

            // get all metadata like the bindings, contract and address from the endpoint...
            MetadataSet endpointMetadata = WcfServiceHelper.ExportEndpointMetadata(endpoint);

            // ...convert it to xml and add the xml to the metadata the DiscoveryClient will receive
            if (endpointMetadata != null)
            {
                XElement endpointMetadataExtension = WcfServiceHelper.ConvertServiceMetadataToXml(endpointMetadata);

                return endpointMetadataExtension;
            }

            throw new MetadataExportFailedException("Could not export service metadata for endpoint")
                .WithAdditionalInfo("endpoint", endpoint);
        }

        /// <summary>
        /// Tries to get (A)ddress and (B)inding for an endpoint from <see cref="EndpointDiscoveryMetadata"/>
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <param name="scopes">The scopes of the endpoints to look for. <see cref="FindCriteria.Scopes"/></param>
        /// <param name="serviceInfo">The contract info.</param>
        /// <returns><c>true</c> if (A)ddress and (B)inding could be retrieved; <c>false</c> otherwise</returns>
        private static bool TryGetServiceInfoFromDiscoveryMetadata(Type contract, Collection<Uri> scopes, out ServiceInfo serviceInfo)
        {
            using (DiscoveryClient discoveryClient = new DiscoveryClient(new UdpDiscoveryEndpoint()))
            {
                FindCriteria criteria = new FindCriteria(contract) { Duration = 2.Seconds() };

                if (scopes != null && scopes.Count > 0)
                {
                    foreach (Uri scope in scopes)
                    {
                        criteria.Scopes.Add(scope);
                    }
                }

                FindResponse response = discoveryClient.Find(criteria);

                Collection<EndpointDiscoveryMetadata> availableServices = response.Endpoints;

                if (availableServices != null &&
                    availableServices.Count > 0)
                {
                    // the first available service is used (no guarantees about order or special features)
                    EndpointDiscoveryMetadata svc = availableServices[0];

                    XElement element = svc.Extensions.Elements(Constants.EndpointMetadataExtensionName).FirstOrDefault();
                    if (element != null)
                    {
                        string wsdl = element.Value;

                        StringReader stringReader = new StringReader(wsdl);

                        XmlReader reader = XmlReader.Create(stringReader);

                        MetadataSet metadata = MetadataSet.ReadFrom(reader);

                        if (TryGetServiceInfoFromMetadataSet(metadata, contract, out serviceInfo))
                        {
                            return true;
                        }
                    }
                }
            }

            serviceInfo = null;
            return false;
        }

        /// <summary>
        /// (A)ddress and (B)inding info from service metadata
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <param name="contract">Contract the endpoint must implement</param>
        /// <param name="serviceInfo">The service info.</param>
        /// <returns><c>true</c> if address and binding could be retrieved; <c>false</c> otherwise</returns>
        private static bool TryGetServiceInfoFromMetadataSet(MetadataSet metadata, Type contract, out ServiceInfo serviceInfo)
        {
            MetadataImporter importer = new WsdlImporter(metadata);

            ServiceEndpointCollection endpoints = importer.ImportAllEndpoints();

            if (endpoints.Count > 0)
            {
                // only use the metadata of a service that offers the requested contract
                ServiceEndpoint endpoint = endpoints.FirstOrDefault();

                if (endpoint != null)
                {
                    serviceInfo = new ServiceInfo(endpoint.Address, endpoint.Binding);
                    return true;
                }
            }

            serviceInfo = null;
            return false;
        }

        /// <summary>
        /// Gets the endpoint metadata in wsdl format
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <returns>WSDL xml document with endpoint metadata</returns>
        private static MetadataSet ExportEndpointMetadata(ServiceEndpoint endpoint)
        {
            MetadataExporter exporter = new WsdlExporter();

            exporter.ExportEndpoint(endpoint);

            if (exporter.Errors != null &&
                exporter.Errors.Count > 0)
            {
                throw new MetadataExportFailedException("Exporting endpoint metadata failed")
                    .WithAdditionalInfo("metadataConversionErrors", exporter.Errors);
            }

            MetadataSet metadata = exporter.GetGeneratedMetadata();

            return metadata;
        }

        /// <summary>
        /// Converts the <paramref name="metadata"/> to xml that can be added to the 
        /// <see cref="EndpointDiscoveryBehavior.Extensions"/>
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The endpoint metadata in wsdl format</returns>
        private static XElement ConvertServiceMetadataToXml(MetadataSet metadata)
        {
            StringBuilder sb = new StringBuilder(1024);

            XmlWriter writer = XmlWriter.Create(sb);

            metadata.WriteTo(writer);

            string wsdl = sb.ToString();

            XElement element = new XElement(Constants.EndpointMetadataExtensionName) { Value = wsdl };

            XElement root = new XElement("root", element);

            return root;
        }

        /// <summary>
        /// Tries to the get the (A)ddress and (B)inding info from a mex endpoint
        /// </summary>
        /// <param name="contract">The contract type.</param>
        /// <param name="serviceInfo">The service info.</param>
        /// <returns><c>true</c> if address and binding could be retrieved; <c>false</c> otherwise</returns>
        private static bool TryGetServiceInfoFromMex(Type contract, out ServiceInfo serviceInfo)
        {
            using (DiscoveryClient discoveryClient = new DiscoveryClient(new UdpDiscoveryEndpoint()))
            {
                // define search criteria that identify a MetadataExchangeEndpoint
                FindCriteria criteria = FindCriteria.CreateMetadataExchangeEndpointCriteria();
                criteria.MaxResults = 1;
                criteria.Duration = 2.Seconds();

                FindResponse discovered = discoveryClient.Find(criteria);

                discoveryClient.Close();

                CustomBinding binding = new CustomBinding(new HttpTransportBindingElement());

                MetadataExchangeClient mexClient = new MetadataExchangeClient(binding);

                MetadataSet metadata = mexClient.GetMetadata(discovered.Endpoints[0].Address);

                if (TryGetServiceInfoFromMetadataSet(metadata, contract, out serviceInfo))
                {
                    return true;
                }
            }

            serviceInfo = null;
            return false;
        }
    }
}