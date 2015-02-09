namespace Hydra.Jobs.Server
{
    using System;
    using System.Diagnostics.Contracts;
    using Quartz;

    public class SchedulerService : ISchedulerService
    {
        private readonly IScheduler scheduler;

        public SchedulerService(IScheduler scheduler)
        {
            Contract.Requires(scheduler != null);

            this.scheduler = scheduler;
        }

        public JobScheduleResponse Schedule(SimpleJobScheduleRequest jobSchedule)
        {
            IJobDetail job = JobBuilder
                .Create()
                .OfType(jobSchedule.Job.JobType)
                .RequestRecovery(jobSchedule.RequestRecovery)
                .StoreDurably(jobSchedule.StoreDurably)
                .Build();

            ITrigger trigger = TriggerBuilder
                .Create()
                .WithSchedule(SimpleScheduleBuilder.Create()
                    .WithInterval(TimeSpan.FromTicks(jobSchedule.IntervalInTicks)))
                .StartAt(jobSchedule.StartAt)
                .Build();

            DateTimeOffset nextExecutionAt = this.scheduler.ScheduleJob(job, trigger);

            return new JobScheduleResponse { NextExecutionAt = nextExecutionAt };
        }
    }
}
