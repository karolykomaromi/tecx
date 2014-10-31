namespace Hydra.Jobs.Server
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class JobDetail
    {
        [DataMember]
        public JobKey Key { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public Type JobType { get; set; }

        [DataMember]
        public bool Durable { get; set; }

        [DataMember]
        public bool PersistJobDataAfterExecution { get; set; }

        [DataMember]
        public bool ConcurrentExecutionDisallowed { get; set; }

        [DataMember]
        public bool RequestsRecovery { get; set; }
    }
}