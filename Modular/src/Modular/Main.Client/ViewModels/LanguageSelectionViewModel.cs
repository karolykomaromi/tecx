namespace Main.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Windows.Input;
    using Infrastructure.I18n;
    using Infrastructure.ViewModels;

    public class LanguageSelectionViewModel : ViewModel
    {
        private readonly ICommand changeLanguageCommand;
        private readonly ILanguageManager languageManager;
        private readonly ObservableCollection<CultureInfo> availableLanguages;

        private CultureInfo selectedLanguage;

        public LanguageSelectionViewModel(ICommand changeLanguageCommand, ILanguageManager languageManager)
        {
            Contract.Requires(changeLanguageCommand != null);

            this.changeLanguageCommand = changeLanguageCommand;
            this.languageManager = languageManager;
            this.availableLanguages = new ObservableCollection<CultureInfo>
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("de-DE")
                };

            this.SelectedLanguage = this.languageManager.CurrentCulture;
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
