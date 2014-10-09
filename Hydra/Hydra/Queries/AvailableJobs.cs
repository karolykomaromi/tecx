namespace Hydra.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Hydra.Infrastructure;
    using Hydra.Models;
    using Quartz;

    public class AvailableJobs : IQuery<IEnumerable<JobInfo>>
    {
    }

    public class AvailableJobsHandler : IQueryHandler<AvailableJobs, IEnumerable<JobInfo>>
    {
        public IEnumerable<JobInfo> Handle(AvailableJobs query)
        {
            IEnumerable<Type> jobs = typeof(IQuery<>).Assembly.GetExportedTypes().Where(t => typeof(IJob).IsAssignableFrom(t));

            var jobInfos = jobs.Select(
                    j => new JobInfo
                        {
                            Name = StringHelper.SplitCamelCase(j.Name), 
                            Description = "Lorem ipsum..."
                        });

            return jobInfos;
        }
    }
}