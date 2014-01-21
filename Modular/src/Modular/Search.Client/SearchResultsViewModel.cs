namespace Search
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using Infrastructure;
    using Search.Service;

    public class SearchResultsViewModel : ViewModel, IShowThings<IEnumerable<SearchResult>>
    {
        private readonly ObservableCollection<SearchResult> results;

        public SearchResultsViewModel()
        {
            this.results = new ObservableCollection<SearchResult>();

            this.results.Add(new SearchResult { Name = "FooBarBaz" });
        }

        public ObservableCollection<SearchResult> Results
        {
            get { return this.results; }
        }

        public void Show(IEnumerable<SearchResult> searchResults)
        {
            Contract.Requires(searchResults != null);

            this.Results.Clear();

            foreach (SearchResult result in searchResults)
            {
                this.Results.Add(result);
            }
        }
    }
}
