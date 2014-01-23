namespace Infrastructure.Client.Test.TestObjects
{
    using System.Windows;
    using Microsoft.Practices.Prism.Regions;

    public class ViewModelResolutionFailsModule : UnityModule
    {
        public ViewModelResolutionFailsModule(IEntryPointInfo entryPointInfo)
            : base(entryPointInfo)
        {
        }

        protected override void ConfigureRegions(IRegionManager regionManager)
        {
            FrameworkElement view;
            this.TryGetViewFor<ViewModel1>(out view);
        }
    }
}