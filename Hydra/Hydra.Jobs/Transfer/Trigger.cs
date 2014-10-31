namespace Hydra.Jobs.Transfer
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public abstract class Trigger
    {
        [DataMember]
        public TriggerKey Key { get; set; }

        [DataMember]
        public JobKey JobKey { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string CalendarName { get; set; }

        [DataMember]
        public DateTimeOffset? FinalFireTimeUtc { get; set; }

        [DataMember]
        public int MisfireInstruction { get; set; }

        [DataMember]
        public DateTimeOffset? EndTimeUtc { get; set; }

        [DataMember]
        public DateTimeOffset StartTimeUtc { get; set; }

        [DataMember]
        public int Priority { get; set; }

        [DataMember]
        public bool HasMillisecondPrecision { get; set; }
    }
}