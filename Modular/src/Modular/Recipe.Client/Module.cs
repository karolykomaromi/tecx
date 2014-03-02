namespace Recipe
{
    using System.Windows.Controls;
    using Infrastructure;
    using Infrastructure.ListViews;
    using Infrastructure.Modularity;
    using Infrastructure.UnityExtensions.Injection;
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

        public Module(IUnityContainer container, ILoggerFacade logger, IModuleTracker moduleTracker, IRegionManager regionManager)
            : base(container, logger, moduleTracker, regionManager)
        {
        }

        public override string ModuleName
        {
            get { return Name; }
        }

        protected override void ConfigureRegions(IRegionManager regionManager)
        {
            ListViewId recipesList = new ListViewId("Recipe", "Recipes");

            Control recipes;
            if (this.TryGetViewFor<DynamicListViewModel>(out recipes, new Parameter("listViewId", recipesList)))
            {
                regionManager.AddToRegion(RegionNames.Shell.Content, recipes);

                NavigationView navigation = new NavigationBuilder(this.RegionManager)
                                                    .ToView(recipes)
                                                    .WithParameter("name", recipesList.ToString())
                                                    .WithTitle(() => Labels.Recipes);

                regionManager.AddToRegion(RegionNames.Shell.Navigation, navigation);
            }

            ListViewId ingredientsList = new ListViewId("Recipe", "Ingredients");

            Control ingredients;
            if (this.TryGetViewFor<DynamicListViewModel>(out ingredients, new Parameter("listViewId", ingredientsList)))
            {
                regionManager.AddToRegion(RegionNames.Shell.Content, ingredients);

                NavigationView navigation = new NavigationBuilder(this.RegionManager)
                                                .ToView(ingredients)
                                                .WithParameter("name", ingredientsList.ToString())
                                                .WithTitle(() => Labels.Ingredients);

                regionManager.AddToRegion(RegionNames.Shell.Navigation, navigation);
            }
        }
    }
}
