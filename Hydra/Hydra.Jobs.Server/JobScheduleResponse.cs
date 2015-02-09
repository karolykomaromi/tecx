namespace Hydra.Jobs.Server
{
    using System;
    using System.Runtime.Serialization;

    [DataContract(Namespace = Constants.ServiceNamespace)]
    public class JobScheduleResponse
    {
        [DataMember]
        public DateTimeOffset NextExecutionAt { get; set; }
    }
}