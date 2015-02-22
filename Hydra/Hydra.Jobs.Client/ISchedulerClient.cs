namespace Hydra.Jobs.Client
{
    using System.Diagnostics.Contracts;
    using System.ServiceModel;
    using System.Threading.Tasks;
    using Hydra.Infrastructure;
    using Constants = Hydra.Jobs.Constants;

    [ServiceContract(Name = Constants.ServiceName, Namespace = Constants.ServiceNamespace)]
    public interface ISchedulerClient
    {
        [OperationContract(Name = Constants.Methods.SimpleSchedule)]
        Task<JobScheduleResponse> Schedule(SimpleJobScheduleRequest jobSchedule);
    }

    [ContractClassFor(typeof(ISchedulerClient))]
    internal abstract class SchedulerClientContract : ISchedulerClient
    {
        public Task<JobScheduleResponse> Schedule(SimpleJobScheduleRequest jobSchedule)
        {
            Contract.Requires(jobSchedule != null);

            return Default.Value<Task<JobScheduleResponse>>();
        }
    }
}
