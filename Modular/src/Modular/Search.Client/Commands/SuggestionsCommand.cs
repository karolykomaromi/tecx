namespace Search.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Windows.Input;
    using Infrastructure.Triggers;
    using Search.ViewModels;

    public class SuggestionsCommand : ICommand
    {
        public event EventHandler CanExecuteChanged = delegate { };

        public bool CanExecute(object parameter)
        {
            Contract.Requires(parameter != null);

            var acp = parameter as AutoCompleteParameter;

            if (acp == null)
            {
                return false;
            }

            SearchViewModel vm = acp.CommandParameter as SearchViewModel;

            return vm != null && !string.IsNullOrEmpty(vm.SearchTerm) && vm.SearchTerm.Length >= 3;
        }

        public void Execute(object parameter)
        {
            Contract.Requires(parameter != null);

            var acp = parameter as AutoCompleteParameter;

            if (acp == null)
            {
                return;
            }

            acp.CancelPopulating();

            SearchViewModel vm = acp.CommandParameter as SearchViewModel;

            if (vm != null &&
                !string.IsNullOrEmpty(vm.SearchTerm) &&
                vm.SearchTerm.Length >= 3)
            {
                vm.SearchSuggestions(acp.PopulateComplete);
            }
        }
    }
}