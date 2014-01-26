namespace Infrastructure.ViewModels
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Windows;
    using System.Windows.Input;
    using Infrastructure.Commands;
    using Infrastructure.I18n;
    using Microsoft.Practices.Prism.Regions;

    public class NavigationBuilder
    {
        private ViewModel target;

        private INavigateAsync region;

        private ResxKey resourceKey;

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

            NavigationViewModel viewModel = new NavigationViewModel(navigationCommand, this.resourceKey)
                {
                    ResourceManager = this.target.ResourceManager,
                    Destination = new Uri(this.target.GetType().Name.Replace("Model", string.Empty), UriKind.Relative)
                };

            return viewModel;
        }
    }
}