namespace Infrastructure.I18n
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq.Expressions;
    using System.Text;
    using Infrastructure.Events;
    using Infrastructure.ViewModels;

    public class LocalizedString : ISubscribeTo<LanguageChanged>
    {
        private readonly ViewModel viewModel;
        private readonly string propertyName;
        private readonly Action<string> notifyPropertyChanged;
        private readonly ResxKey resourceKey;

        public LocalizedString(ViewModel viewModel, string propertyName, ResxKey resourceKey, Action<string> notifyPropertyChanged)
            : this(viewModel, propertyName, resourceKey, notifyPropertyChanged, EventAggregator.Current)
        {
        }

        public LocalizedString(ViewModel viewModel, string propertyName, ResxKey resourceKey, Action<string> notifyPropertyChanged, IEventAggregator eventAggregator)
        {
            Contract.Requires(viewModel != null);
            Contract.Requires(!string.IsNullOrEmpty(propertyName));
            Contract.Requires(notifyPropertyChanged != null);
            Contract.Requires(eventAggregator != null);

            this.viewModel = viewModel;
            this.propertyName = propertyName;
            this.resourceKey = resourceKey;
            this.notifyPropertyChanged = notifyPropertyChanged;

            eventAggregator.Subscribe(this);
        }

        public string Value
        {
            get { return this.viewModel.ResourceManager[this.resourceKey]; }
        }

        public static LocalizedString Create<TViewModel>(TViewModel viewModel, Expression<Func<TViewModel, string>> propertySelector, Action<string> notifyPropertyChanged)
            where TViewModel : ViewModel
        {
            return Create<TViewModel>(viewModel, propertySelector, notifyPropertyChanged, EventAggregator.Current);
        }

        public static LocalizedString Create<TViewModel>(TViewModel viewModel, Expression<Func<TViewModel, string>> propertySelector, Action<string> notifyPropertyChanged, IEventAggregator eventAggregator)
            where TViewModel : ViewModel
        {
            Contract.Requires(viewModel != null);
            Contract.Requires(propertySelector != null);
            Contract.Requires(notifyPropertyChanged != null);
            Contract.Requires(eventAggregator != null);

            string propertyName = ReflectionHelper.GetPropertyName(propertySelector);

            string rk = new StringBuilder(viewModel.GetType().FullName)
                        .Append(".")
                        .Append(propertyName)
                        .Replace(".ViewModels", string.Empty)
                        .Replace("ViewModel", string.Empty)
                        .Replace("Label_", string.Empty)
                        .Replace("Label", string.Empty)
                        .ToString();

            ResxKey resourceKey = new ResxKey(rk);

            return new LocalizedString(viewModel, propertyName, resourceKey, notifyPropertyChanged, eventAggregator);
        }

        public void Handle(LanguageChanged message)
        {
            this.notifyPropertyChanged(this.propertyName);
        }
    }
}