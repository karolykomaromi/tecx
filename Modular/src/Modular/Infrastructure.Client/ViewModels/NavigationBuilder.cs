namespace Infrastructure.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Windows;
    using System.Windows.Input;
    using Infrastructure.Commands;
    using Infrastructure.I18n;
    using Microsoft.Practices.Prism;
    using Microsoft.Practices.Prism.Regions;

    public class NavigationBuilder
    {
        private readonly List<KeyValuePair<string, string>> parameters;

        private ViewModel target;

        private INavigateAsync region;

        private ResxKey resourceKey;

        public NavigationBuilder()
        {
            this.parameters = new List<KeyValuePair<string, string>>();
        }

        public static implicit operator NavigationViewModel(NavigationBuilder navigate)
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

        public NavigationBuilder WithLabel(ResxKey resourceKey)
        {
            Contract.Requires(resourceKey != ResxKey.Empty);

            this.resourceKey = resourceKey;
            return this;
        }

        public NavigationViewModel Build()
        {
            ICommand navigationCommand = new NavigationCommand(this.region);
            
            UriQuery query = new UriQuery();

            foreach (var parameter in this.parameters)
            {
                query.Add(parameter.Key, parameter.Value);
            }

            NavigationViewModel viewModel = new NavigationViewModel(navigationCommand, this.resourceKey)
                {
                    ResourceManager = this.target.ResourceManager,
                    Destination = new Uri(this.target.GetType().Name.Replace("Model", string.Empty) + query, UriKind.Relative)
                };

            return viewModel;
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