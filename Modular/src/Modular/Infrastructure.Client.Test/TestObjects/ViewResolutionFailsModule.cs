namespace Infrastructure.Client.Test.TestObjects
{
    using System.Windows;
    using Microsoft.Practices.Prism.Regions;

    public class ViewResolutionFailsModule : UnityModule
    {
        public ViewResolutionFailsModule(IEntryPointInfo entryPointInfo)
            : base(entryPointInfo)
        {
        }

        protected override void ConfigureRegions(IRegionManager regionManager)
        {
            FrameworkElement view;
            this.TryGetViewFor<ViewModel2>(out view);
        }
    }
}