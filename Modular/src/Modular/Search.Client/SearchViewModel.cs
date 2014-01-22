namespace Search
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Windows.Input;
    using Infrastructure;

    public class SearchViewModel : ViewModel, IShowThings<IEnumerable<string>>
    {
        private readonly ICommand searchCommand;
        private readonly ICommand suggestionsCommand;
        private readonly ObservableCollection<string> suggestions;

        private string searchTerm;

        public SearchViewModel(ICommand searchCommand, ICommand suggestionsCommand)
        {
            Contract.Requires(searchCommand != null);
            Contract.Requires(suggestionsCommand != null);

            this.searchCommand = searchCommand;
            this.suggestionsCommand = suggestionsCommand;

            this.suggestions = new ObservableCollection<string>();
        }

        public ICommand SearchCommand
        {
            get { return this.searchCommand; }
        }

        public ICommand SuggestionsCommand
        {
            get { return this.suggestionsCommand; }
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

        public void Show(IEnumerable<string> suggestions)
        {
            this.Suggestions.Clear();

            foreach (string suggestion in suggestions)
            {
                this.Suggestions.Add(suggestion);
            }
        }
    }
}