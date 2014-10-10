namespace Hydra.Queries
{
    using System.Collections.Generic;
    using Quartz;

    public class ScheduledJobs : IQuery<IEnumerable<IJobDetail>>
    {
    }
}