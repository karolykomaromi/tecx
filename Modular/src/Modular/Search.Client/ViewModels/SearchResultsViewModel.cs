namespace Search.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Windows.Input;
    using AutoMapper;
    using Infrastructure;
    using Infrastructure.ViewModels;
    using Search.Entities;

    public class SearchResultsViewModel : ViewModel, IShowThings<IEnumerable<SearchResult>>
    {
        private readonly ICommand navigateContentCommand;
        private readonly IMappingEngine mappingEngine;
        private readonly ObservableCollection<SearchResultViewModel> results;

        public SearchResultsViewModel(ICommand navigateContentCommand, IMappingEngine mappingEngine)
        {
            Contract.Requires(navigateContentCommand != null);
            Contract.Requires(mappingEngine != null);

            this.navigateContentCommand = navigateContentCommand;
            this.mappingEngine = mappingEngine;

            this.results = new ObservableCollection<SearchResultViewModel>();
        }

        public ObservableCollection<SearchResultViewModel> Results
        {
            get { return this.results; }
        }

        public void Show(IEnumerable<SearchResult> searchResults)
        {
            Contract.Requires(searchResults != null);

            this.Results.Clear();

            foreach (SearchResult result in searchResults)
            {
                var item = new SearchResultViewModel(this.navigateContentCommand);

                this.mappingEngine.Map(result, item);

                this.Results.Add(item);
            }

            Uri destination = new Uri("SearchResultsView", UriKind.Relative);

            if (this.navigateContentCommand.CanExecute(destination))
            {
                this.navigateContentCommand.Execute(destination);
            }
        }
    }
}
