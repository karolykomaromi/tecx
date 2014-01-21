namespace Search
{
    using System.Collections.Generic;
    using Search.Service;

    public interface IShowSearchResults
    {
        void ShowSearchResults(IEnumerable<SearchResult> results);
    }
}