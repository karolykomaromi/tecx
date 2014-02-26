namespace Recipe
{
    using System.Windows.Controls;
    using Infrastructure;
    using Infrastructure.I18n;
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
            ListViewName recipesList = new ListViewName("Recipe.Recipes");

            Control recipes;
            if (this.TryGetViewFor<DynamicListViewModel>(out recipes, new Parameter("listViewName", recipesList), new Parameter("title", new ResourceAccessor(() => Labels.Recipes))))
            {
                regionManager.AddToRegion(RegionNames.Shell.Content, recipes);

                NavigationView navigation = new NavigationBuilder(this.RegionManager)
                                                    .ToView(recipes)
                                                    .WithParameter("name", recipesList.ToString())
                                                    .WithTitle(() => Labels.Recipes);

                regionManager.AddToRegion(RegionNames.Shell.Navigation, navigation);
            }

            ListViewName ingredientsList = new ListViewName("Recipe.Ingredients");

            Control ingredients;
            if (this.TryGetViewFor<DynamicListViewModel>(out ingredients, new Parameter("listViewName", ingredientsList), new Parameter("title", new ResourceAccessor(() => Labels.Ingredients))))
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
