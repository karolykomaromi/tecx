namespace Hydra.Jobs.Server
{
    using System;
    using System.Diagnostics.Contracts;

    public static class SchedulerServiceExtensions
    {
        public static void Start(this ISchedulerService svc)
        {
            Contract.Requires(svc != null);

            svc.Start(TimeSpan.Zero);
        }

        public static void Shutdown(this ISchedulerService svc)
        {
            Contract.Requires(svc != null);

            svc.Shutdown(false);
        }

        public static void AddJob(this ISchedulerService svc, JobDetail jobDetail, bool replace)
        {
            Contract.Requires(svc != null);
            Contract.Requires(jobDetail != null);

            svc.AddJob(jobDetail, replace, false);
        }

        public static void PauseAll(this ISchedulerService svc)
        {
            Contract.Requires(svc != null);

            // pause triggers group matcher any
        }

        public static void ResumeAll(this ISchedulerService svc)
        {
            Contract.Requires(svc != null);

            // resume triggers group matcher any
        }

        public static bool UnscheduleJob(this ISchedulerService svc, TriggerKey triggerKey)
        {
            Contract.Requires(svc != null);
            Contract.Requires(triggerKey != null);

            return svc.UnscheduleJobs(new[] { triggerKey });
        }

        public static bool DeleteJob(this ISchedulerService svc, JobKey jobKey)
        {
            Contract.Requires(svc != null);
            Contract.Requires(jobKey != null);

            return svc.DeleteJobs(new[] { jobKey });
        }
    }
}