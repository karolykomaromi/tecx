namespace Hydra.Features.Jobs
{
    using Hydra.Infrastructure.Mediator;
    using Hydra.Jobs.Client;

    public class ScheduleJobCommand : IRequest<JobScheduleResponse>
    {
        public KnownJobs Job { get; set; }
    }
}