namespace Infrastructure.ViewModels
{
    using System.Diagnostics.Contracts;
    using System.Threading;
    using System.Windows;
    using System.Windows.Markup;
    using Infrastructure.Commands;
    using Infrastructure.Events;
    using Infrastructure.I18n;
    using Infrastructure.Meta;

    public abstract class ViewModel : NotificationBase, ISubscribeTo<LanguageChanging>
    {
        private IEventAggregator eventAggregator;
        private bool isEnabled;
        private Visibility visibility;
        private XmlLanguage language;

        protected ViewModel()
        {
            this.eventAggregator = new NullEventAggregator();

            this.Visibility = Visibility.Visible;
            this.IsEnabled = true;
            this.Language = XmlLanguage.GetLanguage(Thread.CurrentThread.CurrentUICulture.Name);
        }
        
        [PropertyMeta(IsListViewRelevant = false)]
        public IEventAggregator EventAggregator
        {
            get
            {
                return this.eventAggregator;
            }

            set
            {
                Contract.Requires(value != null);
                
                this.eventAggregator = value;
            }
        }

        [PropertyMeta(IsListViewRelevant = false)]
        public bool IsEnabled
        {
            get
            {
                return this.isEnabled;
            }

            set
            {
                if (this.isEnabled != value)
                {
                    this.OnPropertyChanging(() => this.IsEnabled);
                    this.isEnabled = value;
                    this.OnPropertyChanged(() => this.IsEnabled);
                }
            }
        }

        [PropertyMeta(IsListViewRelevant = false)]
        public Visibility Visibility
        {
            get
            {
                return this.visibility;
            }

            set
            {
                if (this.visibility != value)
                {
                    this.OnPropertyChanging(() => this.Visibility);
                    this.visibility = value;
                    this.OnPropertyChanged(() => this.Visibility);
                }
            }
        }

        [PropertyMeta(IsListViewRelevant = false)]
        public XmlLanguage Language
        {
            get
            {
                return this.language;
            }

            set
            {
                if (this.language != value)
                {
                    this.OnPropertyChanging(() => this.Language);
                    this.language = value;
                    this.OnPropertyChanged(() => this.Language);
                }
            }
        }
        
        public void Enable()
        {
            this.IsEnabled = true;
        }

        public void Disable()
        {
            this.IsEnabled = false;
        }

        public void Hide()
        {
            this.Visibility = Visibility.Collapsed;
        }

        public void Show()
        {
            this.Visibility = Visibility.Visible;
        }

        public void Handle(LanguageChanging message)
        {
            this.Language = XmlLanguage.GetLanguage(message.Culture.Name);
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            this.EventAggregator.Publish(new CanExecuteChanged());
        }
    }
}
