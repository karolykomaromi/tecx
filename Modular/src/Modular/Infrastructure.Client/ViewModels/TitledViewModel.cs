namespace Infrastructure.ViewModels
{
    using System.Diagnostics.Contracts;
    using Infrastructure.I18n;

    public abstract class TitledViewModel : ViewModel
    {
        private readonly LocalizedString title;

        protected TitledViewModel(ResxKey titleKey)
        {
            Contract.Requires(titleKey != ResxKey.Empty);

            this.title = new LocalizedString(this, "Title", titleKey, this.OnPropertyChanged);
        }

        public string Title
        {
            get { return this.title.Value; }
        }
    }
}
