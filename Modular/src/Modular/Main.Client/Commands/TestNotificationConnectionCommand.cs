using System;
using System.Windows.Input;
using Infrastructure.Commands;
using Infrastructure.Events;
using Main.ViewModels;

namespace Main.Commands
{
    public class TestNotificationConnectionCommand : ICommand, ISubscribeTo<CanExecuteChanged>
    {
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
            ////var vm = parameter as OptionsViewModel;

            ////if (vm != null)
            ////{
            ////    vm.TestConnection();
            ////}
        }

        public void Handle(CanExecuteChanged message)
        {
            this.CanExecuteChanged(this, EventArgs.Empty);
        }
    }
}