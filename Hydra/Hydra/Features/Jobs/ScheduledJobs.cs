using System.Collections.Generic;
using Hydra.Queries;
using Quartz;

namespace Hydra.Features.Jobs
{
    public class ScheduledJobs : IQuery<IEnumerable<IJobDetail>>
    {
    }
}