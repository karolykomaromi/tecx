namespace Hydra.Features.Jobs
{
    using Quartz;

    public class ScheduleViewModel
    {
        public int Interval { get; set; }

        public IntervalUnit IntervalUnit { get; set; }

        public JobSchedule Schedule { get; set; }
    }
}