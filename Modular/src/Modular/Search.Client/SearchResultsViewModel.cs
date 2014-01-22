namespace Search
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using Infrastructure;
    using Infrastructure.Commands;
    using Microsoft.Practices.Prism;
    using Search.Service;

    public class SearchResultsViewModel : ViewModel, IShowThings<IEnumerable<SearchResult>>
    {
        private readonly ObservableCollection<SearchResultItemViewModel> results;

        public SearchResultsViewModel()
        {
            this.results = new ObservableCollection<SearchResultItemViewModel>();

            UriQuery query = new UriQuery { { "id", "4711" }, { "type", "Product" } };

            Uri uri = new Uri("DetailsView" + query, UriKind.Relative);

            this.results.Add(new SearchResultItemViewModel(new NullCommand()) { Name = "FooBarBaz", FoundSearchTermIn = "Lorem ipsum...", Uri = uri });
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
                var item = new SearchResultItemViewModel(new NullCommand())
                    {
                        FoundSearchTermIn = result.FoundSearchTermIn,
                        Name = result.Name,
                        Uri = result.Uri
                    };

                this.Results.Add(item);
            }
        }
    }
}
