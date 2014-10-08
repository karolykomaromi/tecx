namespace Search.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Windows.Input;
    using Infrastructure;
    using Search.ViewModels;

    public class SuggestionsCommand : ICommand
    {
        public event EventHandler CanExecuteChanged = delegate { };

        public bool CanExecute(object parameter)
        {
            Contract.Requires(parameter != null);

            var token = parameter as Token;

            if (token == null)
            {
                return false;
            }

            SearchViewModel vm = token.Parameter as SearchViewModel;

            return vm != null && !string.IsNullOrEmpty(vm.SearchTerm) && vm.SearchTerm.Length >= 3;
        }

        public void Execute(object parameter)
        {
            Contract.Requires(parameter != null);

            var token = parameter as Token;

            if (token == null)
            {
                return;
            }

            SearchViewModel vm = token.Parameter as SearchViewModel;

            if (vm != null &&
                !string.IsNullOrEmpty(vm.SearchTerm) &&
                vm.SearchTerm.Length >= 3)
            {
                vm.SearchSuggestions(token);
            }
        }
    }
}