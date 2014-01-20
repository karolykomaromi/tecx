using System;
using System.Diagnostics.Contracts;
using System.Windows.Input;
using Search.Service;

namespace Search
{
    public class SearchCommand : ICommand
    {
        private readonly ISearchService searchService;

        public SearchCommand(ISearchService searchService)
        {
            Contract.Requires(searchService != null);

            this.searchService = searchService;
        }

        public bool CanExecute(object parameter)
        {
            Contract.Requires(parameter != null);

            SearchViewModel vm = parameter as SearchViewModel;

            return vm != null && !string.IsNullOrEmpty(vm.SearchTerm) && vm.SearchTerm.Length >= 3;
        }

        public event EventHandler CanExecuteChanged = delegate { };

        public void Execute(object parameter)
        {
            Contract.Requires(parameter != null);

            SearchViewModel vm = parameter as SearchViewModel;

            if (vm != null &&
                !string.IsNullOrEmpty(vm.SearchTerm) &&
                vm.SearchTerm.Length >= 3)
            {
                this.searchService.BeginSearch(vm.SearchTerm, this.OnAfterSearch, vm);
            }
        }

        private void OnAfterSearch(IAsyncResult ar)
        {
            var results = this.searchService.EndSearch(ar);

            SearchViewModel vm = (SearchViewModel)ar.AsyncState;

            vm.Results.Clear();

            foreach (SearchResult result in results)
            {
                vm.Results.Add(result);
            }
        }
    }
}