namespace Hydra.Jobs.Transfer
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class SimpleTrigger : Trigger
    {
        [DataMember]
        public int RepeatCount { get; set; }

        [DataMember]
        public TimeSpan RepeatInterval { get; set; }

        [DataMember]
        public int TimesTriggered { get; set; }
    }
}