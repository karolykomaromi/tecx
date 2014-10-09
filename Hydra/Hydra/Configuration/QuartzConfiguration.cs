using System.Linq;
using Hydra.Infrastructure.Reflection;

namespace Hydra.Configuration
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reflection;
    using Hydra.Infrastructure.Logging;
    using Microsoft.Practices.Unity;
    using Quartz;
    using Quartz.Impl;
    using Quartz.Spi;

    public class QuartzConfiguration : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Container.RegisterTypes(
                AllClasses.FromAssemblies(typeof(QuartzConfiguration).Assembly).Where(t => typeof(IJob).IsAssignableFrom(t)),
                _ => new[] { typeof(IJob) },
                WithName.TypeName);

            this.Container.RegisterType<IJobFactory, UnityJobFactory>(new ContainerControlledLifetimeManager());
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

        private class UnityJobFactory : IJobFactory
        {
            private readonly IUnityContainer container;

            public UnityJobFactory(IUnityContainer container)
            {
                Contract.Requires(container != null);

                this.container = container;
            }

            public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
            {
                Type jobType = bundle.JobDetail.JobType;

                try
                {
                    return (IJob)this.container.Resolve(jobType);
                }
                catch (ResolutionFailedException ex)
                {
                    HydraEventSource.Log.MissingMapping(ex.TypeRequested, typeof(Missing).Name);

                    throw;
                }
            }

            public void ReturnJob(IJob job)
            {
                throw new NotImplementedException();
            }
        }
    }
}