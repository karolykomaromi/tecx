using System;
using System.Diagnostics.Contracts;
using System.Windows.Input;
using System.Windows.Threading;
using Infrastructure.Commands;
using Search.Service;

namespace Search
{
    public class SearchCommand : ICommand
    {
        private readonly ISearchService searchService;
        private readonly ICommandManager commandManager;
        private readonly Dispatcher dispatcher;

        public SearchCommand(ISearchService searchService, ICommandManager commandManager, Dispatcher dispatcher)
        {
            Contract.Requires(searchService != null);
            Contract.Requires(commandManager != null);
            Contract.Requires(dispatcher != null);

            this.searchService = searchService;
            this.commandManager = commandManager;
            this.dispatcher = dispatcher;
        }

        public bool CanExecute(object parameter)
        {
            Contract.Requires(parameter != null);

            SearchViewModel vm = parameter as SearchViewModel;

            return vm != null && !string.IsNullOrEmpty(vm.SearchTerm) && vm.SearchTerm.Length >= 3;
        }

        public event EventHandler CanExecuteChanged
        {
            add { this.commandManager.RequerySuggested += value; }
            remove { this.commandManager.RequerySuggested -= value; }
        }

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

            this.dispatcher.BeginInvoke(() =>
                {
                    vm.Results.Clear();

                    foreach (SearchResult result in results)
                    {
                        vm.Results.Add(result);
                    }
                });
        }
    }
}