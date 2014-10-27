namespace Hydra.JobHost.Configuration
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Diagnostics.Contracts;
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
            ConnectionStringSettings mysql = ConfigurationManager.ConnectionStrings["mysql"];

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

            NameValueCollection props = new NameValueCollection(StringComparer.OrdinalIgnoreCase)
                {
                    { "quartz.jobStore.clustered", "true" },
                    { "quartz.threadPool.threadCount", "3" },
                    { "quartz.jobStore.type", "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz" },
                    { "quartz.jobStore.driverDelegateType", "Quartz.Impl.AdoJobStore.MySQLDelegate, Quartz" },
                    { "quartz.jobStore.dataSource", "myDS" },
                    { "quartz.dataSource.myDS.connectionString", connectionString },
                    { "quartz.dataSource.myDS.provider", "MySql-65" },
                    { "quartz.jobStore.useProperties", "true" },

                    //// TODO weberse 2014-10-24 copied from Quartz Server config
                    { "quartz.scheduler.instanceName", "ServerScheduler" },
                    { "quartz.scheduler.exporter.type", "Quartz.Simpl.RemotingSchedulerExporter, Quartz" },
                    { "quartz.scheduler.exporter.port", "5555" },
                    { "quartz.scheduler.exporter.bindName", "QuartzScheduler" },
                    { "quartz.scheduler.exporter.channelType", "tcp" },
                    { "quartz.scheduler.exporter.channelName", "httpQuartz" }
                };

            ISchedulerFactory factory = new StdSchedulerFactory(props);

            this.Container.RegisterInstance<ISchedulerFactory>(factory);

            this.Container.RegisterType<IJobFactory, UnityJobFactory>();

            this.Container.RegisterType<IScheduler>(
                new InjectionFactory(c =>
                    {
                        IScheduler scheduler = c.Resolve<ISchedulerFactory>().GetScheduler();

                        scheduler.JobFactory = c.Resolve<IJobFactory>();

                        return scheduler;
                    }));
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