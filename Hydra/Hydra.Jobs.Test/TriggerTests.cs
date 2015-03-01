namespace Hydra.Jobs.Test
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Threading;
    using Hydra.Infrastructure;
    using Moq;
    using Quartz;
    using Quartz.Impl;
    using Quartz.Impl.Matchers;
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
                .Returns(new Fails());

            var settings = new InMemoryRetrySettings { BackoffBaseInterval = 1.Seconds() };

            scheduler.ListenerManager.AddJobListener(new RetryJobListener(settings, reset), GroupMatcher<JobKey>.AnyGroup());

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

            IJobDetail job = JobBuilder.Create<Fails>().WithIdentity("fails", "mail").Build();

            scheduler.ScheduleJob(job, trigger);

            scheduler.Start();

            scheduler.ResumeAll();

            reset.WaitOne();
        }
    }

    public interface IRetrySettings
    {
        int MaxRetries { get; }

        TimeSpan BackoffBaseInterval { get; }
    }

    public class InMemoryRetrySettings : IRetrySettings
    {
        public InMemoryRetrySettings()
        {
            this.MaxRetries = 3;
            this.BackoffBaseInterval = 2.Minutes();
        }

        public int MaxRetries { get; set; }

        public TimeSpan BackoffBaseInterval { get; set; }
    }

    public class RetryJobListener : JobListenerSupport
    {
        private readonly IRetrySettings settings;

        private readonly ManualResetEvent reset;

        /// <summary>
        /// Retries
        /// </summary>
        private const string Retries = "Retries";

        public RetryJobListener(IRetrySettings settings, ManualResetEvent reset)
        {
            Contract.Requires(settings != null);

            this.settings = settings;
            this.reset = reset;
        }

        public override string Name
        {
            get { return "Retry"; }
        }

        public override void JobToBeExecuted(IJobExecutionContext context)
        {
        }

        public override void JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException)
        {
            if (JobSucceeded(jobException))
            {
                return;
            }

            try
            {
                int retries = 0;
                object o;
                if (context.JobDetail.JobDataMap.TryGetValue(Retries, out o) && o is int)
                {
                    retries = (int)o;
                }

                if (retries < this.settings.MaxRetries)
                {
                    // TODO weberse 2015-03-01 reschedule job. exponential backoff 2, 4, 8 minutes

                    long factor = (long)Math.Pow(2, retries);

                    TimeSpan backoff = new TimeSpan(this.settings.BackoffBaseInterval.Ticks * factor);

                    ITrigger trigger = TriggerBuilder.Create()
                        .StartAt(DateTimeOffset.UtcNow + backoff)
                        .WithSimpleSchedule(x => x.WithRepeatCount(0))
                        .WithIdentity(context.Trigger.Key)
                        .ForJob(context.JobDetail)
                        .Build();

                    context.JobDetail.JobDataMap[Retries] = ++retries;

                    context.Scheduler.UnscheduleJob(context.Trigger.Key);

                    context.Scheduler.ScheduleJob(context.JobDetail, trigger);
                }
                else
                {
                    this.reset.Set();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                this.reset.Set();
            }
        }

        private static bool JobSucceeded(JobExecutionException jobException)
        {
            return jobException == null;
        }
    }

    public class ExceptionHandlingDecorator : IJob
    {
        private readonly IJob inner;

        public ExceptionHandlingDecorator(IJob inner)
        {
            Contract.Requires(inner != null);

            this.inner = inner;
        }

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                this.inner.Execute(context);
            }
            catch (JobExecutionException)
            {
                // JobExecutionExceptions are handled by Quartz
                throw;
            }
            catch (Exception cause)
            {
                // No other exception Type is allowed so we wrap them in a JobExecutionException (which is handled by Quartz)
                throw new JobExecutionException(cause);
            }
        }
    }

    public interface IRetryStrategy
    {
        bool ShouldRetry(IJobExecutionContext context);

        ITrigger GetTrigger(IJobExecutionContext context);
    }

    public class ExponentialBackoffRetryStrategy : IRetryStrategy
    {
        /// <summary>
        /// Retries
        /// </summary>
        private const string Retries = "Retries";

        private readonly IRetrySettings settings;

        public ExponentialBackoffRetryStrategy(IRetrySettings settings)
        {
            Contract.Requires(settings != null);

            this.settings = settings;
        }

        public bool ShouldRetry(IJobExecutionContext context)
        {
            int retries = GetAlreadyPerformedRetries(context);

            return retries < this.settings.MaxRetries;
        }

        private static int GetAlreadyPerformedRetries(IJobExecutionContext context)
        {
            int retries = 0;
            object o;
            if (context.JobDetail.JobDataMap.TryGetValue(Retries, out o) && o is int)
            {
                retries = (int)o;
            }
            return retries;
        }

        public ITrigger GetTrigger(IJobExecutionContext context)
        {
            int retries = GetAlreadyPerformedRetries(context);

            long factor = (long)Math.Pow(2, retries);

            TimeSpan backoff = new TimeSpan(this.settings.BackoffBaseInterval.Ticks * factor);

            ITrigger trigger = TriggerBuilder.Create()
                .StartAt(DateTimeOffset.UtcNow + backoff)
                .WithSimpleSchedule(x => x.WithRepeatCount(0))
                .WithIdentity(context.Trigger.Key)
                .ForJob(context.JobDetail)
                .Build();

            context.JobDetail.JobDataMap[Retries] = ++retries;

            return trigger;
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
        public void Execute(IJobExecutionContext context)
        {
            throw new JobExecutionException();
        }
    }
}
