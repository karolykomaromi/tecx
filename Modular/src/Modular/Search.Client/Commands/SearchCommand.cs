namespace Search.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Windows.Input;
    using Infrastructure.Commands;
    using Infrastructure.Events;
    using Search.ViewModels;

    public class SearchCommand : ICommand, ISubscribeTo<CanExecuteChanged>
    {
        public event EventHandler CanExecuteChanged = delegate { };

        public bool CanExecute(object parameter)
        {
            Contract.Requires(parameter != null);

            SearchViewModel vm = parameter as SearchViewModel;

            return vm != null && !string.IsNullOrEmpty(vm.SearchTerm) && vm.SearchTerm.Length >= 3;
        }

        public void Execute(object parameter)
        {
            Contract.Requires(parameter != null);

            SearchViewModel vm = parameter as SearchViewModel;

            if (vm != null &&
                !string.IsNullOrEmpty(vm.SearchTerm) &&
                vm.SearchTerm.Length >= 3)
            {
                vm.Search();
            }
        }

        void ISubscribeTo<CanExecuteChanged>.Handle(CanExecuteChanged message)
        {
            this.CanExecuteChanged(this, EventArgs.Empty);
        }
    }
}