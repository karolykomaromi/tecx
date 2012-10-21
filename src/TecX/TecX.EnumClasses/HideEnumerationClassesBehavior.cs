namespace TecX.EnumClasses
{
    using System;
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;

    public class HideEnumerationClassesBehavior : Attribute, IServiceBehavior
    {
        private readonly IDataContractSurrogate surrogate;

        public HideEnumerationClassesBehavior()
        {
            this.surrogate = new EnumerationClassesSurrogate();
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase host)
        {
            foreach (ServiceEndpoint endpoint in serviceDescription.Endpoints)
            {
                foreach (OperationDescription operation in endpoint.Contract.Operations)
                {
                    DataContractSerializerOperationBehavior behavior = operation.Behaviors.Find<DataContractSerializerOperationBehavior>();

                    if (behavior == null)
                    {
                        behavior = new DataContractSerializerOperationBehavior(operation) { DataContractSurrogate = this.surrogate };

                        operation.Behaviors.Add(behavior);
                    }
                    else
                    {
                        behavior.DataContractSurrogate = this.surrogate;
                    }
                }
            }

            this.HideEnumerationClassesFromMetadata(host);
        }

        private void HideEnumerationClassesFromMetadata(ServiceHostBase host)
        {
            ServiceMetadataBehavior smb = host.Description.Behaviors.Find<ServiceMetadataBehavior>();

            if (smb == null)
            {
                return;
            }

            WsdlExporter exporter = smb.MetadataExporter as WsdlExporter;

            if (exporter == null)
            {
                return;
            }

            object dataContractExporter;
            XsdDataContractExporter xsdInventoryExporter;
            if (!exporter.State.TryGetValue(typeof(XsdDataContractExporter), out dataContractExporter))
            {
                xsdInventoryExporter = new XsdDataContractExporter(exporter.GeneratedXmlSchemas);
            }
            else
            {
                xsdInventoryExporter = (XsdDataContractExporter)dataContractExporter;
            }

            exporter.State.Add(typeof(XsdDataContractExporter), xsdInventoryExporter);

            if (xsdInventoryExporter.Options == null)
            {
                xsdInventoryExporter.Options = new ExportOptions();
            }

            xsdInventoryExporter.Options.DataContractSurrogate = this.surrogate;
        }
    }
}