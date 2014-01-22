namespace Search
{
    using System;
    using Search.Entities;

    public class SearchService : ISearchService
    {
        public string[] SearchSuggestions(string searchTerm)
        {
            return new[] { "abc123", "def456", "ghi789" };
        }

        public SearchResult[] Search(string searchTerm)
        {
            return new[]
                { 
                    new SearchResult { Name = "Foo", FoundSearchTermIn = "Lorem ipsum...", Uri = new Uri("ProductDetailsView?id=1", UriKind.Relative) },
                    new SearchResult { Name = "Bar", FoundSearchTermIn = "Lorem ipsum...", Uri = new Uri("ProductDetailsView?id=2", UriKind.Relative) },
                    new SearchResult { Name = "Baz", FoundSearchTermIn = "Lorem ipsum...", Uri = new Uri("ProductDetailsView?id=3", UriKind.Relative) }
                };
        }
    }
}