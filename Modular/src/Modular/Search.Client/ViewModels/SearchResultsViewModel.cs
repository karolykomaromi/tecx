namespace Search.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Windows.Input;
    using AutoMapper;
    using Infrastructure.Events;
    using Infrastructure.I18n;
    using Infrastructure.Options;
    using Infrastructure.ViewModels;
    using Search.Assets.Resources;
    using Search.Entities;
    using Search.Events;

    public class SearchResultsViewModel : TitledViewModel, ISubscribeTo<DisplaySearchResults>, ISubscribeTo<IOptionsChanged<SearchOptionsViewModel>>
    {
        private readonly ICommand navigateContentCommand;
        private readonly IMappingEngine mappingEngine;
        private readonly ObservableCollection<SearchResultViewModel> results;
        private readonly LocalizedString title;

        public SearchResultsViewModel(ICommand navigateContentCommand, IMappingEngine mappingEngine)
        {
            Contract.Requires(navigateContentCommand != null);
            Contract.Requires(mappingEngine != null);

            this.title = new LocalizedString(() => this.Title, () => Labels.SearchResults, this.OnPropertyChanged);
            this.navigateContentCommand = navigateContentCommand;
            this.mappingEngine = mappingEngine;

            this.results = new ObservableCollection<SearchResultViewModel>();
        }

        public override string Title
        {
            get { return this.title.Value; }
        }

        public ObservableCollection<SearchResultViewModel> Results
        {
            get { return this.results; }
        }

        public void Handle(DisplaySearchResults message)
        {
            Contract.Requires(message != null);
            Contract.Requires(message.SearchResults != null);

            this.Results.Clear();

            foreach (SearchResult result in message.SearchResults)
            {
                var item = new SearchResultViewModel(this.navigateContentCommand);

                this.mappingEngine.Map(result, item);

                this.Results.Add(item);
            }

            Uri destination = new Uri("SearchResultsView", UriKind.Relative);

            if (this.navigateContentCommand.CanExecute(destination))
            {
                this.navigateContentCommand.Execute(destination);
            }
        }

        public void Handle(IOptionsChanged<SearchOptionsViewModel> message)
        {
            var options = message.Options;

            if (options.IsSearchEnabled)
            {
                this.Show();
            }
            else
            {
                this.Hide();
            }
        }
    }
}
