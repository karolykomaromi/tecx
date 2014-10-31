namespace Hydra.Jobs.Server
{
    using System;
    using System.Runtime.Serialization;
    using Quartz;

    [DataContract]
    public class CalendarIntervalTrigger : Trigger
    {
        [DataMember]
        public IntervalUnit RepeatIntervalUnit { get; set; }

        [DataMember]
        public int RepeatInterval { get; set; }

        [DataMember]
        public int TimesTriggered { get; set; }

        [DataMember]
        public TimeZoneInfo TimeZone { get; set; }

        [DataMember]
        public bool PreserveHourOfDayAcrossDaylightSavings { get; set; }

        [DataMember]
        public bool SkipDayIfHourDoesNotExist { get; set; }
    }
}