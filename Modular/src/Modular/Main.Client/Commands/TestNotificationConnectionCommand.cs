namespace Main.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using System.Windows.Threading;
    using Infrastructure.Commands;
    using Infrastructure.Events;
    using Main.ViewModels;
    using Microsoft.AspNet.SignalR.Client.Hubs;

    public class TestNotificationConnectionCommand : ICommand, ISubscribeTo<CanExecuteChanged>
    {
        private readonly Dispatcher dispatcher;

        public TestNotificationConnectionCommand(Dispatcher dispatcher)
        {
            Contract.Requires(dispatcher != null);

            this.dispatcher = dispatcher;
        }

        public event EventHandler CanExecuteChanged = delegate { };

        public bool CanExecute(object parameter)
        {
            var vm = parameter as OptionsViewModel;

            if (vm == null)
            {
                return false;
            }

            string uri = vm.NotificationUrl;

            if (!string.IsNullOrEmpty(uri))
            {
                return Uri.IsWellFormedUriString(uri, UriKind.Absolute);
            }

            return false;
        }

        public void Execute(object parameter)
        {
            var vm = parameter as OptionsViewModel;

            if (vm != null)
            {
                Uri uri = new Uri(vm.NotificationUrl, UriKind.Absolute);

                HubConnection connection = new HubConnection(uri.ToString());

                connection.Error += (Exception ex) =>
                    {
                        var vm1 = vm;
                        this.dispatcher.BeginInvoke(() => vm1.TestConnectionReturn = ex.ToString());
                    };

                IHubProxy proxy = connection.CreateHubProxy("NotificationHub");

                proxy.On(
                    "notify",
                    (string notification) =>
                    {
                        var vm1 = vm;
                        var c1 = connection;
                        this.dispatcher.BeginInvoke(() => vm1.TestConnectionReturn = notification);
                        c1.Stop();
                    });

                Task start = connection.Start();

                start.ContinueWith(startCompleted =>
                    {
                        if (startCompleted.IsFaulted)
                        {

                        }
                        else
                        {
                            proxy.Invoke("Notify", "Test successfull.").ContinueWith(invokeCompleted =>
                            {

                            });
                        }
                    });
            }
        }

        public void Handle(CanExecuteChanged message)
        {
            this.CanExecuteChanged(this, EventArgs.Empty);
        }
    }
}