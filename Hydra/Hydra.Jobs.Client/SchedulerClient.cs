namespace Hydra.Jobs.Client
{
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Threading.Tasks;

    public class SchedulerClient : ClientBase<ISchedulerClient>, ISchedulerClient
    {
        public SchedulerClient(Binding binding, EndpointAddress remoteAddress)
            : base(binding, remoteAddress)
        {
        }

        public async Task<JobScheduleResponse> Schedule(SimpleJobScheduleRequest jobSchedule)
        {
            return await this.Channel.Schedule(jobSchedule);
        }
    }
}
