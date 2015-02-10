namespace Hydra.Features.Jobs
{
    using Hydra.Commands;
    using Hydra.Jobs.Client;

    public class ScheduleJobCommand : ICommand<JobScheduleResponse>
    {
        public KnownJobs Job { get; set; }
    }
}