namespace Hydra.Jobs.Test
{
    using System;
    using System.ServiceModel;
    using System.Threading.Tasks;
    using Hydra.Jobs.Client;
    using Hydra.Jobs.Server;
    using Xunit;

    public class ClientServerCommunicationTests
    {
        private const string ServiceAddress = "net.pipe://localhost/scheduler";

        [Fact]
        public async Task Should_Send_Message()
        {
            using (ServiceHost host = new ServiceHost(typeof(SchedulerService)))
            {
                host.AddServiceEndpoint(
                    typeof(ISchedulerService),
                    new NetNamedPipeBinding(NetNamedPipeSecurityMode.None),
                    new Uri(ServiceAddress));

                host.Description.Behaviors.Find<ServiceBehaviorAttribute>().IncludeExceptionDetailInFaults = true;

                host.Open();

                using (var client = new SchedulerClient(new NetNamedPipeBinding(NetNamedPipeSecurityMode.None), new EndpointAddress(ServiceAddress)))
                {
                    Client.JobScheduleResponse response = await client.Schedule(new Client.SimpleJobScheduleRequest());

                    Assert.NotNull(response);
                }
            }
        }
    }
}
