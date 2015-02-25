namespace Hydra.Features.Jobs
{
    using System.Collections.Generic;
    using Hydra.Infrastructure.Mediator;
    using Quartz;

    public class ScheduledJobsRequest : IRequest<IEnumerable<IJobDetail>>
    {
    }
}