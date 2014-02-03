namespace Main
{
    using System.Windows;
    using System.Windows.Input;
    using Infrastructure;
    using Infrastructure.I18n;
    using Infrastructure.Modularity;
    using Infrastructure.Views;
    using Main.Assets.Resources;
    using Main.Commands;
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

        public Module(IUnityContainer container, ILoggerFacade logger, IModuleInitializer initializer)
            : base(container, logger, initializer)
        {
        }

        protected override void ConfigureRegions(IRegionManager regionManager)
        {
            FrameworkElement options;
            if (this.TryGetViewFor<OptionsViewModel>(out options))
            {
                regionManager.AddToRegion(RegionNames.Shell.Content, options);
            }

            NavigationView navigation = new NavigationBuilder()
                .ToView(options)
                .InRegion(regionManager.Regions[RegionNames.Shell.Content])
                .WithLabel(new ResxKey("MAIN.LABEL_OPTIONS"));

            regionManager.AddToRegion(RegionNames.Shell.Navigation, navigation);
        }

        protected override Infrastructure.Options.IOptions CreateModuleOptions()
        {
            return this.Container.Resolve<OptionsViewModel>();
        }

        protected override IResourceManager CreateResourceManager()
        {
            return new ResxFilesResourceManager(typeof(Labels));
        }

        protected override void ConfigureContainer(IUnityContainer container)
        {
            container.RegisterType<ICommand, ChangeLanguageCommand>("language");
            container.RegisterType<LanguageSelectionViewModel>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(new ResolvedParameter<ICommand>("language")));

            container.RegisterType<ICommand, ChangeThemeCommand>("theme");
            container.RegisterType<ThemeSelectionViewModel>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(new ResolvedParameter<ICommand>("theme")));

            container.RegisterType<OptionsViewModel>(
                new ContainerControlledLifetimeManager(), 
                new InjectionConstructor(new ResxKey("MAIN.LABEL_OPTIONS"), typeof(LanguageSelectionViewModel), typeof(ThemeSelectionViewModel)));
        }
    }
}
