namespace Hydra.Jobs.Server
{
    using System;
    using System.Runtime.Serialization;
    using Hydra.Jobs.Transfer;

    [DataContract]
    public class CronTrigger : Trigger
    {
        [DataMember]
        public string CronExpressionString { get; set; }

        [DataMember]
        public TimeZoneInfo TimeZone { get; set; }
    }
}