namespace Search.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Windows.Input;
    using Infrastructure;
    using Infrastructure.Commands;
    using Search.Service;
    using Search.ViewModels;

    public class SearchCommand : ICommand
    {
        private readonly ISearchService searchService;
        private readonly ICommandManager commandManager;
        private readonly IShowThings<IEnumerable<SearchResult>> showResults;

        public SearchCommand(ISearchService searchService, ICommandManager commandManager, IShowThings<IEnumerable<SearchResult>> showResults)
        {
            Contract.Requires(searchService != null);
            Contract.Requires(commandManager != null);
            Contract.Requires(showResults != null);

            this.searchService = searchService;
            this.commandManager = commandManager;
            this.showResults = showResults;
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
                this.searchService.BeginSearch(vm.SearchTerm, this.OnAfterSearch, null);
            }
        }

        private void OnAfterSearch(IAsyncResult ar)
        {
            var results = this.searchService.EndSearch(ar);

            this.showResults.Show(results);
        }
    }
}