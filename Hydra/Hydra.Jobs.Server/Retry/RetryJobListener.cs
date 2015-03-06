namespace Hydra.Jobs.Server.Retry
{
    using System;
    using System.Diagnostics.Contracts;
    using Hydra.Infrastructure.Logging;
    using Quartz;
    using Quartz.Listener;

    public class RetryJobListener : JobListenerSupport
    {
        private readonly IRetryStrategy retryStrategy;

        public RetryJobListener(IRetryStrategy retryStrategy)
        {
            Contract.Requires(retryStrategy != null);

            this.retryStrategy = retryStrategy;
        }

        public override string Name
        {
            get { return "Retry"; }
        }

        public override void JobToBeExecuted(IJobExecutionContext context)
        {
        }

        public override void JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException)
        {
            if (JobFailed(jobException) && this.retryStrategy.ShouldRetry(context))
            {
                ITrigger trigger = this.retryStrategy.GetTrigger(context);

                bool unscheduled = context.Scheduler.UnscheduleJob(context.Trigger.Key);

                DateTimeOffset nextRunAt = context.Scheduler.ScheduleJob(context.JobDetail, trigger);

                HydraEventSource.Log.JobScheduledForRetry(context.JobDetail.Key, context.Trigger.Key, nextRunAt);
            }
            else
            {
                HydraEventSource.Log.JobFinallyFailed(context.JobDetail.Key);
            }
        }

        private static bool JobFailed(JobExecutionException jobException)
        {
            return jobException != null;
        }
    }
}