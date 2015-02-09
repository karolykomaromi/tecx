namespace Hydra.Jobs.Client
{
    using System;
    using System.Runtime.Serialization;

    [DataContract(Namespace = Constants.ServiceNamespace)]
    public class JobScheduleResponse : IExtensibleDataObject
    {
        [DataMember]
        public DateTimeOffset NextExecutionAt { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }
}