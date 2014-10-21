namespace Hydra.Configuration
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Hydra.Infrastructure.Logging;
    using Microsoft.Practices.Unity;
    using Quartz;
    using Quartz.Impl;
    using Quartz.Simpl;
    using Quartz.Spi;

    public class QuartzConfiguration : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Container.RegisterType<IJobStore, RAMJobStore>(new ContainerControlledLifetimeManager());

            this.Container.RegisterTypes(
                AllClasses.FromAssemblies(typeof(QuartzConfiguration).Assembly).Where(t => typeof(IJob).IsAssignableFrom(t)),
                _ => new[] { typeof(IJob) },
                WithName.TypeName);

            this.Container.RegisterType<IJobFactory, UnityJobFactory>();
            this.Container.RegisterType<ISchedulerFactory, StdSchedulerFactory>(new ContainerControlledLifetimeManager(), new InjectionConstructor());
            this.Container.RegisterType<IScheduler>(
                new InjectionFactory(c =>
                    {
                        IScheduler scheduler = c.Resolve<ISchedulerFactory>().GetScheduler();

                        scheduler.JobFactory = c.Resolve<IJobFactory>();

                        return scheduler;
                    }),
                new InjectionProperty("JobFactory"));
        }

        private sealed class UnityJobFactory : PropertySettingJobFactory
        {
            private readonly IUnityContainer container;

            public UnityJobFactory(IUnityContainer container)
            {
                Contract.Requires(container != null);

                this.ThrowIfPropertyNotFound = false;

                this.WarnIfPropertyNotFound = true;

                this.container = container;
            }

            public override IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
            {
                Contract.Requires(bundle != null);
                Contract.Requires(bundle.JobDetail != null);
                Contract.Requires(bundle.JobDetail.JobType != null);
                Contract.Ensures(Contract.Result<IJob>() != null);

                Type jobType = bundle.JobDetail.JobType;

                try
                {
                    IJob job = (IJob)this.container.Resolve(jobType);

                    JobDataMap data = new JobDataMap();
                    data.PutAll(scheduler.Context);
                    data.PutAll(bundle.JobDetail.JobDataMap);
                    data.PutAll(bundle.Trigger.JobDataMap);

                    this.SetObjectProperties((object)job, data);

                    return job;
                }
                catch (ResolutionFailedException ex)
                {
                    HydraEventSource.Log.MissingMapping(ex.TypeRequested, ex.NameRequested);

                    throw;
                }
            }

            public override void ReturnJob(IJob job)
            {
                Contract.Requires(job != null);

                this.container.Teardown(job);
            }
        }
    }
}