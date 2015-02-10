namespace Hydra.Features.Jobs
{
    using Hydra.Commands;
    using Hydra.Jobs.Client;

    public class ScheduleJobCommand : ICommand<object>
    {
        public KnownJobs Job { get; set; }
    }
}