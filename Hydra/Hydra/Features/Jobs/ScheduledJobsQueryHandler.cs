namespace Hydra.Features.Jobs
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Hydra.Queries;
    using Quartz;

    public class ScheduledJobsQueryHandler : IQueryHandler<ScheduledJobsQuery, IEnumerable<IJobDetail>>
    {
        public Task<IEnumerable<IJobDetail>> Handle(ScheduledJobsQuery query)
        {
            throw new NotImplementedException();
        }
    }
}