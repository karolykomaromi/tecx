namespace Infrastructure.Views
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Windows;
    using System.Windows.Input;
    using Infrastructure.Commands;
    using Infrastructure.I18n;
    using Infrastructure.Options;
    using Infrastructure.ViewModels;
    using Microsoft.Practices.Prism;
    using Microsoft.Practices.Prism.Regions;

    public class NavigationBuilder
    {
        private readonly List<KeyValuePair<string, string>> parameters;
        private ViewModel target;
        private INavigateAsync region;
        private Action<IOptionsChanged<IOptions>, NavigationViewModel> handleOptionsChanged;
        private Func<string> title;

        public NavigationBuilder()
        {
            this.parameters = new List<KeyValuePair<string, string>>();
        }

        public static implicit operator NavigationView(NavigationBuilder navigate)
        {
            Contract.Requires(navigate != null);

            return navigate.Build();
        }

        public NavigationBuilder ToView(FrameworkElement target)
        {
            Contract.Requires(target != null);
            Contract.Requires(target.DataContext != null);
            Contract.Requires(target.DataContext is ViewModel);

            this.target = (ViewModel)target.DataContext;

            return this;
        }

        public NavigationBuilder InRegion(INavigateAsync region)
        {
            Contract.Requires(region != null);

            this.region = region;
            return this;
        }

        public NavigationBuilder WithTitle(Func<string> title)
        {
            Contract.Requires(title != null);

            this.title = title;

            return this;
        }

        public NavigationBuilder OnOptionsChanged(Action<IOptionsChanged<IOptions>, NavigationViewModel> handleOptionsChanged)
        {
            this.handleOptionsChanged = handleOptionsChanged;

            return this;
        }

        public NavigationView Build()
        {
            ICommand navigationCommand = new NavigationCommand(this.region);

            UriQuery query = new UriQuery();

            foreach (var parameter in this.parameters)
            {
                query.Add(parameter.Key, parameter.Value);
            }

            NavigationViewModel viewModel = new NavigationViewModel(
                navigationCommand, 
                new ResourceAccessor(this.title), 
                new Uri(this.target.GetType().Name.Replace("Model", string.Empty) + query, UriKind.Relative),
                this.handleOptionsChanged)
                {
                    EventAggregator = this.target.EventAggregator
                };

            this.target.EventAggregator.Subscribe(viewModel);

            NavigationView view = new NavigationView { DataContext = viewModel };

            ViewModelBinder.Bind(view, viewModel);

            return view;
        }

        public NavigationBuilder WithParameter(string parameterName, string parameterValue)
        {
            Contract.Requires(!string.IsNullOrEmpty(parameterName));
            Contract.Requires(!string.IsNullOrEmpty(parameterValue));

            this.parameters.Add(new KeyValuePair<string, string>(parameterName, parameterValue));

            return this;
        }
    }
}