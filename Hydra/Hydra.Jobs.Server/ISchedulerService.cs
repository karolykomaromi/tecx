namespace Hydra.Jobs.Server
{
    using System;
    using System.Diagnostics.Contracts;
    using System.ServiceModel;
    using Quartz;

    [ServiceContract]
    [ContractClass(typeof(SchedulerServiceContract))]
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

    [ContractClassFor(typeof(ISchedulerService))]
    internal abstract class SchedulerServiceContract : ISchedulerService
    {
        public bool IsJobGroupPaused(string groupName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(groupName));

            return false;
        }

        public bool IsTriggerGroupPaused(string groupName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(groupName));

            return false;
        }

        public SchedulerMetaData GetMetaData()
        {
            Contract.Ensures(Contract.Result<SchedulerMetaData>() != null);

            return default(SchedulerMetaData);
        }

        public string[] GetJobGroupNames()
        {
            Contract.Ensures(Contract.Result<string[]>() != null);

            return new string[0];
        }

        public string[] GetTriggerGroupNames()
        {
            Contract.Ensures(Contract.Result<string[]>() != null);

            return new string[0];
        }

        public string[] GetPausedTriggerGroups()
        {
            Contract.Ensures(Contract.Result<string[]>() != null);

            return new string[0];
        }

        public void Start(TimeSpan delay)
        {
        }

        public void Standby()
        {
        }

        public void Shutdown(bool waitForJobsToComplete)
        {
        }

        public DateTimeOffset ScheduleJob(JobDetail jobDetail, Trigger trigger)
        {
            Contract.Requires(jobDetail != null);
            Contract.Requires(trigger != null);

            return default(DateTimeOffset);
        }

        public bool UnscheduleJobs(TriggerKey[] triggerKeys)
        {
            Contract.Requires(triggerKeys != null);

            return false;
        }

        public DateTimeOffset? RescheduleJob(TriggerKey triggerKey, Trigger newTrigger)
        {
            Contract.Requires(triggerKey != null);
            Contract.Requires(newTrigger != null);

            return default(DateTimeOffset?);
        }

        public void AddJob(JobDetail jobDetail, bool replace, bool storeNonDurableWhileAwaitingScheduling)
        {
            Contract.Requires(jobDetail != null);
        }

        public bool DeleteJobs(JobKey[] jobKeys)
        {
            Contract.Requires(jobKeys != null);

            return false;
        }

        public void TriggerJob(JobKey jobKey)
        {
            Contract.Requires(jobKey != null);
        }

        public void PauseJob(JobKey jobKey)
        {
            Contract.Requires(jobKey != null);
        }

        public void PauseTrigger(TriggerKey triggerKey)
        {
            Contract.Requires(triggerKey != null);
        }

        public void ResumeJob(JobKey jobKey)
        {
            Contract.Requires(jobKey != null);
        }

        public void ResumeTrigger(TriggerKey triggerKey)
        {
            Contract.Requires(triggerKey != null);
        }

        public Trigger[] GetTriggersOfJob(JobKey jobKey)
        {
            Contract.Requires(jobKey != null);
            Contract.Ensures(Contract.Result<Trigger[]>() != null);

            return new Trigger[0];
        }

        public JobDetail GetJobDetail(JobKey jobKey)
        {
            Contract.Requires(jobKey != null);

            return default(JobDetail);
        }

        public Trigger GetTrigger(TriggerKey triggerKey)
        {
            Contract.Requires(triggerKey != null);

            return default(Trigger);
        }

        public TriggerState GetTriggerState(TriggerKey triggerKey)
        {
            Contract.Requires(triggerKey != null);

            return default(TriggerState);
        }

        public bool Interrupt(JobKey jobKey)
        {
            Contract.Requires(jobKey != null);

            return false;
        }

        public bool CheckExists(JobKey jobKey)
        {
            Contract.Requires(jobKey != null);

            return false;
        }

        public bool CheckExists(TriggerKey triggerKey)
        {
            Contract.Requires(triggerKey != null);

            return false;
        }
    }
}
