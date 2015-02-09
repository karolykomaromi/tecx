namespace Hydra.Jobs.Client
{
    using System.Threading.Tasks;

    public class NullSchedulerClient : ISchedulerClient
    {
        public async Task<JobScheduleResponse> Schedule(SimpleJobScheduleRequest jobSchedule)
        {
            return await Task<JobScheduleResponse>.Factory.StartNew(() => JobScheduleResponse.Empty);
        }
    }
}