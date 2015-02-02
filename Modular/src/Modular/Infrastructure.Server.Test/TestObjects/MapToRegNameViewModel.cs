using System.Windows.Input;

namespace Infrastructure.Server.Test.TestObjects
{
    public class MapToRegNameViewModel
    {
        public ICommand SearchCommand { get; set; }
        public ICommand SuggestionsCommand { get; set; }
        public ISearchService SearchService { get; set; }

        public MapToRegNameViewModel(ICommand searchCommand, ICommand suggestionsCommand, ISearchService searchService)
        {
            SearchCommand = searchCommand;
            SuggestionsCommand = suggestionsCommand;
            SearchService = searchService;
        }
    }
}