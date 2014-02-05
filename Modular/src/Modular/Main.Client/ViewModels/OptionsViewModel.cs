using System;
using System.Windows.Input;
using Infrastructure.Commands;
using Infrastructure.Events;

namespace Main.ViewModels
{
    using Infrastructure.I18n;
    using Infrastructure.Options;

    public class OptionsViewModel : Options
    {
        private readonly LanguageSelectionViewModel languageSelection;
        private readonly ThemeSelectionViewModel themeSelection;
        private readonly LocalizedString labelLanguageSelection;
        private readonly LocalizedString labelThemeSelection;
        private readonly LocalizedString labelTestConnection;

        private string notificationUrl;
        private string testConnectionReturn;

        public OptionsViewModel(ResxKey titleKey, LanguageSelectionViewModel languageSelection, ThemeSelectionViewModel themeSelection)
            : base(titleKey)
        {
            this.languageSelection = languageSelection;
            this.themeSelection = themeSelection;

            this.labelLanguageSelection = new LocalizedString(this, "LabelLanguageSelection", new ResxKey("MAIN.LABEL_LANGUAGESELECTION"), this.OnPropertyChanged);
            this.labelThemeSelection = new LocalizedString(this, "LabelThemeSelection", new ResxKey("MAIN.LABEL_THEMESELECTION"), this.OnPropertyChanged);
            this.labelTestConnection = new LocalizedString(this, "LabelTestConnection", new ResxKey("MAIN.LABEL_TESTCONNECTION"), this.OnPropertyChanged);
        }

        public string LabelLanguageSelection
        {
            get { return this.labelLanguageSelection.Value; }
        }

        public LanguageSelectionViewModel LanguageSelection
        {
            get { return this.languageSelection; }
        }

        public string LabelThemeSelection
        {
            get { return this.labelThemeSelection.Value; }
        }

        public ThemeSelectionViewModel ThemeSelection
        {
            get { return this.themeSelection; }
        }

        public string NotificationUrl
        {
            get
            {
                return this.notificationUrl;
            }

            set
            {
                if (!object.Equals(this.notificationUrl, value))
                {
                    this.OnPropertyChanging(() => this.NotificationUrl);
                    this.notificationUrl = value;
                    this.OnPropertyChanged(() => this.NotificationUrl);
                }
            }
        }

        public string LabelTestConnection
        {
            get { return this.labelTestConnection.Value; }
        }

        public ICommand TestConnectionCommand
        {
            get { throw new System.NotImplementedException(); }
        }

        public string TestConnectionReturn
        {
            get
            {
                return this.testConnectionReturn;
            }

            set
            {
                if (!object.Equals(this.testConnectionReturn, value))
                {
                    this.OnPropertyChanging(() => this.TestConnectionReturn);
                    this.testConnectionReturn = value;
                    this.OnPropertyChanged(() => this.TestConnectionReturn);
                }
            }
        }
    }

    public class TestNotificationConnectionCommand : ICommand, ISubscribeTo<CanExecuteChanged>
    {
        public event EventHandler CanExecuteChanged = delegate { };

        public bool CanExecute(object parameter)
        {
            string uri = parameter as string;

            if (!string.IsNullOrEmpty(uri))
            {
                return Uri.IsWellFormedUriString(uri, UriKind.Absolute);
            }

            return false;
        }

        public void Execute(object parameter)
        {
            string uriString = parameter as string;

            if (!string.IsNullOrEmpty(uriString) && Uri.IsWellFormedUriString(uriString, UriKind.Absolute))
            {
                
            }
        }

        public void Handle(CanExecuteChanged message)
        {
            this.CanExecuteChanged(this, EventArgs.Empty);
        }
    }
}