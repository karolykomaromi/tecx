namespace Hydra.Features.Jobs
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Hydra.Queries;
    using Quartz;
    using Quartz.Impl.Matchers;
    using Quartz.Spi;

    public class ScheduledJobsQueryHandler : IQueryHandler<ScheduledJobsQuery, IEnumerable<IJobDetail>>
    {
        private readonly IJobStore store;

        public ScheduledJobsQueryHandler(IJobStore store)
        {
            Contract.Requires(store != null);

            this.store = store;
        }

        public IEnumerable<IJobDetail> Handle(ScheduledJobsQuery query)
        {
            var jobKeysAndTriggers = this.store.GetJobKeys(GroupMatcher<JobKey>.AnyGroup()).Select(key => new { JobKey = key, Triggers = this.store.GetTriggersForJob(key) });
            
            return new[]
                   {
                       new DummyJobDetails("Foo", "Daily"),
                       new DummyJobDetails("Send Emails", "Daily"),
                       new DummyJobDetails("Baz", "Weekly")
                   };

            ////IEnumerable<Type> jobs = typeof(IQuery<>).Assembly.GetExportedTypes().Where(t => typeof(IJob).IsAssignableFrom(t));

            ////var jobInfos = jobs.Select(
            ////        j => new JobInfo
            ////            {
            ////                Name = StringHelper.SplitCamelCase(j.Name), 
            ////                Description = "Lorem ipsum...",
            ////                JobType = j
            ////            });

            ////return jobInfos;
        }

        private class DummyJobDetails : IJobDetail
        {
            public DummyJobDetails(string name, string @group)
            {
                this.Key = new JobKey(name, @group);
            }

            public JobKey Key { get; set; }

            public string Description { get; set; }

            public Type JobType { get; set; }

            public JobDataMap JobDataMap { get; set; }

            public bool Durable { get; set; }

            public bool PersistJobDataAfterExecution { get; set; }

            public bool ConcurrentExecutionDisallowed { get; set; }

            public bool RequestsRecovery { get; set; }

            public object Clone()
            {
                throw new NotImplementedException();
            }

            public JobBuilder GetJobBuilder()
            {
                throw new NotImplementedException();
            }
        }
    }
}