namespace Hydra.Jobs.Client
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.Serialization;
    using System.ServiceModel;
    using System.Threading.Tasks;
    using Quartz;

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

    [ContractClassFor(typeof(ISchedulerClient))]
    internal abstract class SchedulerClientContract : ISchedulerClient
    {
        public Task<bool> IsJobGroupPaused(string groupName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(groupName));

            return default(Task<bool>);
        }

        public Task<bool> IsTriggerGroupPaused(string groupName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(groupName));

            return default(Task<bool>);
        }

        public Task<SchedulerMetaData> GetMetaData()
        {
            Contract.Ensures(Contract.Result<SchedulerMetaData>() != null);

            return default(Task<SchedulerMetaData>);
        }

        public Task<string[]> GetJobGroupNames()
        {
            return default(Task<string[]>);
        }

        public Task<string[]> GetTriggerGroupNames()
        {
            return default(Task<string[]>);
        }

        public Task<string[]> GetPausedTriggerGroups()
        {
            return default(Task<string[]>);
        }

        public Task Start(TimeSpan delay)
        {
            return default(Task);
        }

        public Task Standby()
        {
            return default(Task);
        }

        public Task Shutdown(bool waitForJobsToComplete)
        {
            return default(Task);
        }

        public Task<DateTimeOffset> ScheduleJob(JobDetail jobDetail, Trigger trigger)
        {
            Contract.Requires(jobDetail != null);
            Contract.Requires(trigger != null);

            return default(Task<DateTimeOffset>);
        }

        public Task<bool> UnscheduleJobs(TriggerKey[] triggerKeys)
        {
            Contract.Requires(triggerKeys != null);

            return default(Task<bool>);
        }

        public Task<DateTimeOffset?> RescheduleJob(TriggerKey triggerKey, Trigger newTrigger)
        {
            Contract.Requires(triggerKey != null);
            Contract.Requires(newTrigger != null);

            return default(Task<DateTimeOffset?>);
        }

        public Task AddJob(JobDetail jobDetail, bool replace, bool storeNonDurableWhileAwaitingScheduling)
        {
            Contract.Requires(jobDetail != null);

            return default(Task);
        }

        public Task<bool> DeleteJobs(JobKey[] jobKeys)
        {
            Contract.Requires(jobKeys != null);

            return default(Task<bool>);
        }

        public Task TriggerJob(JobKey jobKey)
        {
            Contract.Requires(jobKey != null);

            return default(Task);
        }

        public Task PauseJob(JobKey jobKey)
        {
            Contract.Requires(jobKey != null);

            return default(Task);
        }

        public Task PauseTrigger(TriggerKey triggerKey)
        {
            Contract.Requires(triggerKey != null);

            return default(Task);
        }

        public Task ResumeJob(JobKey jobKey)
        {
            Contract.Requires(jobKey != null);

            return default(Task);
        }

        public Task ResumeTrigger(TriggerKey triggerKey)
        {
            Contract.Requires(triggerKey != null);

            return default(Task);
        }

        public Task<Trigger[]> GetTriggersOfJob(JobKey jobKey)
        {
            Contract.Requires(jobKey != null);

            return default(Task<Trigger[]>);
        }

        public Task<JobDetail> GetJobDetail(JobKey jobKey)
        {
            Contract.Requires(jobKey != null);

            return default(Task<JobDetail>);
        }

        public Task<Trigger> GetTrigger(TriggerKey triggerKey)
        {
            Contract.Requires(triggerKey != null);

            return default(Task<Trigger>);
        }

        public Task<TriggerState> GetTriggerState(TriggerKey triggerKey)
        {
            Contract.Requires(triggerKey != null);

            return default(Task<TriggerState>);
        }

        public Task<bool> Interrupt(JobKey jobKey)
        {
            Contract.Requires(jobKey != null);

            return default(Task<bool>);
        }

        public Task<bool> CheckExists(JobKey jobKey)
        {
            Contract.Requires(jobKey != null);

            return default(Task<bool>);
        }

        public Task<bool> CheckExists(TriggerKey triggerKey)
        {
            Contract.Requires(triggerKey != null);

            return default(Task<bool>);
        }
    }
}
