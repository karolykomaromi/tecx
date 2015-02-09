namespace Hydra.Jobs.Client
{
    using System;
    using System.Runtime.Serialization;

    [DataContract(Namespace = Constants.ServiceNamespace)]
    public class SimpleJobScheduleRequest : JobScheduleRequest
    {
        [DataMember]
        public bool RequestRecovery { get; set; }

        [DataMember]
        public bool StoreDurably { get; set; }

        [DataMember]
        public long IntervalInTicks { get; set; }

        [DataMember]
        public DateTimeOffset StartAt { get; set; }

        [DataMember]
        public KnownJobs Job { get; set; }
    }
}