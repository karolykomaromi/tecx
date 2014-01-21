namespace Search
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Windows.Input;
    using Infrastructure.Commands;
    using Search.Service;

    public class SearchCommand : ICommand
    {
        private readonly ISearchService searchService;
        private readonly ICommandManager commandManager;
        private readonly IShowSearchResults showResults;

        public SearchCommand(ISearchService searchService, ICommandManager commandManager, IShowSearchResults showResults)
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
                this.searchService.BeginSearch(vm.SearchTerm, this.OnAfterSearch, this.showResults);
            }
        }

        private void OnAfterSearch(IAsyncResult ar)
        {
            var results = this.searchService.EndSearch(ar);

            this.showResults.ShowSearchResults(results);
        }
    }
}