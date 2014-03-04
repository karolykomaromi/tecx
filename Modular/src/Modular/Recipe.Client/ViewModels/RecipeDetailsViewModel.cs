namespace Recipe.ViewModels
{
    using System;
    using System.Diagnostics.Contracts;
    using Infrastructure.I18n;
    using Infrastructure.ViewModels;
    using Microsoft.Practices.Prism.Regions;
    using Recipe.Assets.Resources;
    using Recipe.Entities;

    public class RecipeDetailsViewModel : TitledViewModel, INavigationAware
    {
        private readonly IRecipeService recipeService;
        private readonly LocalizedString title;
        private Recipe recipe;

        public RecipeDetailsViewModel(IRecipeService recipeService)
        {
            Contract.Requires(recipeService != null);

            this.recipeService = recipeService;
            this.title = new LocalizedString(() => this.Title, () => Labels.Recipe, this.OnPropertyChanged);
        }

        public override string Title
        {
            get { return this.title.Value; }
        }

        public Recipe Recipe
        {
            get
            {
                return this.recipe;
            }

            set
            {
                if (this.recipe != value)
                {
                    this.OnPropertyChanging(() => this.Recipe);
                    this.recipe = value;
                    this.OnPropertyChanged(() => this.Recipe);
                }
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            long id;
            if (long.TryParse(navigationContext.Parameters["id"], out id))
            {
                this.recipeService.BeginGetRecipe(id, this.OnGetRecipeCompleted, null);
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            bool isTarget = navigationContext.Uri.ToString().StartsWith("RecipeDetailsView", StringComparison.OrdinalIgnoreCase);

            return isTarget;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        private void OnGetRecipeCompleted(IAsyncResult ar)
        {
            Recipe r = this.recipeService.EndGetRecipe(ar);

            this.Recipe = r;
        }
    }
}
