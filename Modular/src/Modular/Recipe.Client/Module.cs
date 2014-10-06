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
    using Recipe.ViewModels;

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

                NavigationView navigation = new NavigationBuilder(this.RegionManager).ToView(recipes);

                regionManager.AddToRegion(RegionNames.Shell.Navigation, navigation);
            }

            ListViewId ingredientsList = new ListViewId("Recipe", "Ingredients");

            Control ingredients;
            if (this.TryGetViewFor<DynamicListViewModel>(out ingredients, new Parameter("listViewId", ingredientsList)))
            {
                regionManager.AddToRegion(RegionNames.Shell.Content, ingredients);

                NavigationView navigation = new NavigationBuilder(this.RegionManager).ToView(ingredients);

                regionManager.AddToRegion(RegionNames.Shell.Navigation, navigation);
            }

            Control ingredientDetails;
            if (this.TryGetViewFor<IngredientDetailsViewModel>(out ingredientDetails))
            {
                regionManager.AddToRegion(RegionNames.Shell.Content, ingredientDetails);
            }

            Control recipeDetails;
            if (this.TryGetViewFor<RecipeDetailsViewModel>(out recipeDetails))
            {
                regionManager.AddToRegion(RegionNames.Shell.Content, recipeDetails);
            }
        }
    }
}
