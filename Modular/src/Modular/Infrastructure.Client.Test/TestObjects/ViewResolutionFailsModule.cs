﻿namespace Infrastructure.Client.Test.TestObjects
{
    using System.Windows.Controls;
    using Infrastructure.Modularity;
    using Microsoft.Practices.Prism.Logging;
    using Microsoft.Practices.Prism.Regions;
    using Microsoft.Practices.Unity;

    public class ViewResolutionFailsModule : UnityModule
    {
        public ViewResolutionFailsModule(IUnityContainer container, ILoggerFacade logger, IModuleInitializer initializer)
            : base(container, logger, initializer)
        {
        }

        protected override void ConfigureRegions(IRegionManager regionManager)
        {
            Control view;
            this.TryGetViewFor<ViewModel2>(out view);
        }
    }
}