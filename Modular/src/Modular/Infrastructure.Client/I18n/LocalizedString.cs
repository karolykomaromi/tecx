namespace Infrastructure.I18n
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq.Expressions;
    using System.Text;

    using Infrastructure.ViewModels;

    public class LocalizedString
    {
        private readonly ViewModel viewModel;
        private readonly string propertyName;
        private readonly Action<string> notifyPropertyChanged;
        private readonly ResxKey resourceKey;

        public LocalizedString(ViewModel viewModel, string propertyName, ResxKey resourceKey, Action<string> notifyPropertyChanged)
            : this(viewModel, propertyName, resourceKey, notifyPropertyChanged, LanguageManager.Current)
        {
        }

        public LocalizedString(ViewModel viewModel, string propertyName, ResxKey resourceKey, Action<string> notifyPropertyChanged, ILanguageManager languageManager)
        {
            Contract.Requires(viewModel != null);
            Contract.Requires(!string.IsNullOrEmpty(propertyName));
            Contract.Requires(notifyPropertyChanged != null);
            Contract.Requires(languageManager != null);

            this.viewModel = viewModel;
            this.propertyName = propertyName;
            this.resourceKey = resourceKey;
            this.notifyPropertyChanged = notifyPropertyChanged;

            languageManager.LanguageChanged += this.OnLanguageChanged;
        }

        public string Value
        {
            get
            {
                return this.viewModel.ResourceManager[this.resourceKey];
            }
        }

        public static LocalizedString Create<TViewModel>(TViewModel viewModel, Expression<Func<TViewModel, string>> propertySelector, Action<string> notifyPropertyChanged)
            where TViewModel : ViewModel
        {
            return Create<TViewModel>(viewModel, propertySelector, notifyPropertyChanged, LanguageManager.Current);
        }

        public static LocalizedString Create<TViewModel>(TViewModel viewModel, Expression<Func<TViewModel, string>> propertySelector, Action<string> notifyPropertyChanged, ILanguageManager languageManager)
            where TViewModel : ViewModel
        {
            Contract.Requires(viewModel != null);
            Contract.Requires(propertySelector != null);
            Contract.Requires(notifyPropertyChanged != null);
            Contract.Requires(languageManager != null);

            MemberExpression property = (MemberExpression)propertySelector.Body;

            string propertyName = property.Member.Name;

            string rk = new StringBuilder(viewModel.GetType().FullName)
                        .Append(".")
                        .Append(propertyName)
                        .Replace(".ViewModels", string.Empty)
                        .Replace("ViewModel", string.Empty)
                        .Replace("Label", "Label_")
                        .ToString();

            ResxKey resourceKey = new ResxKey(rk);

            return new LocalizedString(viewModel, propertyName, resourceKey, notifyPropertyChanged, languageManager);
        }

        private void OnLanguageChanged(object sender, EventArgs e)
        {
            this.notifyPropertyChanged(this.propertyName);
        }
    }
}