namespace Hydra.Jobs.Server
{
    using System;
    using System.Diagnostics.Contracts;
    using System.ServiceModel;
    using Common.Logging;
    using Quartz;

    public class SchedulerServiceHost : IDisposable
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

            this.host = new ServiceHost(new SchedulerService(scheduler), baseAddresses);
            this.host.Closed += this.OnHostClosed;
            this.host.Closing += this.OnHostClosing;
            this.host.Faulted += this.OnHostFaulted;
            this.host.Opened += this.OnHostOpened;
            this.host.Opening += this.OnHostOpening;
            this.host.UnknownMessageReceived += this.OnHostUnkownMessageReceived;
            this.host.Description.Behaviors.Find<ServiceBehaviorAttribute>().InstanceContextMode = InstanceContextMode.Single;
            this.host.AddServiceEndpoint(
                typeof(ISchedulerService),
                new NetTcpBinding(SecurityMode.None),
                new Uri("net.pipe://localhost/scheduler"));
        }

        public bool Start()
        {
            try
            {
                this.scheduler.Start();

                this.host.Open();

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
                this.host.Close();
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

        public void Dispose()
        {
            if (this.host != null)
            {
                this.host.Close();

                this.host.Closed -= this.OnHostClosed;
                this.host.Closing -= this.OnHostClosing;
                this.host.Faulted -= this.OnHostFaulted;
                this.host.Opened -= this.OnHostOpened;
                this.host.Opening -= this.OnHostOpening;
                this.host.UnknownMessageReceived -= this.OnHostUnkownMessageReceived;
            }
        }

        private void OnHostOpening(object sender, EventArgs e)
        {
        }

        private void OnHostOpened(object sender, EventArgs e)
        {
        }

        private void OnHostClosing(object sender, EventArgs e)
        {
        }

        private void OnHostClosed(object sender, EventArgs e)
        {
        }

        private void OnHostFaulted(object sender, EventArgs e)
        {
        }

        private void OnHostUnkownMessageReceived(object sender, UnknownMessageReceivedEventArgs e)
        {
        }
    }
}