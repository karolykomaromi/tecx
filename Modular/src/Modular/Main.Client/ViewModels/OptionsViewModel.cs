namespace Main.ViewModels
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Windows;
    using System.Windows.Input;
    using Infrastructure.I18n;
    using Infrastructure.Options;
    using Main.Assets.Resources;

    public class OptionsViewModel : Options
    {
        private readonly LanguageSelectionViewModel languageSelection;
        private readonly ThemeSelectionViewModel themeSelection;
        private readonly AppInfoViewModel appInfo;
        private readonly LocalizedString labelLanguageSelection;
        private readonly LocalizedString labelThemeSelection;
        private readonly LocalizedString labelTestConnection;
        private readonly LocalizedString labelNotificationUrl;
        private readonly LocalizedString labelAppInfo;
        private readonly ICommand testNotificationCommand;

        private string notificationUrl;
        private string testConnectionReturn;

        public OptionsViewModel(ResourceAccessor title, LanguageSelectionViewModel languageSelection, ThemeSelectionViewModel themeSelection, AppInfoViewModel appInfo, ICommand testNotificationConnectionCommand)
            : base(title)
        {
            Contract.Requires(languageSelection != null);
            Contract.Requires(themeSelection != null);
            Contract.Requires(appInfo != null);
            Contract.Requires(testNotificationConnectionCommand != null);

            this.languageSelection = languageSelection;
            this.themeSelection = themeSelection;
            this.appInfo = appInfo;
            this.testNotificationCommand = testNotificationConnectionCommand;

            this.labelLanguageSelection = new LocalizedString(() => this.LabelLanguageSelection, () => Labels.LanguageSelection, this.OnPropertyChanged);
            this.labelThemeSelection = new LocalizedString(() => this.LabelThemeSelection, () => Labels.ThemeSelection, this.OnPropertyChanged);
            this.labelTestConnection = new LocalizedString(() => this.LabelTestConnection, () => Labels.TestConnection, this.OnPropertyChanged);
            this.labelNotificationUrl = new LocalizedString(() => this.LabelNotificationUrl, () => Labels.NotificationUrl, this.OnPropertyChanged);
            this.labelAppInfo = new LocalizedString(() => this.LabelAppInfo, () => Labels.NotificationUrl, this.OnPropertyChanged);

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

        public AppInfoViewModel AppInfo
        {
            get { return this.appInfo; }
        }

        public string LabelThemeSelection
        {
            get { return this.labelThemeSelection.Value; }
        }

        public string LabelAppInfo
        {
            get { return this.labelAppInfo.Value; }
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