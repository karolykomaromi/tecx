namespace Hydra.Jobs.Test
{
    using System;
    using System.ServiceModel;
    using System.Threading.Tasks;
    using Hydra.Infrastructure.ServiceModel;
    using Hydra.Jobs.Client;
    using Hydra.Jobs.Server;
    using Xunit;

    public class ClientServerCommunicationTests
    {
        private static readonly Uri ServiceAddress = new Uri("net.pipe://localhost/scheduler");

        private static readonly Uri BaseAddress = new Uri("http://localhost:80/Temporary_Listen_Addresses/scheduler");

        [Fact]
        public async Task Should_Send_Message()
        {
            using (ServiceHost host = new ServiceHost(new SchedulerService(new NullScheduler()), BaseAddress))
            {
                host.AddServiceEndpoint(
                    typeof(ISchedulerService),
                    new NetNamedPipeBinding(NetNamedPipeSecurityMode.None),
                    ServiceAddress);

                ServiceBehaviorAttribute behavior = host.Description.Behaviors.Find<ServiceBehaviorAttribute>();

                behavior.IncludeExceptionDetailInFaults = true;
                behavior.InstanceContextMode = InstanceContextMode.Single;

                host.Description.Behaviors.Add(new HideEnumerationClassesBehavior());

                host.Open();

                using (var client = new SchedulerClient(new NetNamedPipeBinding(NetNamedPipeSecurityMode.None), new EndpointAddress(ServiceAddress)))
                {
                    Client.SimpleJobScheduleRequest request = new Client.SimpleJobScheduleRequest
                    {
                        StartAt = DateTimeOffset.Now, 
                        Job = Client.KnownJobs.Noop
                    };

                    Client.JobScheduleResponse response = await client.Schedule(request);

                    Assert.NotNull(response);
                }
            }
        }
    }
}
