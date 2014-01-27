namespace Search.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Windows.Input;
    using Infrastructure;
    using Infrastructure.Events;
    using Infrastructure.I18n;
    using Infrastructure.ViewModels;
    using Search.Entities;
    using Search.Events;

    public class SearchViewModel : ViewModel
    {
        private readonly ICommand searchCommand;
        private readonly ICommand suggestionsCommand;
        private readonly ISearchService searchService;
        private readonly IEventAggregator eventAggregator;
        private readonly ObservableCollection<string> suggestions;
        private readonly LocalizedString labelSearch;

        private string searchTerm;

        public SearchViewModel(ICommand searchCommand, ICommand suggestionsCommand, ISearchService searchService, IEventAggregator eventAggregator)
        {
            Contract.Requires(searchCommand != null);
            Contract.Requires(suggestionsCommand != null);
            Contract.Requires(searchService != null);
            Contract.Requires(eventAggregator != null);

            this.searchCommand = searchCommand;
            this.suggestionsCommand = suggestionsCommand;
            this.searchService = searchService;
            this.eventAggregator = eventAggregator;

            this.suggestions = new ObservableCollection<string>();
            this.labelSearch = new LocalizedString(this, ReflectionHelper.GetPropertyName(() => this.LabelSearch), new ResxKey("SEARCH.LABEL_SEARCH"), this.OnPropertyChanged);
        }

        public string LabelSearch
        {
            get { return this.labelSearch.Value; }
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

        public void Search()
        {
            this.searchService.BeginSearch(this.SearchTerm, this.OnSearchCompleted, null);
        }

        public void SearchSuggestions()
        {
            this.searchService.BeginSearchSuggestions(this.SearchTerm, this.OnSearchSuggestionsCompleted, null);
        }

        private void OnSearchCompleted(IAsyncResult ar)
        {
            SearchResult[] searchResults = this.searchService.EndSearch(ar);

            this.eventAggregator.Publish(new DisplaySearchResults(new ReadOnlyCollection<SearchResult>(searchResults)));
        }

        private void OnSearchSuggestionsCompleted(IAsyncResult ar)
        {
            string[] searchSuggestions = this.searchService.EndSearchSuggestions(ar);
            
            this.Suggestions.Clear();

            foreach (string suggestion in suggestions)
            {
                this.Suggestions.Add(suggestion);
            }
        }
    }
}