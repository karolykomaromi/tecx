using Infrastructure.Views;

namespace Recipe
{
    using System.Windows;
    using Infrastructure;
    using Infrastructure.I18n;
    using Infrastructure.ListViews;
    using Infrastructure.Modularity;
    using Infrastructure.ViewModels;
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

        public Module(IUnityContainer container, ILoggerFacade logger, IModuleInitializer initializer)
            : base(container, logger, initializer)
        {
        }

        protected override void ConfigureRegions(IRegionManager regionManager)
        {
            FrameworkElement view;

            ResxKey recipesTitle = new ResxKey("Recipe.Label_Recipes");
            ListViewName recipesList = new ListViewName("Recipes");

            if (this.TryGetViewFor<DynamicListViewModel>(out view, new Param("listViewName", recipesList), new Param("listViewTitleKey", recipesTitle)))
            {
                regionManager.AddToRegion(RegionNames.Shell.Content, view);

                NavigationView navigation = new NavigationBuilder()
                                                    .ToView(view)
                                                    .WithParameter("name", recipesList.ToString())
                                                    .InRegion(regionManager.Regions[RegionNames.Shell.Content])
                                                    .WithLabel(recipesTitle);

                regionManager.AddToRegion(RegionNames.Shell.Navigation, navigation);
            }

            ResxKey ingredientsTitle = new ResxKey("Recipe.Label_Ingredients");
            ListViewName ingredientsList = new ListViewName("Ingredients");

            if (this.TryGetViewFor<DynamicListViewModel>(out view, new Param("listViewName", ingredientsList), new Param("listViewTitleKey", ingredientsTitle)))
            {
                regionManager.AddToRegion(RegionNames.Shell.Content, view);

                NavigationView navigation = new NavigationBuilder()
                                                .ToView(view)
                                                .WithParameter("name", ingredientsList.ToString())
                                                .InRegion(regionManager.Regions[RegionNames.Shell.Content])
                                                .WithLabel(ingredientsTitle);

                regionManager.AddToRegion(RegionNames.Shell.Navigation, navigation);
            }
        }

        protected override IResourceManager CreateResourceManager()
        {
            return new ResxFilesResourceManager(typeof(Labels));
        }
    }
}
