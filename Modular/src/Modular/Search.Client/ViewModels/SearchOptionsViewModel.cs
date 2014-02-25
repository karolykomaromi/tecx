namespace Search.ViewModels
{
    using Infrastructure.I18n;
    using Infrastructure.Options;
    using Search.Assets.Resources;

    public class SearchOptionsViewModel : Options
    {
        private readonly LocalizedString title;
        private readonly LocalizedString labelIsSearchEnabled;

        private bool isSearchEnabled;

        public SearchOptionsViewModel()
        {
            this.title = new LocalizedString(() => this.Title, () => Labels.SearchOptions, this.OnPropertyChanged);
            this.labelIsSearchEnabled = new LocalizedString(() => this.LabelIsSearchEnabled, () => Labels.IsSearchEnabled, this.OnPropertyChanged);
            this.IsSearchEnabled = true;
        }

        public override string Title
        {
            get { return this.title.Value; }
        }

        public bool IsSearchEnabled
        {
            get
            {
                return this.isSearchEnabled;
            }

            set
            {
                if (this.isSearchEnabled != value)
                {
                    this.OnPropertyChanging(() => this.IsSearchEnabled);
                    this.isSearchEnabled = value;
                    this.OnPropertyChanged(() => this.IsSearchEnabled);
                    this.EventAggregator.Publish(new OptionsChanged<SearchOptionsViewModel>(this, Option.Create(this, o => o.IsSearchEnabled)));
                }
            }
        }

        public string LabelIsSearchEnabled
        {
            get { return this.labelIsSearchEnabled.Value; }
        }
    }
}
