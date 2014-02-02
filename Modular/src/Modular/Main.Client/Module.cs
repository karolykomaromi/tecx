namespace Main
{
    using System.Windows;
    using System.Windows.Input;
    using Infrastructure;
    using Infrastructure.I18n;
    using Infrastructure.Modularity;
    using Infrastructure.ViewModels;
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
            FrameworkElement language;
            if (this.TryGetViewFor<LanguageSelectionViewModel>(out language))
            {
                regionManager.AddToRegion(RegionNames.Shell.MenuRight, language);
            }

            FrameworkElement theme;
            if (this.TryGetViewFor<ThemeSelectionViewModel>(out theme))
            {
                regionManager.AddToRegion(RegionNames.Shell.MenuLeft, theme);
            }

            FrameworkElement options;
            if (this.TryGetViewFor<OptionsViewModel>(out options, new Param("titleKey", new ResxKey("Label_Options"))))
            {
                regionManager.AddToRegion(RegionNames.Shell.Content, options);
            }

            NavigationViewModel navigation = new NavigationBuilder().ToView(options)
                .InRegion(regionManager.Regions[RegionNames.Shell.Content])
                .WithLabel(new ResxKey("Label_Options"));

            regionManager.AddToRegion(RegionNames.Shell.Navigation, navigation);
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
        }
    }
}
