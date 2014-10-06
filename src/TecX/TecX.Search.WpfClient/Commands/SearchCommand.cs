namespace TecX.Search.WpfClient.Commands
{
    using System;
    using System.ComponentModel;
    using System.Windows.Input;

    using TecX.Common;
    using TecX.Search.WpfClient.ViewModel;

    public class SearchCommand : ICommand
    {
        private readonly MainWindowViewModel vm;

        public SearchCommand(MainWindowViewModel vm)
        {
            Guard.AssertNotNull(vm, "vm");

            this.vm = vm;
            this.vm.PropertyChanged += this.OnSearchTermsChanged;
        }

        public event EventHandler CanExecuteChanged = delegate { };

        public void Execute(object parameter)
        {
            this.vm.SearchMessages();
        }

        public bool CanExecute(object parameter)
        {
            return this.vm.CanSearch;
        }

        private void OnSearchTermsChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Constants.CanSearchPropertyName)
            {
                this.CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}