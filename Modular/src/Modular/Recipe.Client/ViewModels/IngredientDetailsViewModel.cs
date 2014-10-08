namespace Recipe.ViewModels
{
    using System;
    using System.Diagnostics.Contracts;
    using AutoMapper;
    using Infrastructure.I18n;
    using Infrastructure.ViewModels;
    using Microsoft.Practices.Prism.Regions;
    using Recipe.Assets.Resources;
    using Recipe.Entities;

    public class IngredientDetailsViewModel : TitledViewModel, INavigationAware
    {
        private readonly IRecipeService recipeService;
        private readonly IMappingEngine mappingEngine;
        private readonly LocalizedString title;
        private IngredientViewModel ingredient;

        public IngredientDetailsViewModel(IRecipeService recipeService, IMappingEngine mappingEngine)
        {
            Contract.Requires(recipeService != null);
            Contract.Requires(mappingEngine != null);

            this.recipeService = recipeService;
            this.mappingEngine = mappingEngine;
            this.title = new LocalizedString(() => this.Title, () => Labels.Ingredient, this.OnPropertyChanged);
        }

        public override string Title
        {
            get { return this.title.Value; }
        }

        public IngredientViewModel Ingredient
        {
            get
            {
                return this.ingredient;
            }

            set
            {
                if (this.ingredient != value)
                {
                    this.OnPropertyChanging(() => this.Ingredient);
                    this.ingredient = value;
                    this.OnPropertyChanged(() => this.Ingredient);
                }
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            long id;
            if (long.TryParse(navigationContext.Parameters["id"], out id))
            {
                this.recipeService.BeginGetIngredient(id, this.OnGetIngredientCompleted, null);
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            bool isTarget = navigationContext.Uri.ToString().StartsWith("IngredientDetailsView", StringComparison.OrdinalIgnoreCase);

            return isTarget;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        private void OnGetIngredientCompleted(IAsyncResult ar)
        {
            Ingredient i = this.recipeService.EndGetIngredient(ar);

            this.Ingredient = this.mappingEngine.Map<Ingredient, IngredientViewModel>(i);
        }
    }
}
