using Quartz;

namespace Hydra.Features.Jobs
{
    public class ScheduleViewModel
    {
        public int Interval { get; set; }

        public IntervalUnit IntervalUnit { get; set; }

        public JobSchedule Schedule { get; set; }
    }
}