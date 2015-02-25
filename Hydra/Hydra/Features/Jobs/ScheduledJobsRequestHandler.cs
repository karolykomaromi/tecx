namespace Hydra.Features.Jobs
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Hydra.Infrastructure.Mediator;
    using Quartz;

    public class ScheduledJobsRequestHandler : IRequestHandler<ScheduledJobsRequest, IEnumerable<IJobDetail>>
    {
        public Task<IEnumerable<IJobDetail>> Handle(ScheduledJobsRequest request)
        {
            throw new NotImplementedException();
        }
    }
}