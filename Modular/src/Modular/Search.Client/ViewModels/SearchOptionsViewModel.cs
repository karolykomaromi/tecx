namespace Search.ViewModels
{
    using System;
    using Infrastructure.I18n;
    using Infrastructure.Options;
    using Search.Assets.Resources;

    public class SearchOptionsViewModel : Options
    {
        private readonly LocalizedString labelIsSearchEnabled;

        private bool isSearchEnabled;

        public SearchOptionsViewModel(ResourceAccessor title)
            : base(title)
        {
            this.labelIsSearchEnabled = new LocalizedString(() => this.LabelIsSearchEnabled, () => Labels.IsSearchEnabled, this.OnPropertyChanged);
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
