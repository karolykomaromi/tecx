namespace Search
{
    using System;
    using Search.Entities;

    public class SearchService : ISearchService
    {
        public string[] SearchSuggestions(string searchTerm)
        {
            return new[]
                   {
                       searchTerm + "123", 
                       "FOO" + searchTerm + "BAR", 
                       searchTerm + searchTerm + searchTerm
                   };
        }

        public SearchResult[] Search(string searchTerm)
        {
            return new[]
                { 
                    new SearchResult { Name = "Foo", FoundSearchTermIn = "Lorem ipsum...", Uri = new Uri("IngredientDetailsView?id=1", UriKind.Relative) },
                    new SearchResult { Name = "Bar", FoundSearchTermIn = "Lorem ipsum...", Uri = new Uri("RecipeDetailsView?id=2", UriKind.Relative) },
                    new SearchResult { Name = "Baz", FoundSearchTermIn = "Lorem ipsum...", Uri = new Uri("IngredientDetailsView?id=3", UriKind.Relative) }
                };
        }
    }
}