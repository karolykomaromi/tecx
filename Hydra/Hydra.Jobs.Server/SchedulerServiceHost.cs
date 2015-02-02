namespace Hydra.Jobs.Server
{
    using System;
    using System.Diagnostics.Contracts;
    using System.ServiceModel;
    using Common.Logging;
    using Quartz;

    public class SchedulerServiceHost
    {
        private readonly ILog log;
        private readonly ServiceHost host;

        private readonly IScheduler scheduler;

        public SchedulerServiceHost(IScheduler scheduler, ILog log, params Uri[] baseAddresses)
        {
            Contract.Requires(scheduler != null);
            Contract.Requires(log != null);

            this.scheduler = scheduler;
            this.log = log;
            this.host = new ServiceHost(scheduler, baseAddresses);
        }

        public bool Start()
        {
            try
            {
                this.host.Open();

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
                this.host.Close();

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