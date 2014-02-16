namespace Recipe
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using Infrastructure;
    using Infrastructure.I18n;
    using Infrastructure.ListViews;
    using Infrastructure.Modularity;
    using Infrastructure.ViewModels;
    using Infrastructure.Views;
    using Microsoft.Practices.Prism.Logging;
    using Microsoft.Practices.Prism.Regions;
    using Microsoft.Practices.Unity;
    using Recipe.Assets.Resources;

    public class Module : UnityModule
    {
        /// <summary>
        /// Recipe
        /// </summary>
        public static readonly string Name = "Recipe";

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
            ListViewName recipesList = new ListViewName("Recipes");

            Control recipes;
            if (this.TryGetViewFor<DynamicListViewModel>(out recipes, new Param("listViewName", recipesList), new Param("title", new ResourceAccessor(() => Labels.Recipes))))
            {
                regionManager.AddToRegion(RegionNames.Shell.Content, recipes);

                NavigationView navigation = new NavigationBuilder()
                                                    .ToView(recipes)
                                                    .WithParameter("name", recipesList.ToString())
                                                    .InRegion(regionManager.Regions[RegionNames.Shell.Content])
                                                    .WithTitle(() => Labels.Recipes);

                regionManager.AddToRegion(RegionNames.Shell.Navigation, navigation);
            }

            ListViewName ingredientsList = new ListViewName("Ingredients");

            Control ingredients;
            if (this.TryGetViewFor<DynamicListViewModel>(out ingredients, new Param("listViewName", ingredientsList), new Param("title", new ResourceAccessor(() => Labels.Ingredients))))
            {
                regionManager.AddToRegion(RegionNames.Shell.Content, ingredients);

                NavigationView navigation = new NavigationBuilder()
                                                .ToView(ingredients)
                                                .WithParameter("name", ingredientsList.ToString())
                                                .InRegion(regionManager.Regions[RegionNames.Shell.Content])
                                                .WithTitle(() => Labels.Ingredients);

                regionManager.AddToRegion(RegionNames.Shell.Navigation, navigation);
            }
        }

        protected override ResourceDictionary CreateModuleResources()
        {
            Uri source = new Uri("/Recipe.Client;component/Assets/Resources/Resources.xaml", UriKind.Relative);

            ResourceDictionary resources = new ResourceDictionary { Source = source };

            return resources;
        }
    }
}
