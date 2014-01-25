namespace Main.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Windows.Input;
    using Infrastructure.ViewModels;

    public class LanguageSelectionViewModel : ViewModel
    {
        private readonly ICommand changeLanguageCommand;
        private readonly ObservableCollection<CultureInfo> availableLanguages;

        private CultureInfo selectedLanguage;

        public LanguageSelectionViewModel(ICommand changeLanguageCommand)
        {
            Contract.Requires(changeLanguageCommand != null);

            this.changeLanguageCommand = changeLanguageCommand;
            this.availableLanguages = new ObservableCollection<CultureInfo>
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("de-DE")
                };
        }

        public ObservableCollection<CultureInfo> AvailableLanguages
        {
            get { return this.availableLanguages; }
        }

        public CultureInfo SelectedLanguage
        {
            get
            {
                return this.selectedLanguage;
            }

            set
            {
                if (!object.Equals(this.selectedLanguage, value))
                {
                    this.OnPropertyChanging(() => this.SelectedLanguage);
                    this.selectedLanguage = value;
                    this.OnPropertyChanged(() => this.SelectedLanguage);
                }
            }
        }

        public ICommand ChangeLanguageCommand
        {
            get { return this.changeLanguageCommand; }
        }
    }
}
