namespace Hydra.Jobs.Client
{
    using System;
    using System.Runtime.Serialization;

    [DataContract(Namespace = Constants.ServiceNamespace)]
    public class JobScheduleResponse : IExtensibleDataObject
    {
        public static readonly JobScheduleResponse Empty = new JobScheduleResponse();

        [DataMember]
        public DateTimeOffset NextExecutionAt { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }
}