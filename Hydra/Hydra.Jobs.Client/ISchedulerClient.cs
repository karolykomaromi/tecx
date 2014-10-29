namespace Hydra.Jobs.Client
{
    using System;
    using System.Runtime.Serialization;
    using System.ServiceModel;
    using System.Threading.Tasks;

    [DataContract]
    public class Trigger
    {
    }

    [DataContract]
    public class JobDetail
    {
    }

    [DataContract]
    public class TriggerKey
    {
    }

    [DataContract]
    public class JobKey
    {
    }

    [DataContract]
    public class SchedulerMetaData
    {
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

    [ServiceContract(Name = "ISchedulerService")]
    public interface ISchedulerClient
    {
        [OperationContract]
        Task<bool> IsJobGroupPaused(string groupName);

        [OperationContract]
        Task<bool> IsTriggerGroupPaused(string groupName);

        [OperationContract]
        Task<SchedulerMetaData> GetMetaData();

        [OperationContract]
        Task<string[]> GetJobGroupNames();

        [OperationContract]
        Task<string[]> GetTriggerGroupNames();

        [OperationContract]
        Task<string[]> GetPausedTriggerGroups();

        [OperationContract]
        Task Start(TimeSpan delay);

        [OperationContract]
        Task Standby();

        [OperationContract]
        Task Shutdown(bool waitForJobsToComplete);

        [OperationContract]
        Task<DateTimeOffset> ScheduleJob(JobDetail jobDetail, Trigger trigger);

        [OperationContract]
        Task<bool> UnscheduleJobs(TriggerKey[] triggerKeys);

        [OperationContract]
        Task<DateTimeOffset?> RescheduleJob(TriggerKey triggerKey, Trigger newTrigger);

        [OperationContract]
        Task AddJob(JobDetail jobDetail, bool replace, bool storeNonDurableWhileAwaitingScheduling);

        [OperationContract]
        Task<bool> DeleteJobs(JobKey[] jobKeys);

        [OperationContract]
        Task TriggerJob(JobKey jobKey);

        [OperationContract]
        Task PauseJob(JobKey jobKey);

        [OperationContract]
        Task PauseTrigger(TriggerKey triggerKey);

        [OperationContract]
        Task ResumeJob(JobKey jobKey);

        [OperationContract]
        Task ResumeTrigger(TriggerKey triggerKey);

        [OperationContract]
        Task<Trigger[]> GetTriggersOfJob(JobKey jobKey);

        [OperationContract]
        Task<JobDetail> GetJobDetail(JobKey jobKey);

        [OperationContract]
        Task<Trigger> GetTrigger(TriggerKey triggerKey);

        [OperationContract]
        Task<TriggerState> GetTriggerState(TriggerKey triggerKey);

        [OperationContract]
        Task<bool> Interrupt(JobKey jobKey);

        [OperationContract(Name = "CheckJobExists")]
        Task<bool> CheckExists(JobKey jobKey);

        [OperationContract(Name = "CheckTriggerExists")]
        Task<bool> CheckExists(TriggerKey triggerKey);
    }
}
