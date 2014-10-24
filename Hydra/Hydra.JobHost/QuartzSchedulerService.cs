namespace Hydra.JobHost
{
    using System;
    using System.Diagnostics.Contracts;
    using Common.Logging;
    using Quartz;

    public class QuartzSchedulerService
    {
        private readonly IScheduler scheduler;

        private readonly ILog log;

        public QuartzSchedulerService(IScheduler scheduler, ILog log)
        {
            Contract.Requires(scheduler != null);
            Contract.Requires(log != null);

            this.scheduler = scheduler;
            this.log = log;
        }

        public bool Start()
        {
            try
            {
                this.scheduler.Start();

                return true;
            }
            catch (Exception ex)
            {
                this.log.Error(Properties.Resources.ServiceStartupError, ex);

                return false;
            }
        }

        public bool Stop()
        {
            try
            {
                this.scheduler.Standby();

                return true;
            }
            catch (Exception ex)
            {
                this.log.Error(Properties.Resources.ServiceStopError, ex);

                return false;
            }
        }

        public void Shutdown()
        {
            try
            {
                this.scheduler.Shutdown();
            }
            catch (Exception ex)
            {
                this.log.Error(Properties.Resources.ServiceShutdownError, ex);
            }
        }

        public bool Pause()
        {
            try
            {
                this.scheduler.PauseAll();

                return true;
            }
            catch (Exception ex)
            {
                this.log.Error(Properties.Resources.ServicePauseError, ex);

                return false;
            }
        }

        public bool Continue()
        {
            try
            {
                this.scheduler.ResumeAll();

                return true;
            }
            catch (Exception ex)
            {
                this.log.Error(Properties.Resources.ServiceContinueError, ex);

                return false;
            }
        }
    }
}