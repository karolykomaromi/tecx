namespace Hydra.Jobs.Test.Retry
{
    using System.Threading;
    using Hydra.Infrastructure;
    using Hydra.Jobs.Server.Jobs;
    using Hydra.Jobs.Server.Retry;
    using Moq;
    using Quartz;
    using Quartz.Impl;
    using Quartz.Impl.Matchers;
    using Quartz.Spi;
    using Xunit;

    public class RetryJobListenerTests
    {
        [Fact]
        public void Should_Try_3_Times_And_Then_Give_Up()
        {
            ISchedulerFactory factory = new StdSchedulerFactory();

            IScheduler scheduler = factory.GetScheduler();

            ManualResetEvent reset = new ManualResetEvent(false);
            
            AlwaysFails alwaysFails = new AlwaysFails();

            var jobFactory = new Mock<IJobFactory>();

            jobFactory.Setup(jf => jf.NewJob(It.IsAny<TriggerFiredBundle>(), It.IsAny<IScheduler>()))
                .Returns(new ExceptionHandlingDecorator(alwaysFails));

            var settings = new InMemoryRetrySettings { BackoffBaseInterval = 1.Seconds() };

            IRetryStrategy retryStrategy = new ExponentialBackoffRetryStrategy(settings);

            scheduler.ListenerManager.AddJobListener(new TestSupportJobListener(new RetryJobListener(retryStrategy), reset), GroupMatcher<JobKey>.AnyGroup());

            scheduler.JobFactory = jobFactory.Object;

            ITrigger trigger = TriggerBuilder
                .Create()
                .StartNow()
                .WithSimpleSchedule(
                    x =>
                    {
                        x.WithIntervalInSeconds(1);
                        x.WithRepeatCount(0);
                    })
                .WithIdentity("fails", "mail")
                .Build();

            IJobDetail job = JobBuilder.Create<AlwaysFails>().WithIdentity("fails", "mail").Build();

            scheduler.ScheduleJob(job, trigger);

            scheduler.Start();

            scheduler.ResumeAll();

            reset.WaitOne(15.Seconds());

            Assert.Equal(3, alwaysFails.Counter);
        }
    }
}