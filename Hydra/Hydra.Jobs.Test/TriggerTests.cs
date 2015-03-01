namespace Hydra.Jobs.Test
{
    using System.Linq;
    using System.Threading;
    using FluentNHibernate.Testing.Values;
    using Moq;
    using Quartz;
    using Quartz.Impl;
    using Quartz.Impl.Matchers;
    using Quartz.Job;
    using Quartz.Listener;
    using Quartz.Spi;
    using Xunit;

    public class TriggerTests
    {
        [Fact]
        public void Should_Try_3_Times_And_Then_Give_Up()
        {
            ISchedulerFactory factory = new StdSchedulerFactory();

            IScheduler scheduler = factory.GetScheduler();

            ManualResetEvent reset = new ManualResetEvent(false);

            var jobFactory = new Mock<IJobFactory>();
            jobFactory.Setup(jf => jf.NewJob(It.IsAny<TriggerFiredBundle>(), It.IsAny<IScheduler>()))
                .Returns(new Fails(reset));

            scheduler.ListenerManager.AddJobListener(new RetryJobListener(), GroupMatcher<JobKey>.AnyGroup());

            scheduler.JobFactory = jobFactory.Object;

            ITrigger trigger = TriggerBuilder
                .Create()
                .StartNow()
                .WithSimpleSchedule(
                    x =>
                    {
                        x.WithIntervalInSeconds(1);
                        x.WithRepeatCount(1);
                    })
                .WithIdentity("fails", "mail")
                .Build();

            IJobDetail job = JobBuilder.Create<Fails>().WithIdentity("fails", "mail").Build();

            scheduler.ScheduleJob(job, trigger);

            scheduler.Start();

            scheduler.ResumeAll();

            reset.WaitOne();
        }
    }

    public class RetryJobListener : JobListenerSupport
    {
        public override string Name
        {
            get { return "Retry"; }
        }

        public override void JobToBeExecuted(IJobExecutionContext context)
        {
        }

        public override void JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException)
        {
        }
    }

    public class TestSupportJobListener : IJobListener
    {
        private readonly IJobListener inner;

        public TestSupportJobListener(IJobListener inner)
        {
            this.inner = inner;
        }

        public string Name
        {
            get { return this.inner.Name; }
        }

        public void JobToBeExecuted(IJobExecutionContext context)
        {
            this.inner.JobToBeExecuted(context);
        }

        public void JobExecutionVetoed(IJobExecutionContext context)
        {
            this.inner.JobExecutionVetoed(context);
        }

        public void JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException)
        {
            this.inner.JobWasExecuted(context, jobException);
        }
    }

    public class Fails : IJob
    {
        private readonly ManualResetEvent reset;

        public Fails(ManualResetEvent reset)
        {
            this.reset = reset;
        }

        public void Execute(IJobExecutionContext context)
        {
            this.reset.Set();

            throw new JobExecutionException();
        }
    }
}
