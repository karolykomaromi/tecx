namespace Main.ViewModels
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using Infrastructure.I18n;
    using Infrastructure.Options;

    public class OptionsViewModel : Options
    {
        private readonly LanguageSelectionViewModel languageSelection;
        private readonly ThemeSelectionViewModel themeSelection;
        private readonly LocalizedString labelLanguageSelection;
        private readonly LocalizedString labelThemeSelection;
        private readonly LocalizedString labelTestConnection;
        private readonly LocalizedString labelNotificationUrl;
        private readonly ICommand testNotificationCommand;

        private string notificationUrl;
        private string testConnectionReturn;

        public OptionsViewModel(ResxKey titleKey, LanguageSelectionViewModel languageSelection, ThemeSelectionViewModel themeSelection, ICommand testNotificationCommand)
            : base(titleKey)
        {
            this.languageSelection = languageSelection;
            this.themeSelection = themeSelection;
            this.testNotificationCommand = testNotificationCommand;

            this.labelLanguageSelection = new LocalizedString(this, "LabelLanguageSelection", new ResxKey("MAIN.LABEL_LANGUAGESELECTION"), this.OnPropertyChanged);
            this.labelThemeSelection = new LocalizedString(this, "LabelThemeSelection", new ResxKey("MAIN.LABEL_THEMESELECTION"), this.OnPropertyChanged);
            this.labelTestConnection = new LocalizedString(this, "LabelTestConnection", new ResxKey("MAIN.LABEL_TESTCONNECTION"), this.OnPropertyChanged);
            this.labelNotificationUrl = new LocalizedString(this, "LabelNotificationUrl", new ResxKey("MAIN.LABEL_NOTIFICATIONURL"), this.OnPropertyChanged);

            Uri source = Application.Current.Host.Source;

            this.NotificationUrl = source.AbsoluteUri.Replace(source.AbsolutePath, string.Empty) + "/signalr/hubs/";
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

        public ICommand TestNotificationCommand
        {
            get { return this.testNotificationCommand; }
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

        public string LabelNotificationUrl
        {
            get { return this.labelNotificationUrl.Value; }
        }
    }
}