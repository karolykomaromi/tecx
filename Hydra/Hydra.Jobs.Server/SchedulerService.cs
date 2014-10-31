namespace Hydra.Jobs.Server
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using AutoMapper;
    using Quartz;

    public class SchedulerService : ISchedulerService
    {
        private readonly IScheduler scheduler;

        private readonly IMappingEngine mapper;

        public SchedulerService(IScheduler scheduler, IMappingEngine mapper)
        {
            Contract.Requires(scheduler != null);
            Contract.Requires(mapper != null);

            this.scheduler = scheduler;
            this.mapper = mapper;
        }

        public bool IsJobGroupPaused(string groupName)
        {
            return this.scheduler.IsJobGroupPaused(groupName);
        }

        public bool IsTriggerGroupPaused(string groupName)
        {
            return this.scheduler.IsTriggerGroupPaused(groupName);
        }

        public SchedulerMetaData GetMetaData()
        {
            SchedulerMetaData meta = this.mapper.Map<Quartz.SchedulerMetaData, SchedulerMetaData>(this.scheduler.GetMetaData());

            return meta;
        }

        public string[] GetJobGroupNames()
        {
            IList<string> jobGroupNames = this.scheduler.GetJobGroupNames() ?? new List<string>();

            return jobGroupNames.ToArray();
        }

        public string[] GetTriggerGroupNames()
        {
            IList<string> triggerGroupNames = this.scheduler.GetTriggerGroupNames() ?? new List<string>();

            return triggerGroupNames.ToArray();
        }

        public string[] GetPausedTriggerGroups()
        {
            IEnumerable<string> pausedTriggerGroups = this.scheduler.GetPausedTriggerGroups() ?? Enumerable.Empty<string>();

            return pausedTriggerGroups.ToArray();
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
            IJobDetail jd = this.mapper.Map<JobDetail, Quartz.IJobDetail>(jobDetail);
            ITrigger t = this.mapper.Map<Trigger, ITrigger>(trigger);

            return this.scheduler.ScheduleJob(jd, t);
        }

        public bool UnscheduleJobs(TriggerKey[] triggerKeys)
        {
            return this.scheduler.UnscheduleJobs(triggerKeys.Select(this.mapper.Map<TriggerKey, Quartz.TriggerKey>).ToList());
        }

        public DateTimeOffset? RescheduleJob(TriggerKey triggerKey, Trigger newTrigger)
        {
            ITrigger nt = this.mapper.Map<Trigger, ITrigger>(newTrigger);

            Quartz.TriggerKey tk = this.mapper.Map<TriggerKey, Quartz.TriggerKey>(triggerKey);

            DateTimeOffset? rescheduledTo = this.scheduler.RescheduleJob(tk, nt);

            return rescheduledTo;
        }

        public void AddJob(JobDetail jobDetail, bool replace, bool storeNonDurableWhileAwaitingScheduling)
        {
            IJobDetail jd = this.mapper.Map<JobDetail, Quartz.IJobDetail>(jobDetail);

            this.scheduler.AddJob(jd, replace, storeNonDurableWhileAwaitingScheduling);
        }

        public bool DeleteJobs(JobKey[] jobKeys)
        {
            List<Quartz.JobKey> jks = jobKeys.Select(this.mapper.Map<JobKey, Quartz.JobKey>).ToList();

            return this.scheduler.DeleteJobs(jks);
        }

        public void TriggerJob(JobKey jobKey)
        {
            Quartz.JobKey jk = this.mapper.Map<JobKey, Quartz.JobKey>(jobKey);

            this.scheduler.TriggerJob(jk);
        }

        public void PauseJob(JobKey jobKey)
        {
            Quartz.JobKey jk = this.mapper.Map<JobKey, Quartz.JobKey>(jobKey);

            this.scheduler.PauseJob(jk);
        }

        public void PauseTrigger(TriggerKey triggerKey)
        {
            Quartz.TriggerKey tk = this.mapper.Map<TriggerKey, Quartz.TriggerKey>(triggerKey);

            this.scheduler.PauseTrigger(tk);
        }

        public void ResumeJob(JobKey jobKey)
        {
            Quartz.JobKey jk = this.mapper.Map<JobKey, Quartz.JobKey>(jobKey);

            this.scheduler.ResumeJob(jk);
        }

        public void ResumeTrigger(TriggerKey triggerKey)
        {
            Quartz.TriggerKey tk = this.mapper.Map<TriggerKey, Quartz.TriggerKey>(triggerKey);

            this.scheduler.ResumeTrigger(tk);
        }

        public Trigger[] GetTriggersOfJob(JobKey jobKey)
        {
            Quartz.JobKey jk = this.mapper.Map<JobKey, Quartz.JobKey>(jobKey);

            IList<ITrigger> triggersOfJob = this.scheduler.GetTriggersOfJob(jk) ?? new List<ITrigger>();

            return triggersOfJob
                .Select(this.mapper.Map<ITrigger, Trigger>)
                .ToArray();
        }

        public JobDetail GetJobDetail(JobKey jobKey)
        {
            Quartz.JobKey jk = this.mapper.Map<JobKey, Quartz.JobKey>(jobKey);

            JobDetail jd = this.mapper.Map<IJobDetail, JobDetail>(this.scheduler.GetJobDetail(jk));

            return jd;
        }

        public Trigger GetTrigger(TriggerKey triggerKey)
        {
            Quartz.TriggerKey tk = this.mapper.Map<TriggerKey, Quartz.TriggerKey>(triggerKey);

            Trigger t = this.mapper.Map<ITrigger, Trigger>(this.scheduler.GetTrigger(tk));

            return t;
        }

        public TriggerState GetTriggerState(TriggerKey triggerKey)
        {
            Quartz.TriggerKey tk = this.mapper.Map<TriggerKey, Quartz.TriggerKey>(triggerKey);

            TriggerState ts = this.scheduler.GetTriggerState(tk);

            return ts;
        }

        public bool Interrupt(JobKey jobKey)
        {
            Quartz.JobKey jk = this.mapper.Map<JobKey, Quartz.JobKey>(jobKey);

            bool interrupted = this.scheduler.Interrupt(jk);

            return interrupted;
        }

        public bool CheckExists(JobKey jobKey)
        {
            Quartz.JobKey jk = this.mapper.Map<JobKey, Quartz.JobKey>(jobKey);

            bool exists = this.scheduler.CheckExists(jk);

            return exists;
        }

        public bool CheckExists(TriggerKey triggerKey)
        {
            Quartz.TriggerKey tk = this.mapper.Map<TriggerKey, Quartz.TriggerKey>(triggerKey);

            bool exists = this.scheduler.CheckExists(tk);

            return exists;
        }
    }
}
