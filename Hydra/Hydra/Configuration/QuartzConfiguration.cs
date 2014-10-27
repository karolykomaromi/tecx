namespace Hydra.Configuration
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Web.Configuration;
    using Hydra.Infrastructure.Logging;
    using Microsoft.Practices.Unity;
    using Quartz;
    using Quartz.Impl;
    using Quartz.Impl.AdoJobStore;
    using Quartz.Simpl;
    using Quartz.Spi;

    public class QuartzConfiguration : UnityContainerExtension
    {
        protected override void Initialize()
        {
            ConnectionStringSettings mysql = WebConfigurationManager.ConnectionStrings["mysql"];

            string connectionString;

            if (mysql != null)
            {
                connectionString = mysql.ConnectionString;
            }
            else
            {
                string msg = string.Format(Infrastructure.Properties.Resources.ConnectionStringNotFound, "mysql");
                throw new ConfigurationErrorsException(msg);
            }

            string address = "tcp://localhost:5555/QuartzScheduler";

            // I want to be able to schedule jobs via my web interface but want them to be executed by a scheduler
            // that runs as a windows service.
            NameValueCollection props = new NameValueCollection(StringComparer.OrdinalIgnoreCase)
                {

                    { "quartz.scheduler.instanceName", "ServerScheduler" },
                    { "quartz.scheduler.proxy",  "true" },
                    { "quartz.threadPool.threadCount", "0" },
                    { "quartz.scheduler.proxy.address", address },
                    //// TODO weberse 2014-10-24 maybe use this thread pool type instead of setting pool size to zero
                    //// { "quartz.threadPool.type", "Quartz.Simpl.ZeroSizeThreadPool, Quartz" },
                    //// { "quartz.jobStore.type", "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz" },
                    //// { "quartz.jobStore.driverDelegateType", "Quartz.Impl.AdoJobStore.MySQLDelegate, Quartz" },
                    //// { "quartz.jobStore.dataSource", "myDS" },
                    //// { "quartz.dataSource.myDS.connectionString", connectionString },
                    //// { "quartz.dataSource.myDS.provider", "MySql-65" },
                    //// { "quartz.jobStore.useProperties", "true" }
                };

            ISchedulerFactory factory = new StdSchedulerFactory(props);

            this.Container.RegisterInstance<ISchedulerFactory>(factory);


            this.Container.RegisterType<IJobStore, JobStoreCMT>(new ContainerControlledLifetimeManager());

            this.Container.RegisterTypes(
                AllClasses.FromAssemblies(typeof(QuartzConfiguration).Assembly).Where(t => typeof(IJob).IsAssignableFrom(t)),
                _ => new[] { typeof(IJob) },
                WithName.TypeName);
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