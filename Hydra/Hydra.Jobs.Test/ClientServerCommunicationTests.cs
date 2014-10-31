namespace Hydra.Jobs.Test
{
    using System;
    using System.ServiceModel;
    using System.Threading.Tasks;
    using AutoMapper;
    using Hydra.Infrastructure;
    using Hydra.Jobs.Client;
    using Hydra.Jobs.Server;
    using Quartz;
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
           Mapper.AddProfile(new JobsProfile());

            ICalendarIntervalTrigger source = TriggerBuilder.Create().WithCalendarIntervalSchedule(b => b.WithIntervalInDays(3)).Build() as ICalendarIntervalTrigger;

            CalendarIntervalTrigger destination = Mapper.Map<CalendarIntervalTrigger>(source);

            Assert.NotNull(source);
            Assert.Equal(source.RepeatInterval, destination.RepeatInterval);
            Assert.Equal(source.RepeatIntervalUnit, destination.RepeatIntervalUnit);
        }
    }
}
