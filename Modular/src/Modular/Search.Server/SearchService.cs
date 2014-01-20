using Search.Entities;

namespace Search
{
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
                    new SearchResult{Name = "Foo"},
                    new SearchResult{Name ="Bar"},
                    new SearchResult{Name = "Baz"}
                };
        }
    }
}