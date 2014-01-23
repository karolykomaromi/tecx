namespace Search.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Windows.Input;
    using Infrastructure;
    using Infrastructure.ViewModels;
    using Search.Entities;

    public class SearchResultsViewModel : ViewModel, IShowThings<IEnumerable<SearchResult>>
    {
        private readonly ICommand navigateContentCommand;
        private readonly ObservableCollection<SearchResultItemViewModel> results;

        public SearchResultsViewModel(ICommand navigateContentCommand)
        {
            Contract.Requires(navigateContentCommand != null);

            this.navigateContentCommand = navigateContentCommand;

            this.results = new ObservableCollection<SearchResultItemViewModel>();
        }

        public ObservableCollection<SearchResultItemViewModel> Results
        {
            get { return this.results; }
        }

        public void Show(IEnumerable<SearchResult> searchResults)
        {
            Contract.Requires(searchResults != null);

            this.Results.Clear();

            foreach (SearchResult result in searchResults)
            {
                var item = new SearchResultItemViewModel(this.navigateContentCommand)
                    {
                        FoundSearchTermIn = result.FoundSearchTermIn,
                        Name = result.Name,
                        Uri = result.Uri
                    };

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
