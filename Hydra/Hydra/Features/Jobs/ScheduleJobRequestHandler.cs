namespace Hydra.Features.Jobs
{
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;
    using Hydra.Infrastructure.Mediator;
    using Hydra.Jobs.Client;

    public class ScheduleJobRequestHandler : IRequestHandler<ScheduleJobCommand, JobScheduleResponse>
    {
        private readonly ISchedulerClient scheduler;

        public ScheduleJobRequestHandler(ISchedulerClient scheduler)
        {
            Contract.Requires(scheduler != null);

            this.scheduler = scheduler;
        }

        public async Task<JobScheduleResponse> Handle(ScheduleJobCommand command)
        {
            SimpleJobScheduleRequest request = new SimpleJobScheduleRequest { Job = command.Job };

            return await this.scheduler.Schedule(request);
        }
    }
}