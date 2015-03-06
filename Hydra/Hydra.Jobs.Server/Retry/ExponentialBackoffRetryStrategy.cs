namespace Hydra.Jobs.Server.Retry
{
    using System;
    using System.Diagnostics.Contracts;
    using Quartz;

    public class ExponentialBackoffRetryStrategy : IRetryStrategy
    {
        /// <summary>
        /// Retries
        /// </summary>
        private const string Retries = "Retries";

        private readonly IRetrySettings settings;

        public ExponentialBackoffRetryStrategy(IRetrySettings settings)
        {
            Contract.Requires(settings != null);

            this.settings = settings;
        }

        public bool ShouldRetry(IJobExecutionContext context)
        {
            int retries = GetAlreadyPerformedRetries(context);

            return retries < this.settings.MaxRetries;
        }

        public ITrigger GetTrigger(IJobExecutionContext context)
        {
            int retries = GetAlreadyPerformedRetries(context);

            long factor = (long)Math.Pow(2, retries);

            TimeSpan backoff = new TimeSpan(this.settings.BackoffBaseInterval.Ticks * factor);

            ITrigger trigger = TriggerBuilder.Create()
                .StartAt(DateTimeOffset.UtcNow + backoff)
                .WithSimpleSchedule(x => x.WithRepeatCount(0))
                .WithIdentity(context.Trigger.Key)
                .ForJob(context.JobDetail)
                .Build();

            context.JobDetail.JobDataMap[Retries] = ++retries;

            return trigger;
        }

        private static int GetAlreadyPerformedRetries(IJobExecutionContext context)
        {
            int retries = 0;
            object o;
            if (context.JobDetail.JobDataMap.TryGetValue(Retries, out o) && o is int)
            {
                retries = (int)o;
            }

            return retries;
        }
    }
}