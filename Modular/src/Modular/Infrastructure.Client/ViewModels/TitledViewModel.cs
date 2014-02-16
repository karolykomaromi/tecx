namespace Infrastructure.ViewModels
{
    using System.Diagnostics.Contracts;
    using Infrastructure.I18n;

    public abstract class TitledViewModel : ViewModel
    {
        private readonly LocalizedString title;

        protected TitledViewModel(ResourceAccessor title)
        {
            Contract.Requires(title != null);

            this.title = new LocalizedString(() => this.Title, title.GetResource, this.OnPropertyChanged);
        }

        public string Title
        {
            get { return this.title.Value; }
        }
    }
}
