using AutoMapper;
using Quartz;

namespace Hydra.Jobs.Test
{
    using System;
    using System.ServiceModel;
    using System.Threading.Tasks;
    using Hydra.Infrastructure;
    using Hydra.Jobs.Client;
    using Hydra.Jobs.Server;
    using Xunit;

    public class ClientServerCommunicationTests
    {
        [Fact]
        public async Task Should_Send_Message()
        {
            using (var host = new ServiceHost(typeof(SchedulerService), new Uri("http://localhost:12345/scheduler")))
            {
                host.Description.Behaviors.Find<ServiceBehaviorAttribute>().IncludeExceptionDetailInFaults = true;

                host.Open();

                using (var client = new SchedulerClient(new BasicHttpBinding(), new EndpointAddress("http://localhost:12345/scheduler")))
                {
                    //bool exists = await client.CheckExists(new Client.JobKey());

                    //Assert.True(exists);

                    await client.Start(10.Seconds());
                }
            }
        }

        [Fact]
        public void Should_Map()
        {
            Mapper.CreateMap<ICalendarIntervalTrigger, CalendarIntervalTrigger>();
            Mapper.CreateMap<Quartz.TriggerKey, Server.TriggerKey>();
            Mapper.CreateMap<Quartz.JobKey, Server.JobKey>();

            ICalendarIntervalTrigger source = TriggerBuilder.Create().WithCalendarIntervalSchedule(b => b.WithIntervalInDays(3)).Build() as ICalendarIntervalTrigger;

            CalendarIntervalTrigger destination = Mapper.Map<CalendarIntervalTrigger>(source);
        }
    }
}
