namespace Hydra.Jobs.Server.Retry
{
    using System.Diagnostics.Contracts;
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

                context.Scheduler.UnscheduleJob(context.Trigger.Key);

                context.Scheduler.ScheduleJob(context.JobDetail, trigger);
            }
        }

        private static bool JobFailed(JobExecutionException jobException)
        {
            return jobException != null;
        }
    }
}