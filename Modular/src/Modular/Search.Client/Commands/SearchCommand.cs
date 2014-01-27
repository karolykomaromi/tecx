namespace Search.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Windows.Input;
    using Infrastructure.Commands;
    using Search.ViewModels;

    public class SearchCommand : ICommand
    {
        private readonly ICommandManager commandManager;

        public SearchCommand(ICommandManager commandManager)
        {
            Contract.Requires(commandManager != null);

            this.commandManager = commandManager;
        }

        public event EventHandler CanExecuteChanged
        {
            add { this.commandManager.RequerySuggested += value; }
            remove { this.commandManager.RequerySuggested -= value; }
        }

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
    }
}