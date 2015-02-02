namespace Hydra.Jobs.Transfer
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public abstract class Trigger
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Group { get; set; }

        [DataMember]
        public string JobName { get; set; }

        [DataMember]
        public string JobGroup { get; set; }
        
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