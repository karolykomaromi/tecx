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
            
            AlwaysFails alwaysFails = new AlwaysFails();
            IJob ensure = new EnsureJobExecutionExceptionDecorator(alwaysFails);
            var jobFactory = new Mock<IJobFactory>();
            jobFactory
                .Setup(jf => jf.NewJob(It.IsAny<TriggerFiredBundle>(), It.IsAny<IScheduler>()))
                .Returns(ensure);

            ManualResetEvent reset = new ManualResetEvent(false);
            IRetrySettings settings = new InMemoryRetrySettings
            {
                BackoffBaseInterval = 250.Milliseconds(), 
                MaxRetries = 2
            };
            IRetryStrategy retryStrategy = new ExponentialBackoffRetryStrategy(settings);
            IRetryStrategy unfreeze = new UnfreezeWhenJobShouldNotRunAgain(retryStrategy, reset);
            IJobListener listener = new RetryJobListener(unfreeze);

            scheduler.ListenerManager.AddJobListener(listener, GroupMatcher<JobKey>.AnyGroup());

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
                .WithIdentity("always", "fails")
                .Build();

            IJobDetail job = JobBuilder
                .Create<AlwaysFails>()
                .WithIdentity("always", "fails")
                .Build();

            scheduler.ScheduleJob(job, trigger);
            scheduler.Start();
            scheduler.ResumeAll();

            reset.WaitOne(3.Seconds());

            Assert.Equal(3, alwaysFails.Counter);
        }
    }
}