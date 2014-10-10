namespace Hydra.Queries
{
    using System;
    using System.Collections.Generic;
    using Quartz;

    public class ScheduledJobsHandler : IQueryHandler<ScheduledJobs, IEnumerable<IJobDetail>>
    {
        public IEnumerable<IJobDetail> Handle(ScheduledJobs query)
        {
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