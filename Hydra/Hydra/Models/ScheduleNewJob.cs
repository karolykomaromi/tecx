namespace Hydra.Models
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Quartz;

    public class ScheduleNewJob
    {
        public int Interval { get; set; }

        public IntervalUnit IntervalUnit { get; set; }
    }
}