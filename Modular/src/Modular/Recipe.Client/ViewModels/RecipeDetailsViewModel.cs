namespace Recipe.ViewModels
{
    using System;
    using Infrastructure.I18n;
    using Infrastructure.ViewModels;
    using Microsoft.Practices.Prism.Regions;
    using Recipe.Assets.Resources;

    public class RecipeDetailsViewModel : TitledViewModel, INavigationAware
    {
        private readonly LocalizedString title;

        public RecipeDetailsViewModel()
        {
            this.title = new LocalizedString(() => this.Title, () => Labels.Recipe, this.OnPropertyChanged);
        }

        public override string Title
        {
            get { return this.title.Value; }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            bool isTarget = navigationContext.Uri.ToString().StartsWith("RecipeDetailsView", StringComparison.OrdinalIgnoreCase);

            return isTarget;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}
