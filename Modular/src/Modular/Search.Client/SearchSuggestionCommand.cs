namespace Search
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Windows.Input;
    using System.Windows.Threading;
    using Search.Service;

    public class SearchSuggestionCommand : ICommand
    {
        private readonly ISearchService searchService;
        private readonly Dispatcher dispatcher;

        public SearchSuggestionCommand(ISearchService searchService, Dispatcher dispatcher)
        {
            Contract.Requires(searchService != null);
            Contract.Requires(dispatcher != null);

            this.searchService = searchService;
            this.dispatcher = dispatcher;
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
                this.searchService.BeginSearchSuggestions(vm.SearchTerm, this.OnAfterSearchSuggestions, vm);
            }
        }

        private void OnAfterSearchSuggestions(IAsyncResult result)
        {
            var suggestions = this.searchService.EndSearchSuggestions(result);

            SearchViewModel vm = (SearchViewModel)result.AsyncState;

            this.dispatcher.BeginInvoke(() =>
                {
                    vm.Suggestions.Clear();

                    foreach (string suggestion in suggestions)
                    {
                        vm.Suggestions.Add(suggestion);
                    }
                });
        }
    }
}