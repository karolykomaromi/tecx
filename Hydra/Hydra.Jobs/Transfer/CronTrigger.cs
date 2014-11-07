namespace Hydra.Jobs.Transfer
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class CronTrigger : Trigger
    {
        [DataMember]
        public string CronExpressionString { get; set; }

        [DataMember]
        public TimeZoneInfo TimeZone { get; set; }
    }
}