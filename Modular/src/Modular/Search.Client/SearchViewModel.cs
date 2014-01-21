namespace Search
{
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Windows.Input;
    using Infrastructure;

    public class SearchViewModel : ViewModel
    {
        private readonly ICommand searchCommand;
        private readonly ICommand searchSuggestionCommand;
        private readonly ObservableCollection<string> suggestions;

        private string searchTerm;

        public SearchViewModel(ICommand searchCommand, ICommand searchSuggestionCommand)
        {
            Contract.Requires(searchCommand != null);
            Contract.Requires(searchSuggestionCommand != null);

            this.searchCommand = searchCommand;
            this.searchSuggestionCommand = searchSuggestionCommand;

            this.suggestions = new ObservableCollection<string>();
        }

        public ICommand SearchCommand
        {
            get { return this.searchCommand; }
        }

        public ICommand SearchSuggestionCommand
        {
            get { return this.searchSuggestionCommand; }
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