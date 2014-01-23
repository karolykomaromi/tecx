namespace Infrastructure.Client.Test.TestObjects
{
    using System.Windows;
    using Microsoft.Practices.Prism.Regions;

    public class NoViewModule : UnityModule
    {
        public NoViewModule(IEntryPointInfo entryPointInfo)
            : base(entryPointInfo)
        {
        }

        protected override void ConfigureRegions(IRegionManager regionManager)
        {
            FrameworkElement view;
            this.TryGetViewFor<NoView>(out view);
        }
    }
}