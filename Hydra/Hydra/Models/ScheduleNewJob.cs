namespace Hydra.Models
{
    using Hydra.Jobs;
    using Quartz;

    public class ScheduleNewJob
    {
        public int Interval { get; set; }

        public IntervalUnit IntervalUnit { get; set; }

        public JobSchedule Schedule { get; set; }
    }
}