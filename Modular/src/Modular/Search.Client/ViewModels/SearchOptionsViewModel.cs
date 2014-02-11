namespace Search.ViewModels
{
    using Infrastructure;
    using Infrastructure.I18n;
    using Infrastructure.Options;

    public class SearchOptionsViewModel : Options
    {
        private readonly LocalizedString labelIsSearchEnabled;

        private bool isSearchEnabled;

        public SearchOptionsViewModel(ResxKey titleKey)
            : base(titleKey)
        {
            this.labelIsSearchEnabled = new LocalizedString(this, ReflectionHelper.GetPropertyName(() => this.LabelIsSearchEnabled), new ResxKey("SEARCH.LABEL_ISSEARCHENABLED"), this.OnPropertyChanged);
            this.IsSearchEnabled = true;
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
