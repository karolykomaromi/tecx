namespace Search
{
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Windows.Input;
    using Infrastructure;
    using Search.Service;

    public class SearchViewModel : ViewModel
    {
        private readonly ICommand searchCommand;
        private readonly ICommand searchSuggestionCommand;
        private readonly ObservableCollection<string> suggestions;
        private readonly ObservableCollection<SearchResult> results;

        private string searchTerm;

        public SearchViewModel(ICommand searchCommand, ICommand searchSuggestionCommand)
        {
            Contract.Requires(searchCommand != null);
            Contract.Requires(searchSuggestionCommand != null);

            this.searchCommand = searchCommand;
            this.searchSuggestionCommand = searchSuggestionCommand;

            this.suggestions = new ObservableCollection<string>();
            this.results = new ObservableCollection<SearchResult>();
        }

        public ICommand SearchCommand
        {
            get { return this.searchCommand; }
        }

        public ICommand SearchSuggestionCommand
        {
            get { return this.searchSuggestionCommand; }
        }

        public ObservableCollection<SearchResult> Results
        {
            get { return this.results; }
        }

        public ObservableCollection<string> Suggestions
        {
            get { return this.suggestions; }
        }

        public string SearchTerm
        {
            get
            {
                return this.searchTerm;
            }

            set
            {
                if (this.searchTerm != value)
                {
                    this.OnPropertyChanging(() => this.SearchTerm);
                    this.searchTerm = value;
                    this.OnPropertyChanged(() => this.SearchTerm);
                }
            }
        }
    }
}