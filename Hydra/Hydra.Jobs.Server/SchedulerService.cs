namespace Hydra.Jobs.Server
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Quartz;

    public class SchedulerService : ISchedulerService
    {
        private readonly IScheduler scheduler;

        public SchedulerService(IScheduler scheduler)
        {
            Contract.Requires(scheduler != null);

            this.scheduler = scheduler;
        }

        public bool IsJobGroupPaused(string groupName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(groupName));

            return this.scheduler.IsJobGroupPaused(groupName);
        }

        public bool IsTriggerGroupPaused(string groupName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(groupName));

            return this.scheduler.IsTriggerGroupPaused(groupName);
        }

        public SchedulerMetaData GetMetaData()
        {
            return SchedulerMetaData.FromQuartz(this.scheduler.GetMetaData());
        }

        public string[] GetJobGroupNames()
        {
            return this.scheduler.GetJobGroupNames().ToArray();
        }

        public string[] GetTriggerGroupNames()
        {
            return this.scheduler.GetTriggerGroupNames().ToArray();
        }

        public string[] GetPausedTriggerGroups()
        {
            return this.scheduler.GetPausedTriggerGroups().ToArray();
        }

        public void Start(TimeSpan delay)
        {
            if (delay == TimeSpan.Zero)
            {
                this.scheduler.Start();
            }
            else
            {
                this.scheduler.StartDelayed(delay);
            }
        }

        public void Standby()
        {
            this.scheduler.Standby();
        }

        public void Shutdown(bool waitForJobsToComplete)
        {
            this.scheduler.Shutdown(waitForJobsToComplete);
        }

        public DateTimeOffset ScheduleJob(JobDetail jobDetail, Trigger trigger)
        {
            Contract.Requires(jobDetail != null);
            Contract.Requires(trigger != null);

            return this.scheduler.ScheduleJob(jobDetail.ToQuartz(), trigger.ToQuartz());
        }

        public bool UnscheduleJobs(TriggerKey[] triggerKeys)
        {
            Contract.Requires(triggerKeys != null);

            return this.scheduler.UnscheduleJobs(triggerKeys.Select(tk => tk.ToQuartz()).ToList());
        }

        public DateTimeOffset? RescheduleJob(TriggerKey triggerKey, Trigger newTrigger)
        {
            Contract.Requires(triggerKey != null);
            Contract.Requires(newTrigger != null);

            return this.scheduler.RescheduleJob(triggerKey.ToQuartz(), newTrigger.ToQuartz());
        }

        public void AddJob(JobDetail jobDetail, bool replace, bool storeNonDurableWhileAwaitingScheduling)
        {
            Contract.Requires(jobDetail != null);

            this.scheduler.AddJob(jobDetail.ToQuartz(), replace, storeNonDurableWhileAwaitingScheduling);
        }

        public bool DeleteJobs(JobKey[] jobKeys)
        {
            Contract.Requires(jobKeys != null);

            return this.scheduler.DeleteJobs(jobKeys.Select(jk => jk.ToQuartz()).ToList());
        }

        public void TriggerJob(JobKey jobKey)
        {
            Contract.Requires(jobKey != null);

            this.scheduler.TriggerJob(jobKey.ToQuartz());
        }

        public void PauseJob(JobKey jobKey)
        {
            Contract.Requires(jobKey != null);

            this.scheduler.PauseJob(jobKey.ToQuartz());
        }

        public void PauseTrigger(TriggerKey triggerKey)
        {
            Contract.Requires(triggerKey != null);

            this.scheduler.PauseTrigger(triggerKey.ToQuartz());
        }

        public void ResumeJob(JobKey jobKey)
        {
            Contract.Requires(jobKey != null);

            this.scheduler.ResumeJob(jobKey.ToQuartz());
        }

        public void ResumeTrigger(TriggerKey triggerKey)
        {
            Contract.Requires(triggerKey != null);

            this.scheduler.ResumeTrigger(triggerKey.ToQuartz());
        }

        public Trigger[] GetTriggersOfJob(JobKey jobKey)
        {
            Contract.Requires(jobKey != null);

            return this.scheduler.GetTriggersOfJob(jobKey.ToQuartz())
                .Select(Trigger.FromQuartz)
                .ToArray();
        }

        public JobDetail GetJobDetail(JobKey jobKey)
        {
            Contract.Requires(jobKey != null);

            return JobDetail.FromQuartz(this.scheduler.GetJobDetail(jobKey.ToQuartz()));
        }

        public Trigger GetTrigger(TriggerKey triggerKey)
        {
            Contract.Requires(triggerKey != null);

            return Trigger.FromQuartz(this.scheduler.GetTrigger(triggerKey.ToQuartz()));
        }

        public TriggerState GetTriggerState(TriggerKey triggerKey)
        {
            Quartz.TriggerState ts = this.scheduler.GetTriggerState(triggerKey.ToQuartz());

            switch (ts)
            {
                case Quartz.TriggerState.Normal:
                    return TriggerState.Normal;
                case Quartz.TriggerState.Paused:
                    return TriggerState.Paused;
                case Quartz.TriggerState.Complete:
                    return TriggerState.Complete;
                case Quartz.TriggerState.Error:
                    return TriggerState.Error;
                case Quartz.TriggerState.Blocked:
                    return TriggerState.Blocked;
                case Quartz.TriggerState.None:
                    return TriggerState.None;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool Interrupt(JobKey jobKey)
        {
            Contract.Requires(jobKey != null);

            return this.scheduler.Interrupt(jobKey.ToQuartz());
        }

        public bool CheckExists(JobKey jobKey)
        {
            Contract.Requires(jobKey != null);

            return this.scheduler.CheckExists(jobKey.ToQuartz());
        }

        public bool CheckExists(TriggerKey triggerKey)
        {
            Contract.Requires(triggerKey != null);

            return this.scheduler.CheckExists(triggerKey.ToQuartz());
        }
    }
}
