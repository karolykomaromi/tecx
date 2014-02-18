namespace Main
{
    using System.Windows.Controls;
    using System.Windows.Input;
    using Assets.Resources;
    using Commands;
    using Infrastructure;
    using Infrastructure.I18n;
    using Infrastructure.Modularity;
    using Infrastructure.UnityExtensions.Injection;
    using Infrastructure.Views;
    using Main.ViewModels;
    using Microsoft.Practices.Prism.Logging;
    using Microsoft.Practices.Prism.Regions;
    using Microsoft.Practices.Unity;

    public class Module : UnityModule
    {
        /// <summary>
        /// Main
        /// </summary>
        public static readonly string Name = "Main";

        public Module(IUnityContainer container, ILoggerFacade logger, IModuleTracker moduleTracker, IModuleInitializer initializer)
            : base(container, logger, moduleTracker, initializer)
        {
        }

        public override string ModuleName
        {
            get { return Name; }
        }

        protected override void ConfigureRegions(IRegionManager regionManager)
        {
            Control options;
            if (this.TryGetViewFor<OptionsViewModel>(out options))
            {
                regionManager.AddToRegion(RegionNames.Shell.Content, options);
            }

            NavigationView navigation = new NavigationBuilder()
                .ToView(options)
                .InRegion(regionManager.Regions[RegionNames.Shell.Content])
                .WithTitle(() => Labels.Options);

            regionManager.AddToRegion(RegionNames.Shell.Navigation, navigation);
        }

        protected override Infrastructure.Options.IOptions CreateModuleOptions()
        {
            return this.Container.Resolve<OptionsViewModel>();
        }

        protected override void ConfigureContainer(IUnityContainer container)
        {
            container.RegisterType<ICommand, ChangeLanguageCommand>("changeLanguageCommand");

            container.RegisterType<LanguageSelectionViewModel>(new ContainerControlledLifetimeManager(), new MapToRegistrationNames());

            container.RegisterType<ICommand, ChangeThemeCommand>("changeThemeCommand");
            container.RegisterType<ThemeSelectionViewModel>(new ContainerControlledLifetimeManager(), new MapToRegistrationNames());

            container.RegisterType<ICommand, TestNotificationConnectionCommand>("testNotificationConnectionCommand");

            container.RegisterType<OptionsViewModel>(
                new ContainerControlledLifetimeManager(),
                new SmartConstructor(new ResourceAccessor(() => Labels.Options)));
        }
    }
}
