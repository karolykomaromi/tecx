namespace Hydra.Features.Jobs
{
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;
    using Hydra.Commands;
    using Hydra.Jobs.Client;

    public class ScheduleJobCommandHandler : ICommandHandler<ScheduleJobCommand, object>
    {
        private readonly ISchedulerClient scheduler;

        public ScheduleJobCommandHandler(ISchedulerClient scheduler)
        {
            Contract.Requires(scheduler != null);

            this.scheduler = scheduler;
        }

        public async Task<object> Handle(ScheduleJobCommand command)
        {
            SimpleJobScheduleRequest request = new SimpleJobScheduleRequest { Job = command.Job };

            JobScheduleResponse response = await this.scheduler.Schedule(request);

            return response;
        }
    }
}