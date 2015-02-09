namespace Hydra.Jobs.Server
{
    using System;
    using System.Runtime.Serialization;

    [DataContract(Namespace = Constants.ServiceNamespace)]
    public class JobScheduleResponse
    {
        public static readonly JobScheduleResponse Empty = new JobScheduleResponse();

        [DataMember]
        public DateTimeOffset NextExecutionAt { get; set; }
    }
}