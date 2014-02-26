namespace Infrastructure.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Windows.Input;
    using Microsoft.Practices.Prism.Regions;

    public class NavigateContentCommand : ICommand
    {
        private readonly IRegionManager regionManager;

        public NavigateContentCommand(IRegionManager regionManager)
        {
            Contract.Requires(regionManager != null);

            this.regionManager = regionManager;
        }

        public event EventHandler CanExecuteChanged = delegate { };

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Contract.Requires(parameter != null);

            Uri destination = parameter as Uri;

            if (destination != null)
            {
                IRegion content = this.regionManager.Regions[RegionNames.Shell.Content];

                if (content != null)
                {
                    content.RequestNavigate(destination);
                }
            }
        }
    }
}