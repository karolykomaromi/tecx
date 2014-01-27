using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using Search.Entities;

namespace Search.Events
{
    public class DisplaySearchResults
    {
        private readonly ReadOnlyCollection<SearchResult> searchResults;

        public DisplaySearchResults(ReadOnlyCollection<SearchResult> searchResults)
        {
            Contract.Requires(searchResults != null);

            this.searchResults = searchResults;
        }

        public ReadOnlyCollection<SearchResult> SearchResults
        {
            get { return this.searchResults; }
        }
    }
}