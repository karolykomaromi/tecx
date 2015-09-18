namespace Hydra.Jobs.Server
{
    using System;
    using System.Diagnostics.Contracts;
    using Hydra.Infrastructure;
    using Hydra.Infrastructure.Logging;
    using Quartz;

    public static class HydraEventSourceExtensions
    {
        public static void JobScheduledForRetry(this HydraEventSource log, JobKey jobKey, TriggerKey triggerKey, DateTimeOffset nextRunAt)
        {
            Contract.Requires(log != null);
            Contract.Requires(jobKey != null);
            Contract.Requires(triggerKey != null);

            log.JobScheduledForRetry(jobKey.ToString(), triggerKey.ToString(), nextRunAt.ToString(FormatStrings.DateAndTime.RoundTrip));
        }

        public static void JobFinallyFailed(this HydraEventSource log, JobKey jobKey)
        {
            Contract.Requires(log != null);
            Contract.Requires(jobKey != null);

            log.JobFinallyFailed(jobKey.ToString());
        }
    }
}