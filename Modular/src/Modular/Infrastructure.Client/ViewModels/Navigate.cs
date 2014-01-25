namespace Infrastructure.ViewModels
{
    using System.Diagnostics.Contracts;
    using System.Windows;
    using System.Windows.Input;
    using Infrastructure.Commands;
    using Infrastructure.I18n;
    using Microsoft.Practices.Prism.Regions;

    public class Navigate
    {
        private FrameworkElement target;

        private INavigateAsync region;

        private ResxKey resourceKey;

        public static implicit operator NavigationViewModel(Navigate navigate)
        {
            Contract.Requires(navigate != null);

            return navigate.Build();
        }

        public Navigate ToView(FrameworkElement target)
        {
            this.target = target;
            return this;
        }

        public Navigate InRegion(INavigateAsync region)
        {
            this.region = region;
            return this;
        }

        public Navigate WithLabel(ResxKey resourceKey)
        {
            this.resourceKey = resourceKey;
            return this;
        }

        public NavigationViewModel Build()
        {
            ICommand navigationCommand = new NavigationCommand(this.region);

            NavigationViewModel viewModel = new NavigationViewModel(navigationCommand, this.resourceKey);

            ViewModel targetViewModel = this.target.DataContext as ViewModel;

            if (targetViewModel != null)
            {
                viewModel.ResourceManager = targetViewModel.ResourceManager;
            }

            return viewModel;
        }
    }
}