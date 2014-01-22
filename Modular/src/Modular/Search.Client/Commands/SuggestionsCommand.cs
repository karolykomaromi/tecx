namespace Search.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Windows.Input;
    using Infrastructure;
    using Search.Service;
    using Search.ViewModels;

    public class SuggestionsCommand : ICommand
    {
        private readonly ISearchService searchService;

        private readonly IShowThings<IEnumerable<string>> showSuggestions;

        public SuggestionsCommand(ISearchService searchService, IShowThings<IEnumerable<string>> showSuggestions)
        {
            Contract.Requires(searchService != null);
            Contract.Requires(showSuggestions != null);

            this.searchService = searchService;
            this.showSuggestions = showSuggestions;
        }

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
                this.searchService.BeginSearchSuggestions(vm.SearchTerm, this.OnAfterSearchSuggestions, null);
            }
        }

        private void OnAfterSearchSuggestions(IAsyncResult result)
        {
            var suggestions = this.searchService.EndSearchSuggestions(result);

            this.showSuggestions.Show(suggestions);
        }
    }
}