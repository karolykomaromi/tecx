using AutoMapper;

namespace Hydra.Jobs.Server
{
    using System;
    using System.Runtime.Serialization;
    using System.ServiceModel;
    using Hydra.Infrastructure;
    using Quartz;

    // ISimpleTrigger
    // ICalendarIntervalTrigger
    // ICronTrigger
    // IDailyTimeIntervalTrigger

    [DataContract]
    public abstract class Trigger
    {
        [DataMember]
        public TriggerKey Key { get; set; }

        [DataMember]
        public JobKey JobKey { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string CalendarName { get; set; }

        [DataMember]
        public DateTimeOffset? FinalFireTimeUtc { get; set; }

        [DataMember]
        public int MisfireInstruction { get; set; }

        [DataMember]
        public DateTimeOffset? EndTimeUtc { get; set; }

        [DataMember]
        public DateTimeOffset StartTimeUtc { get; set; }

        [DataMember]
        public int Priority { get; set; }

        [DataMember]
        public bool HasMillisecondPrecision { get; set; }

        public static Trigger FromQuartz(ITrigger trigger)
        {
            Trigger t = null;

            new Switch(trigger)
                .Case<ISimpleTrigger>(simple =>
                    {
                        t = Mapper.Map<SimpleTrigger>(simple);
                    })
                .Case<ICalendarIntervalTrigger>(interval =>
                    {
                        t = Mapper.Map<CalendarIntervalTrigger>(interval);
                    })
                .Case<ICronTrigger>(cron =>
                    {
                        t = Mapper.Map<CronTrigger>(cron);
                    })
                .Default(o =>
                    {
                        throw new NotSupportedException();
                    });

            return t;
        }

        public abstract ITrigger ToQuartz();
    }

    [DataContract]
    public class SimpleTrigger : Trigger
    {
        [DataMember]
        public int RepeatCount { get; set; }

        [DataMember]
        public TimeSpan RepeatInterval { get; set; }

        [DataMember]
        public int TimesTriggered { get; set; }

        public override ITrigger ToQuartz()
        {
            throw new NotImplementedException();
        }
    }

    [DataContract]
    public class CronTrigger : Trigger
    {
        [DataMember]
        public string CronExpressionString { get; set; }

        [DataMember]
        public TimeZoneInfo TimeZone { get; set; }

        public override ITrigger ToQuartz()
        {
            throw new NotImplementedException();
        }
    }

    [DataContract]
    public class CalendarIntervalTrigger : Trigger
    {
        [DataMember]
        public IntervalUnit RepeatIntervalUnit { get; set; }

        [DataMember]
        public int RepeatInterval { get; set; }

        [DataMember]
        public int TimesTriggered { get; set; }

        [DataMember]
        public TimeZoneInfo TimeZone { get; set; }

        [DataMember]
        public bool PreserveHourOfDayAcrossDaylightSavings { get; set; }

        [DataMember]
        public bool SkipDayIfHourDoesNotExist { get; set; }

        public override ITrigger ToQuartz()
        {
            throw new NotImplementedException();
        }
    }

    [DataContract]
    public class JobDetail
    {
        public JobKey Key { get; set; }

        public string Description { get; set; }

        public Type JobType { get; set; }

        public bool Durable { get; set; }

        public bool PersistJobDataAfterExecution { get; set; }

        public bool ConcurrentExecutionDisallowed { get; set; }

        public bool RequestsRecovery { get; set; }

        public static JobDetail FromQuartz(IJobDetail jobDetail)
        {
            throw new NotImplementedException();
        }

        public IJobDetail ToQuartz()
        {
            // JobDetailImpl
            throw new NotImplementedException();
        }
    }

    [DataContract]
    public class TriggerKey
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Group { get; set; }

        public Quartz.TriggerKey ToQuartz()
        {
            return new Quartz.TriggerKey(this.Name, this.Group);
        }
    }

    [DataContract]
    public class JobKey
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Group { get; set; }

        public Quartz.JobKey ToQuartz()
        {
            return new Quartz.JobKey(this.Name, this.Group);
        }
    }

    [DataContract]
    public class SchedulerMetaData
    {
        [DataMember]
        public string SchedulerName { get; set; }

        [DataMember]
        public string SchedulerInstanceId { get; set; }

        [DataMember]
        public Type SchedulerType { get; set; }

        [DataMember]
        public bool SchedulerRemote { get; set; }

        [DataMember]
        public bool Started { get; set; }

        [DataMember]
        public bool InStandbyMode { get; set; }

        [DataMember]
        public bool Shutdown { get; set; }

        [DataMember]
        public Type JobStoreType { get; set; }

        [DataMember]
        public Type ThreadPoolType { get; set; }

        [DataMember]
        public int ThreadPoolSize { get; set; }

        [DataMember]
        public string Version { get; set; }

        [DataMember]
        public DateTimeOffset? RunningSince { get; set; }

        [DataMember]
        public int NumberOfJobsExecuted { get; set; }

        [DataMember]
        public bool JobStoreSupportsPersistence { get; set; }

        [DataMember]
        public bool JobStoreClustered { get; set; }

        public static SchedulerMetaData FromQuartz(Quartz.SchedulerMetaData schedulerMetaData)
        {
            throw new NotImplementedException();
        }
    }

    [DataContract]
    public enum TriggerState
    {
        [EnumMember]
        Normal,

        [EnumMember]
        Paused,

        [EnumMember]
        Complete,

        [EnumMember]
        Error,

        [EnumMember]
        Blocked,

        [EnumMember]
        None,
    }

    [ServiceContract]
    public interface ISchedulerService
    {
        [OperationContract]
        bool IsJobGroupPaused(string groupName);

        [OperationContract]
        bool IsTriggerGroupPaused(string groupName);

        [OperationContract]
        SchedulerMetaData GetMetaData();

        [OperationContract]
        string[] GetJobGroupNames();

        [OperationContract]
        string[] GetTriggerGroupNames();

        [OperationContract]
        string[] GetPausedTriggerGroups();

        [OperationContract]
        void Start(TimeSpan delay);

        [OperationContract]
        void Standby();

        [OperationContract]
        void Shutdown(bool waitForJobsToComplete);

        [OperationContract]
        DateTimeOffset ScheduleJob(JobDetail jobDetail, Trigger trigger);

        [OperationContract]
        bool UnscheduleJobs(TriggerKey[] triggerKeys);

        [OperationContract]
        DateTimeOffset? RescheduleJob(TriggerKey triggerKey, Trigger newTrigger);

        [OperationContract]
        void AddJob(JobDetail jobDetail, bool replace, bool storeNonDurableWhileAwaitingScheduling);

        [OperationContract]
        bool DeleteJobs(JobKey[] jobKeys);

        [OperationContract]
        void TriggerJob(JobKey jobKey);

        [OperationContract]
        void PauseJob(JobKey jobKey);

        [OperationContract]
        void PauseTrigger(TriggerKey triggerKey);

        [OperationContract]
        void ResumeJob(JobKey jobKey);

        [OperationContract]
        void ResumeTrigger(TriggerKey triggerKey);

        [OperationContract]
        Trigger[] GetTriggersOfJob(JobKey jobKey);

        [OperationContract]
        JobDetail GetJobDetail(JobKey jobKey);

        [OperationContract]
        Trigger GetTrigger(TriggerKey triggerKey);

        [OperationContract]
        TriggerState GetTriggerState(TriggerKey triggerKey);

        [OperationContract]
        bool Interrupt(JobKey jobKey);

        [OperationContract(Name = "CheckJobExists")]
        bool CheckExists(JobKey jobKey);

        [OperationContract(Name = "CheckTriggerExists")]
        bool CheckExists(TriggerKey triggerKey);
    }
}
