namespace Search.ViewModels
{
    using Infrastructure.Events;
    using Infrastructure.I18n;
    using Infrastructure.Options;

    public class SearchOptionsViewModel : Options
    {
        private readonly LocalizedString labelIsSearchEnabled;

        private bool isSearchEnabled;

        public SearchOptionsViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            this.labelIsSearchEnabled = new LocalizedString(this, "LabelIsSearchEnabled", new ResxKey("Search.Label_IsSearchEnabled"), this.OnPropertyChanged);
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
