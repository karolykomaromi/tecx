namespace Hydra.Features.Jobs
{
    using System.Collections.Generic;
    using Hydra.Queries;
    using Quartz;

    public class ScheduledJobsQuery : IQuery<IEnumerable<IJobDetail>>
    {
    }
}