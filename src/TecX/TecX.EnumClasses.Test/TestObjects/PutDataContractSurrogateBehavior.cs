namespace TecX.EnumClasses.Tests.TestObjects
{
    using System;
    using System.Collections.ObjectModel;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;

    public class PutDataContractSurrogateBehavior : Attribute, IServiceBehavior
    {
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ServiceEndpoint endpoint in serviceDescription.Endpoints)
            {
                foreach (OperationDescription operation in endpoint.Contract.Operations)
                {
                    DataContractSerializerOperationBehavior behavior = operation.Behaviors.Find<DataContractSerializerOperationBehavior>();

                    if (behavior == null)
                    {
                        behavior = new DataContractSerializerOperationBehavior(operation) { DataContractSurrogate = new EnumerationClassesSurrogate() };

                        operation.Behaviors.Add(behavior);
                    }
                    else
                    {
                        behavior.DataContractSurrogate = new EnumerationClassesSurrogate();
                    }
                }
            }
        }
    }
}