namespace Hydra.Jobs.Client
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Threading.Tasks;
    using Quartz;

    public class SchedulerClient : ClientBase<ISchedulerClient>, ISchedulerClient
    {
        public SchedulerClient(Binding binding, EndpointAddress remoteAddress)
            : base(binding, remoteAddress)
        {
        }

        public async Task<bool> IsJobGroupPaused(string groupName)
        {
            return await this.Channel.IsJobGroupPaused(groupName);
        }

        public async Task<bool> IsTriggerGroupPaused(string groupName)
        {
            return await this.Channel.IsTriggerGroupPaused(groupName);
        }

        public async Task<SchedulerMetaData> GetMetaData()
        {
            return await this.Channel.GetMetaData();
        }

        public async Task<string[]> GetJobGroupNames()
        {
            return await this.Channel.GetJobGroupNames();
        }

        public async Task<string[]> GetTriggerGroupNames()
        {
            return await this.Channel.GetTriggerGroupNames();
        }

        public async Task<string[]> GetPausedTriggerGroups()
        {
            return await this.Channel.GetPausedTriggerGroups();
        }

        public async Task Start(TimeSpan delay)
        {
            await this.Channel.Start(delay);
        }

        public async Task Standby()
        {
            await this.Channel.Standby();
        }

        public async Task Shutdown(bool waitForJobsToComplete)
        {
            await this.Channel.Shutdown(waitForJobsToComplete);
        }

        public async Task<DateTimeOffset> ScheduleJob(JobDetail jobDetail, Trigger trigger)
        {
            return await this.Channel.ScheduleJob(jobDetail, trigger);
        }

        public async Task<bool> UnscheduleJobs(TriggerKey[] triggerKeys)
        {
            return await this.Channel.UnscheduleJobs(triggerKeys);
        }

        public async Task<DateTimeOffset?> RescheduleJob(TriggerKey triggerKey, Trigger newTrigger)
        {
            return await this.Channel.RescheduleJob(triggerKey, newTrigger);
        }

        public async Task AddJob(JobDetail jobDetail, bool replace, bool storeNonDurableWhileAwaitingScheduling)
        {
            await this.Channel.AddJob(jobDetail, replace, storeNonDurableWhileAwaitingScheduling);
        }

        public async Task<bool> DeleteJobs(JobKey[] jobKeys)
        {
            return await this.Channel.DeleteJobs(jobKeys);
        }

        public async Task TriggerJob(JobKey jobKey)
        {
            await this.Channel.TriggerJob(jobKey);
        }

        public async Task PauseJob(JobKey jobKey)
        {
            await this.Channel.PauseJob(jobKey);
        }

        public async Task PauseTrigger(TriggerKey triggerKey)
        {
            await this.Channel.PauseTrigger(triggerKey);
        }

        public async Task ResumeJob(JobKey jobKey)
        {
            await this.Channel.ResumeJob(jobKey);
        }

        public async Task ResumeTrigger(TriggerKey triggerKey)
        {
            await this.Channel.ResumeTrigger(triggerKey);
        }

        public async Task<Trigger[]> GetTriggersOfJob(JobKey jobKey)
        {
            return await this.Channel.GetTriggersOfJob(jobKey);
        }

        public async Task<JobDetail> GetJobDetail(JobKey jobKey)
        {
            return await this.Channel.GetJobDetail(jobKey);
        }

        public async Task<Trigger> GetTrigger(TriggerKey triggerKey)
        {
            return await this.Channel.GetTrigger(triggerKey);
        }

        public async Task<TriggerState> GetTriggerState(TriggerKey triggerKey)
        {
            return await this.Channel.GetTriggerState(triggerKey);
        }

        public async Task<bool> Interrupt(JobKey jobKey)
        {
            return await this.Channel.Interrupt(jobKey);
        }

        public async Task<bool> CheckExists(JobKey jobKey)
        {
            return await this.Channel.CheckExists(jobKey);
        }

        public async Task<bool> CheckExists(TriggerKey triggerKey)
        {
            return await this.Channel.CheckExists(triggerKey);
        }
    }
}
