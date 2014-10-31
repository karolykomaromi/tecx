namespace Hydra.Jobs.Transfer
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class SchedulerMetaData
    {
        [DataMember]
        public string SchedulerName { get; set; }

        [DataMember]
        public string SchedulerInstanceId { get; set; }

        [DataMember]
        public Type SchedulerType { get; set; }

        [DataMember]
        public bool SchedulerRemote { get; set; }

        [DataMember]
        public bool Started { get; set; }

        [DataMember]
        public bool InStandbyMode { get; set; }

        [DataMember]
        public bool Shutdown { get; set; }

        [DataMember]
        public Type JobStoreType { get; set; }

        [DataMember]
        public Type ThreadPoolType { get; set; }

        [DataMember]
        public int ThreadPoolSize { get; set; }

        [DataMember]
        public string Version { get; set; }

        [DataMember]
        public DateTimeOffset? RunningSince { get; set; }

        [DataMember]
        public int NumberOfJobsExecuted { get; set; }

        [DataMember]
        public bool JobStoreSupportsPersistence { get; set; }

        [DataMember]
        public bool JobStoreClustered { get; set; }
    }
}