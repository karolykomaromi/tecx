namespace Search.ViewModels
{
    using Infrastructure.Events;
    using Infrastructure.Options;

    public class SearchOptionsViewModel : Options
    {
        public SearchOptionsViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
        }

        private bool isSearchEnabled;

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
    }
}
