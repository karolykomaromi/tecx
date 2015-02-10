namespace Hydra.Jobs.Test
{
    using System;
    using System.Collections.Generic;
    using Quartz;
    using Quartz.Impl.Matchers;
    using Quartz.Spi;

    public class NullScheduler : IScheduler
    {
        public string SchedulerName
        {
            get { return string.Empty; }
        }

        public string SchedulerInstanceId
        {
            get { return string.Empty; }
        }

        public SchedulerContext Context
        {
            get
            {
                return null;
            }
        }

        public bool InStandbyMode
        {
            get { return true; }
        }

        public bool IsShutdown
        {
            get { return true; }
        }

        public IJobFactory JobFactory
        {
            set { }
        }

        public IListenerManager ListenerManager
        {
            get { return null; }
        }

        public bool IsStarted
        {
            get { return false; }
        }

        public bool IsJobGroupPaused(string groupName)
        {
            return true;
        }

        public bool IsTriggerGroupPaused(string groupName)
        {
            return true;
        }

        public SchedulerMetaData GetMetaData()
        {
            return null;
        }

        public IList<IJobExecutionContext> GetCurrentlyExecutingJobs()
        {
            return new IJobExecutionContext[0];
        }

        public IList<string> GetJobGroupNames()
        {
            return new string[0];
        }

        public IList<string> GetTriggerGroupNames()
        {
            return new string[0];
        }

        public Quartz.Collection.ISet<string> GetPausedTriggerGroups()
        {
            return new Quartz.Collection.HashSet<string>();
        }

        public void Start()
        {
        }

        public void StartDelayed(TimeSpan delay)
        {
        }

        public void Standby()
        {
        }

        public void Shutdown()
        {
        }

        public void Shutdown(bool waitForJobsToComplete)
        {
        }

        public DateTimeOffset ScheduleJob(IJobDetail jobDetail, ITrigger trigger)
        {
            return DateTimeOffset.MaxValue;
        }

        public DateTimeOffset ScheduleJob(ITrigger trigger)
        {
            return DateTimeOffset.MaxValue;
        }

        public void ScheduleJobs(IDictionary<IJobDetail, Quartz.Collection.ISet<ITrigger>> triggersAndJobs, bool replace)
        {
        }

        public void ScheduleJob(IJobDetail jobDetail, Quartz.Collection.ISet<ITrigger> triggersForJob, bool replace)
        {
        }

        public bool UnscheduleJob(TriggerKey triggerKey)
        {
            return true;
        }

        public bool UnscheduleJobs(IList<TriggerKey> triggerKeys)
        {
            return true;
        }

        public DateTimeOffset? RescheduleJob(TriggerKey triggerKey, ITrigger newTrigger)
        {
            return null;
        }

        public void AddJob(IJobDetail jobDetail, bool replace)
        {
        }

        public void AddJob(IJobDetail jobDetail, bool replace, bool storeNonDurableWhileAwaitingScheduling)
        {
        }

        public bool DeleteJob(JobKey jobKey)
        {
            return true;
        }

        public bool DeleteJobs(IList<JobKey> jobKeys)
        {
            return true;
        }

        public void TriggerJob(JobKey jobKey)
        {
        }

        public void TriggerJob(JobKey jobKey, JobDataMap data)
        {
        }

        public void PauseJob(JobKey jobKey)
        {
        }

        public void PauseJobs(GroupMatcher<JobKey> matcher)
        {
        }

        public void PauseTrigger(TriggerKey triggerKey)
        {
        }

        public void PauseTriggers(GroupMatcher<TriggerKey> matcher)
        {
        }

        public void ResumeJob(JobKey jobKey)
        {
        }

        public void ResumeJobs(GroupMatcher<JobKey> matcher)
        {
        }

        public void ResumeTrigger(TriggerKey triggerKey)
        {
        }

        public void ResumeTriggers(GroupMatcher<TriggerKey> matcher)
        {
        }

        public void PauseAll()
        {
        }

        public void ResumeAll()
        {
        }

        public Quartz.Collection.ISet<JobKey> GetJobKeys(GroupMatcher<JobKey> matcher)
        {
            return new Quartz.Collection.HashSet<JobKey>();
        }

        public IList<ITrigger> GetTriggersOfJob(JobKey jobKey)
        {
            return new ITrigger[0];
        }

        public Quartz.Collection.ISet<TriggerKey> GetTriggerKeys(GroupMatcher<TriggerKey> matcher)
        {
            return new Quartz.Collection.HashSet<TriggerKey>();
        }

        public IJobDetail GetJobDetail(JobKey jobKey)
        {
            return null;
        }

        public ITrigger GetTrigger(TriggerKey triggerKey)
        {
            return null;
        }

        public TriggerState GetTriggerState(TriggerKey triggerKey)
        {
            return TriggerState.None;
        }

        public void AddCalendar(string calName, ICalendar calendar, bool replace, bool updateTriggers)
        {
        }

        public bool DeleteCalendar(string calName)
        {
            return true;
        }

        public ICalendar GetCalendar(string calName)
        {
            return null;
        }

        public IList<string> GetCalendarNames()
        {
            return new string[0];
        }

        public bool Interrupt(JobKey jobKey)
        {
            return true;
        }

        public bool Interrupt(string fireInstanceId)
        {
            return true;
        }

        public bool CheckExists(JobKey jobKey)
        {
            return false;
        }

        public bool CheckExists(TriggerKey triggerKey)
        {
            return false;
        }

        public void Clear()
        {
        }
    }
}