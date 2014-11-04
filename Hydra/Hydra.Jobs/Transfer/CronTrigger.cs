using System;
using System.Runtime.Serialization;

namespace Hydra.Jobs.Transfer
{
    [DataContract]
    public class CronTrigger : Trigger
    {
        [DataMember]
        public string CronExpressionString { get; set; }

        [DataMember]
        public TimeZoneInfo TimeZone { get; set; }
    }
}