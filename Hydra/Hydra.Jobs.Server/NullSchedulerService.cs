namespace Hydra.Jobs.Server
{
    public class NullSchedulerService : ISchedulerService
    {
        public JobScheduleResponse Schedule(SimpleJobScheduleRequest jobSchedule)
        {
            return JobScheduleResponse.Empty;
        }
    }
}